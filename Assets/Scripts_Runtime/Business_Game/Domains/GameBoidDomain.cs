using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Air {

    public static class GameBoidDomain {

        public static BoidEntity Spawn(GameBusinessContext ctx, int typeID, Vector2 pos, AllyStatus allyStatus) {
            var Boid = GameFactory.Boid_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              ctx.idRecordService,
                                              ctx.randomService,
                                              typeID,
                                              pos,
                                              allyStatus);
            ctx.boidRepo.Add(Boid);
            return Boid;
        }

        public static void SpawnAll(GameBusinessContext ctx, BoidTM[] BoidTMArr, Vector2[] posArr, AllyStatus[] allyStatusArr) {
            for (int i = 0; i < BoidTMArr.Length; i++) {
                var tm = BoidTMArr[i];
                if (tm == null) continue;
                var pos = posArr[i];
                var allyStatus = allyStatusArr[i];
                Spawn(ctx, tm.typeID, pos, allyStatus);
            }
        }

        public static void CheckAndUnSpawn(GameBusinessContext ctx, BoidEntity boid) {
            if (boid.needTearDown) {
                UnSpawn(ctx, boid);
            }
        }

        public static void UnSpawn(GameBusinessContext ctx, BoidEntity boid) {
            ctx.boidRepo.Remove(boid);
            boid.TearDown();
        }

        // CS
        public static void InitCS(GameBusinessContext ctx, int typeID) {
            var config = ctx.templateInfraContext.Config_Get();
            var compute = config.boidCS;
            var has = ctx.templateInfraContext.Boid_TryGet(typeID, out var boidTM);
            if (!has) {
                GLog.LogError("SetCS: boidTM not found: " + typeID);
                return;
            }

            compute.SetFloat("viewRadius", boidTM.viewRadius);
            compute.SetFloat("avoidRadius", boidTM.avoidRadius);
        }

        public static void ProcessCS(GameBusinessContext ctx, int typeID, float dt) {
            var boidLen = ctx.boidRepo.TakeAll(out var boids);
            var csModel = ctx.boidCSModelTemp;

            if (csModel == null || csModel.Length != boidLen) {
                csModel = new BoidCSModel[boidLen];
                ctx.boidCSModelTemp = csModel;
            }

            for (int i = 0; i < boidLen; i++) {
                csModel[i].position = boids[i].pos;
                csModel[i].direction = boids[i].up;
            }

            if (ctx.boidBuffer == null || ctx.boidBuffer.count != boidLen) {
                if (ctx.boidBuffer != null) {
                    ctx.boidBuffer.Release();
                    ctx.boidBuffer = null;
                }
                ctx.boidBuffer = new ComputeBuffer(boidLen, BoidCSModel.Size);
            }

            if (csModel == null) {
                return;
            }

            ctx.boidBuffer.SetData(csModel, 0, 0, boidLen);

            var config = ctx.templateInfraContext.Config_Get();
            var compute = config.boidCS;

            var has = ctx.templateInfraContext.Boid_TryGet(typeID, out var boidTM);
            if (!has) {
                GLog.LogError("SetCS: boidTM not found: " + typeID);
                return;
            }

            var owener = ctx.Leader_GetOwner();

            compute.SetBuffer(0, "boids", ctx.boidBuffer);
            compute.SetInt("boidsCount", boidLen);

            int threadGroupSize = 256;
            int threadGroups = Mathf.CeilToInt(boidLen / (float)threadGroupSize);
            compute.Dispatch(0, threadGroups, 1, 1);

            ctx.boidBuffer.GetData(csModel, 0, 0, boidLen);
            for (int i = 0; i < boidLen; i++) {
                boids[i].csModel = csModel[i];
            }
        }

        public static void ReleaseCS(GameBusinessContext ctx) {
            ctx.boidBuffer.Release();
        }

        public static void ApplyMove(GameBusinessContext ctx, BoidEntity boid, float dt) {
            var boidData = boid.csModel;
            var alignment = boidData.alignment;
            var separation = boidData.separation;
            var otherNum = boidData.cohesionCount;
            var center = boidData.cohesionCenter;
            var cohesion = otherNum > 0 ? center / otherNum - boid.pos : Vector3.zero;

            var typeID = boid.typeID;
            var has = ctx.templateInfraContext.Boid_TryGet(typeID, out var boidTM);
            if (!has) {
                GLog.LogError("SetCS: boidTM not found: " + typeID);
            }

            var acceleration = Vector3.zero;

            var follow = ctx.Leader_GetOwner().Pos - boid.pos;
            var sqrDst = follow.x * follow.x + follow.y * follow.y;
            var followForce = Vector3.zero;
            if (sqrDst < boidTM.followRadius * boidTM.followRadius) {
                followForce = SteerTowards(ctx, follow, boid) * boidTM.followWeight;
            }
            acceleration += followForce;

            var separationForce = SteerTowards(ctx, separation, boid) * boidTM.separationWeight;
            acceleration += separationForce;

            var alignmentForce = SteerTowards(ctx, alignment, boid) * boidTM.alignmentWeight;
            acceleration += alignmentForce;

            var cohesionForce = SteerTowards(ctx, cohesion, boid) * boidTM.cohesionWeight;
            acceleration += cohesionForce;

            Move(ctx, boid, acceleration, boidTM.minSpeed, boidTM.maxSpeed, dt, false);
        }

        static void Move(GameBusinessContext ctx, BoidEntity boid, Vector3 acceleration, float minSpeed, float maxSpeed, float fixdt, bool hasNoBoids) {
            var velocity = boid.velocity;
            velocity += acceleration * fixdt;

            float speed = velocity.magnitude;
            Vector3 dir = velocity / speed;
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
            velocity = dir * speed;
            boid.Velocity_Set(velocity);

            var pos = boid.transform.position;
            pos += velocity * fixdt;
            boid.Move_SetUp(dir);
            boid.transform.up = dir;

            if (hasNoBoids && speed < minSpeed) {
                Debug.Log("No Boids: Speed =" + speed);
                return;
            }

            boid.Pos_SetPos(pos);
            boid.transform.position = pos;
        }

        static Vector3 SteerTowards(GameBusinessContext ctx, Vector3 vector, BoidEntity boid) {
            var typeID = boid.typeID;
            var has = ctx.templateInfraContext.Boid_TryGet(typeID, out var boidTM);
            if (!has) {
                GLog.LogError("SteerTowards: boidTM not found: " + typeID);
            }
            Vector3 v = vector.normalized * boidTM.maxSpeed - boid.velocity;
            return Vector3.ClampMagnitude(v, boidTM.maxSteerForce);
        }

        public static void ApplyConstraint(GameBusinessContext ctx, BoidEntity boid, float dt) {
            var map = ctx.currentMapEntity;
            var size = map.mapSize;
            var center = map.transform.position;
            var min = center - size / 2;
            var max = center + size / 2;
            var pos = boid.pos;
            if (pos.x < min.x || pos.x > max.x || pos.y < min.y || pos.y > max.y) {
                MoveToOppoSide(ctx, boid, max, min);
            }
        }

        static void MoveToOppoSide(GameBusinessContext ctx, BoidEntity boid, Vector2 max, Vector2 min) {
            Vector3 pos = boid.pos;
            var epsilon = 0.4f;
            if (pos.y >= max.y) {
                var offset = pos.y - max.y;
                pos.y = min.y + offset + epsilon;
            }
            if (pos.y <= min.y) {
                var offset = min.y - pos.y;
                pos.y = max.y - offset - epsilon;
            }
            if (pos.x >= max.x) {
                var offset = pos.x - max.x;
                pos.x = min.x + offset + epsilon;
            }
            if (pos.x <= min.x) {
                var offset = min.x - pos.x;
                pos.x = max.x - offset - epsilon;
            }
            boid.Pos_SetPos(pos);
            boid.transform.position = pos;
            boid.Trail_Clear();
        }

    }

}
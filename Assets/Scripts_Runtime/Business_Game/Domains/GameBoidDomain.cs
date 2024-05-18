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

        public static void ApplyMove(GameBusinessContext ctx, BoidEntity boid, float dt) {
            Boid_Move(ctx, boid, dt);
        }

        // CS
        public static void ProcessCS(GameBusinessContext ctx, float dt) {
            var boidLen = ctx.boidRepo.TakeAll(out var boids);
            var boidData = new BoidData[boidLen];

            for (int i = 0; i < boidLen; i++) {
                boidData[i].position = boids[i].Pos;
                boidData[i].direction = boids[i].Up;
            }

            var boidBuffer = new ComputeBuffer(boidLen, BoidData.Size);
            boidBuffer.SetData(boidData);

            var config = ctx.templateInfraContext.Config_Get();
            var compute = config.boidCS;

            var has = ctx.templateInfraContext.Boid_TryGet(1, out var boidTM);
            if (!has) {
                GLog.LogError("SetCS: boidTM not found: " + 1);
            }

            compute.SetBuffer(0, "boids", boidBuffer);
            compute.SetInt("numBoids", boidLen);
            compute.SetFloat("viewRadius", boidTM.alignmentRadius);
            compute.SetFloat("avoidRadius", boidTM.separationRadius);

            int threadGroupSize = 1024;
            int threadGroups = Mathf.CeilToInt(boidLen / (float)threadGroupSize);
            compute.Dispatch(0, threadGroups, 1, 1);

            boidBuffer.GetData(boidData);

            for (int i = 0; i < boidLen; i++) {
                var boid = boids[i];
                var alignment = boidData[i].alignment;
                var separation = boidData[i].separation;
                var otherNum = boidData[i].otherCount;
                var center = boidData[i].cohesionCenter;
                var cohesion = otherNum > 0 ? center / otherNum - boid.Pos : Vector3.zero;

                var acceleration = Vector3.zero;
                var separationForce = SteerTowards(ctx, separation, boid) * boidTM.separationWeight;
                acceleration += separationForce;

                var alignmentForce = SteerTowards(ctx, alignment, boid) * boidTM.alignmentWeight;
                acceleration += alignmentForce;

                var cohesionForce = SteerTowards(ctx, cohesion, boid) * boidTM.cohesionWeight;
                acceleration += cohesionForce;

                Move(ctx, boid, acceleration, boidTM.minSpeed, boidTM.maxSpeed, dt, false);
            }

            boidBuffer.Release();
        }

        // Boids AI
        public static void Boid_Move(GameBusinessContext ctx, BoidEntity boid, float fixdt) {
        }

        static void Move(GameBusinessContext ctx, BoidEntity boid, Vector3 acceleration, float minSpeed, float maxSpeed, float fixdt, bool hasNoBoids) {
            var velocity = boid.velocity;
            velocity += acceleration * fixdt;

            float speed = velocity.magnitude;
            Vector3 dir = velocity / speed;
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
            velocity = dir * speed;
            boid.Velocity_Set(velocity);

            var pos = boid.Pos;
            pos += velocity * fixdt;
            boid.Move_SetUp(dir);

            if (hasNoBoids && speed < minSpeed) {
                Debug.Log("No Boids: Speed =" + speed);
                return;
            }

            var oldPos = boid.Pos;
            boid.Pos_SetPos(pos);
            ctx.boidRepo.UpdatePos(boid, oldPos);
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
            var pos = boid.Pos;
            if (pos.x < min.x || pos.x > max.x || pos.y < min.y || pos.y > max.y) {
                MoveToOppoSide(ctx, boid, max, min);
            }
        }

        static void MoveToOppoSide(GameBusinessContext ctx, BoidEntity boid, Vector2 max, Vector2 min) {
            Vector3 pos = boid.Pos;
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
        }

    }

}
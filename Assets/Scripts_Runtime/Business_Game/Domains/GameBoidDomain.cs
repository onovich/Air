using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Air {

    public static class GameBoidDomain {

        public static BoidEntity Spawn(GameBusinessContext ctx, int typeID, Vector2 pos, AllyStatus allyStatus) {
            var Boid = GameFactory.Boid_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              ctx.idRecordService,
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

        // Boids AI
        public static void Boid_Move(GameBusinessContext ctx, BoidEntity boid, float fixdt) {
            var pos = boid.Pos;
            var posInt = boid.GridPos;
            var allyStatus = boid.allyStatus;
            var acceleration = Vector2.zero;

            var has = ctx.templateInfraContext.Boid_TryGet(boid.typeID, out var boidTM);
            if (!has) {
                GLog.LogError("Boid_Move: boidTM not found: " + boid.typeID);
            }

            var boidLen = ctx.boidRepo.TryGetAround(boid.entityID, allyStatus, posInt, 4, 10, out var boids);
            if (boidLen == 0) {
                acceleration = boid.velocity;
                ApplyMove(ctx, boid, acceleration, boidTM.minSpeed, boidTM.maxSpeed, fixdt);
                return;
            }

            var separation = SteerTowards(ctx, Boid_GetSeparationVector(ctx, pos, boids, boidLen,
            boidTM.separationRadius), boid) * boidTM.separationWeight;
            acceleration += separation;

            var alignment = SteerTowards(ctx, Boid_GetAlignmentVector(ctx, pos, boids, boidLen,
            boidTM.alignmentRadius), boid) * boidTM.alignmentWeight;
            acceleration += alignment;

            var cohesion = SteerTowards(ctx, Boid_GetCohesionVector(ctx, pos, boids, boidLen,
            boidTM.cohesionRadius), boid) * boidTM.cohesionWeight;
            acceleration += cohesion;

            ApplyMove(ctx, boid, acceleration, boidTM.minSpeed, boidTM.maxSpeed, fixdt);
        }

        static void ApplyMove(GameBusinessContext ctx, BoidEntity boid, Vector2 acceleration, float minSpeed, float maxSpeed, float fixdt) {
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

            var oldPos = boid.Pos;
            boid.Pos_SetPos(pos);
            ctx.boidRepo.UpdatePos(boid, oldPos);
        }

        static Vector2 SteerTowards(GameBusinessContext ctx, Vector2 vector, BoidEntity boid) {
            var typeID = boid.typeID;
            var has = ctx.templateInfraContext.Boid_TryGet(typeID, out var boidTM);
            if (!has) {
                GLog.LogError("SteerTowards: boidTM not found: " + typeID);
            }
            Vector2 v = vector.normalized * boidTM.maxSpeed - boid.velocity;
            return Vector2.ClampMagnitude(v, boidTM.maxSteerForce);
        }

        static Vector2 Boid_GetSeparationVector(GameBusinessContext ctx, Vector2 boidPos, BoidEntity[] boids, int boidLen, float radius) {
            var separation = Vector2.zero;
            if (boidLen == 0) {
                return separation;
            }
            for (int i = 0; i < boidLen; i += 1) {
                var other = boids[i];
                var offset = other.Pos - boidPos;
                float sqrDst = offset.x * offset.x + offset.y * offset.y;

                if (sqrDst >= radius * radius) {
                    continue;
                }
                // if (sqrDst < 0.01f) {
                //     // separation -= ctx.randomService.InsideUnitCircle();
                //     continue;
                // }
                separation -= offset / sqrDst;
            }
            return separation.normalized;
        }

        static Vector2 Boid_GetAlignmentVector(GameBusinessContext ctx, Vector2 boidPos, BoidEntity[] boids, int boidLen, float radius) {
            var alignment = Vector2.zero;
            if (boidLen == 0) {
                return alignment;
            }
            for (int i = 0; i < boidLen; i += 1) {
                var other = boids[i];
                var offset = other.Pos - boidPos;
                float sqrDst = offset.x * offset.x + offset.y * offset.y;

                if (sqrDst >= radius * radius) {
                    continue;
                }

                // if (sqrDst < 0.01f) {
                //     continue;
                // }
                var dir = other.velocity;
                alignment += dir;
            }
            return alignment.normalized;
        }

        static Vector2 Boid_GetCohesionVector(GameBusinessContext ctx, Vector2 boidPos, BoidEntity[] boids, int boidLen, float radius) {
            var cohesion = Vector2.zero;
            if (boidLen == 0) {
                return cohesion;
            }
            for (int i = 0; i < boidLen; i += 1) {
                var other = boids[i];
                var offset = other.Pos - boidPos;
                float sqrDst = offset.x * offset.x + offset.y * offset.y;

                if (sqrDst >= radius * radius) {
                    continue;
                }

                // if (sqrDst < 0.01f) {
                //     continue;
                // }

                var otherPos = other.Pos;
                cohesion += otherPos;
            }
            cohesion = cohesion / boidLen;
            var dir = cohesion - boidPos;
            return dir.normalized;
        }

        public static void ApplyConstraint(GameBusinessContext ctx, BoidEntity boid, float dt) {
            var map = ctx.currentMapEntity;
            var size = map.mapSize;
            var center = map.transform.position;
            var min = center - size / 2;
            var max = center + size / 2;
            var pos = boid.Pos;
            if (pos.x < min.x || pos.x > max.x || pos.y < min.y || pos.y > max.y) {
                // boid.Attr_DeadlyHurt();
                MoveToOppoSide(ctx, boid, max, min);
            }
        }

        static void MoveToOppoSide(GameBusinessContext ctx, BoidEntity boid, Vector2 max, Vector2 min) {
            Vector3 pos = Vector2.zero;
            float x = boid.Pos.x;
            float y = boid.Pos.y;
            if (y >= max.y) {
                pos.y = -max.y;

            }
            if (y <= min.y) {
                pos.y = max.y;
            }
            if (x >= max.x) {
                pos.x = min.x;
            }
            if (x <= min.x) {
                pos.x = max.x;
            }
            boid.Pos_SetPos(pos);
        }

    }

}
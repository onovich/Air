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

            var boidLen = ctx.boidRepo.TryGetAround(boid.entityID, allyStatus, posInt, 10, 10, out var boids);
            if (boidLen <= 1) {
                // TODO: Wandering
                return;
            }

            var separation = Boid_GetSeparationVector(ctx, pos, boids, boidLen, 0.9f) * 10;
            var dir = separation;

            if (dir.sqrMagnitude < 0.01f) {
                dir = ctx.randomService.InsideUnitCircle();
            } else {
                dir = dir.normalized;
            }
            boid.inputCom.moveAxis = dir;

            // Move
            var oldPos = boid.Pos;
            boid.Move_ApplyMove(fixdt);
            ctx.boidRepo.UpdatePos(boid, oldPos);

        }

        static Vector2 Boid_GetSeparationVector(GameBusinessContext ctx, Vector2 boidPos, BoidEntity[] boids, int boidLen, float radius) {


            var separation = Vector2.zero;

            if (boidLen == 0) {
                return separation;
            }

            for (int i = 0; i < boidLen; i += 1) {
                var other = boids[i];
                var otherPos = other.Pos;
                var dir = boidPos - otherPos;
                var sqrDist = dir.sqrMagnitude;
                if (sqrDist >= radius * radius) {
                    continue;
                }
                if (sqrDist < 0.01f) {
                    dir = ctx.randomService.InsideUnitCircle();
                } else {
                    dir = dir.normalized;
                }
                separation += dir * (1f / sqrDist);
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
                var otherPos = other.Pos;
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
                var otherPos = other.Pos;
                cohesion += otherPos;
            }

            cohesion = cohesion / boidLen;
            var dir = cohesion - boidPos;

            return dir.normalized;
        }

        public static void ApplyConstraint(GameBusinessContext ctx, BoidEntity Boid, float dt) {
            var map = ctx.currentMapEntity;
            var size = map.mapSize;
            var center = map.transform.position;
            var min = center - size / 2;
            var max = center + size / 2;
            var pos = Boid.Pos;
            if (pos.x < min.x || pos.x > max.x || pos.y < min.y || pos.y > max.y) {
                Boid.Attr_DeadlyHurt();
                Debug.Log($"Dead: pos = {pos}, min = {min}, max = {max}, center = {center}, size = {size}");
            }
        }

    }

}
using UnityEngine;

namespace Air {

    public static class GameBoidDomain {

        public static BoidEntity Spawn(GameBusinessContext ctx, int typeID, Vector2 pos) {
            var Boid = GameFactory.Boid_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              ctx.idRecordService,
                                              typeID,
                                              pos);
            ctx.boidRepo.Add(Boid);
            return Boid;
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

        static void OnFootEnterSpike(GameBusinessContext ctx, BoidEntity Boid) {
            // - Enter Spike
            Boid.Attr_GetHurt();
        }

        static void OnBodyTriggerEnter(GameBusinessContext gameContext, BoidEntity Boid, Collider2D other) {
            // Eat Star
        }

        public static void ApplyMove(GameBusinessContext ctx, BoidEntity Boid, float dt) {
            Boid.Move_ApplyMove(dt);
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
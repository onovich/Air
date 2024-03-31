using UnityEngine;

namespace Air {

    public static class GameBoidDomain {

        public static BoidEntity Spawn(GameBusinessContext ctx, int typeID, Vector2 pos) {
            var boid = GameFactory.Boid_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              ctx.idRecordService,
                                              typeID,
                                              pos);
            ctx.boidRepo.Add(boid);
            RecordLastPos(ctx, boid);
            return boid;
        }

        public static void UnSpawn(GameBusinessContext ctx, BoidEntity boid) {
            ctx.boidRepo.Remove(boid);
            boid.TearDown();
        }

        public static void RecordLastPos(GameBusinessContext ctx, BoidEntity boid) {
            boid.Pos_RecordLastPosInt();
        }

        public static void UpdatePosDict(GameBusinessContext ctx, BoidEntity boid) {
            if (!boid.Pos_IsDifferentFromLast()) {
                return;
            }
            ctx.Boid_UpdatePosDict(boid);
            RecordLastPos(ctx, boid);
        }

        public static void ApplyMove(GameBusinessContext ctx, BoidEntity boid, float dt) {

            var player = ctx.playerEntity;
            var owner = ctx.Boid_GetOwner();

            if (owner.inputCom.moveAxis == Vector2.zero) {
                boid.Move_Stop();
                // if (owner.inputCom.autoMove) {
                //     boid.Move_MoveToTarget(owner.inputCom.movingTarget, .1f, dt);
                // }
            } else if (owner.inputCom.moveAxis != Vector2.zero) {
                boid.Move_ApplyMove(dt);
                // boid.inputCom.autoMove = false;
            }

        }

    }

}
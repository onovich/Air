using UnityEngine;

namespace Air {

    public static class GameLeaderDomain {

        public static LeaderEntity Spawn(GameBusinessContext ctx, int typeID, Vector2 pos, AllyStatus allyStatus) {
            var leader = GameFactory.Leader_Spawn(ctx.templateInfraContext,
                                                  ctx.assetsInfraContext,
                                                  ctx.idRecordService,
                                                  typeID,
                                                  pos,
                                                  allyStatus);
            ctx.leaderRepo.Add(leader);
            return leader;
        }

        public static void SpawnAll(GameBusinessContext ctx, LeaderTM[] leaderTMArr, Vector2[] posArr, AllyStatus[] allyStatusArr) {
            for (int i = 0; i < leaderTMArr.Length; i++) {
                var tm = leaderTMArr[i];
                if (tm == null) continue;
                var pos = posArr[i];
                var allyStatus = allyStatusArr[i];
                Spawn(ctx, tm.typeID, pos, allyStatus);
            }
        }

        public static void CheckAndUnSpawn(GameBusinessContext ctx, LeaderEntity leader) {
            if (leader.needTearDown) {
                UnSpawn(ctx, leader);
            }
        }

        public static void ApplyConstraint(GameBusinessContext ctx, LeaderEntity leader, float dt) {
            var map = ctx.currentMapEntity;
            var size = map.mapSize;
            var center = map.transform.position;
            var min = center - size / 2;
            var max = center + size / 2;
            var pos = leader.Pos;
            if (pos.x < min.x || pos.x > max.x || pos.y < min.y || pos.y > max.y) {
                MoveToOppoSide(ctx, leader, max, min);
            }
        }

        public static void ApplyMove(GameBusinessContext ctx, LeaderEntity leader, float dt) {
            // Move Up
            var moveSpeed = leader.moveSpeed;
            var dir = leader.transform.up;
            var moveDelta = dir * moveSpeed * dt;
            leader.transform.position += moveDelta;

            // Rotate
            var rotateAxis = leader.inputCom.moveAxis;
            var rotateSpeed = leader.rotationSpeed;
            var rotateDelta = rotateAxis.x * rotateSpeed * dt;
            leader.transform.Rotate(0, 0, -rotateDelta);
        }

        public static void UnSpawn(GameBusinessContext ctx, LeaderEntity leader) {
            ctx.leaderRepo.Remove(leader);
            leader.TearDown();
        }

        static void MoveToOppoSide(GameBusinessContext ctx, LeaderEntity leader, Vector2 max, Vector2 min) {
            Vector3 pos = leader.Pos;
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
            leader.Pos_SetPos(pos);
        }

    }

}
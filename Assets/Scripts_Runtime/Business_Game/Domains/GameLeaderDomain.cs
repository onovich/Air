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

        public static void UnSpawn(GameBusinessContext ctx, LeaderEntity leader) {
            ctx.leaderRepo.Remove(leader);
            leader.TearDown();
        }

    }

}
using UnityEngine;

namespace Air {

    public static class GameBlockDomain {

        public static BlockEntity Spawn(GameBusinessContext ctx, int typeID, Vector2 pos, Vector2 size, int index) {
            var block = GameFactory.Block_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              ctx.idRecordService,
                                              typeID,
                                              pos,
                                              size,
                                              index);
            ctx.blockRepo.Add(block);
            return block;
        }

        public static void SpawnAll(GameBusinessContext ctx, BlockTM[] blockTMArr, Vector2[] posArr, Vector2[] sizeArr, int[] indexArr) {
            for (int i = 0; i < blockTMArr.Length; i++) {
                var tm = blockTMArr[i];
                if (tm == null) continue;
                var pos = posArr[i];
                var size = sizeArr[i];
                var index = indexArr[i];
                Spawn(ctx, tm.typeID, pos, size, index);
            }
        }

        public static void UnSpawn(GameBusinessContext ctx, BlockEntity block) {
            ctx.blockRepo.Remove(block);
            block.TearDown();
        }

    }

}
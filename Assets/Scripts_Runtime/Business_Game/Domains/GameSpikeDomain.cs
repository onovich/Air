using UnityEngine;

namespace Air {

    public static class GameSpikeDomain {

        public static SpikeEntity Spawn(GameBusinessContext ctx, int typeID, Vector2 pos, Vector2 size) {
            var spike = GameFactory.Spike_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              ctx.idRecordService,
                                              typeID,
                                              pos,
                                              size);
            ctx.spikeRepo.Add(spike);
            return spike;
        }

        public static void SpawnAll(GameBusinessContext ctx, SpikeTM[] blockTMArr, Vector2[] posArr, Vector2[] sizeArr) {
            for (int i = 0; i < blockTMArr.Length; i++) {
                var tm = blockTMArr[i];
                var pos = posArr[i];
                var size = sizeArr[i];
                Spawn(ctx, tm.typeID, pos, size);
            }
        }

        public static void UnSpawn(GameBusinessContext ctx, SpikeEntity spike) {
            ctx.spikeRepo.Remove(spike);
            spike.TearDown();
        }

    }

}
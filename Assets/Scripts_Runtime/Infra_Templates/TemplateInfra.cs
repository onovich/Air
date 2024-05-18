using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Air {

    public static class TemplateInfra {

        public static async Task LoadAssets(TemplateInfraContext ctx) {

            {
                var handle = Addressables.LoadAssetAsync<GameConfig>("TM_Config");
                var cotmfig = await handle.Task;
                ctx.Config_Set(cotmfig);
                ctx.configHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<MapTM>("TM_Map", null);
                var mapList = await handle.Task;
                foreach (var tm in mapList) {
                    ctx.Map_Add(tm);
                }
                ctx.mapHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<BoidTM>("TM_Boid", null);
                var boidList = await handle.Task;
                foreach (var tm in boidList) {
                    ctx.Boid_Add(tm);
                }
                ctx.boidHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<BlockTM>("TM_Block", null);
                var blockList = await handle.Task;
                foreach (var tm in blockList) {
                    ctx.Block_Add(tm);
                }
                ctx.blockHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<SpikeTM>("TM_Spike", null);
                var spikeList = await handle.Task;
                foreach (var tm in spikeList) {
                    ctx.Spike_Add(tm);
                }
                ctx.spikeHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<LeaderTM>("TM_Leader", null);
                var leaderList = await handle.Task;
                foreach (var tm in leaderList) {
                    ctx.Leader_Add(tm);
                }
                ctx.leaderHandle = handle;
            }

        }

        public static void Release(TemplateInfraContext ctx) {
            if (ctx.configHandle.IsValid()) {
                Addressables.Release(ctx.configHandle);
            }
            if (ctx.mapHandle.IsValid()) {
                Addressables.Release(ctx.mapHandle);
            }
            if (ctx.boidHandle.IsValid()) {
                Addressables.Release(ctx.boidHandle);
            }
            if (ctx.blockHandle.IsValid()) {
                Addressables.Release(ctx.blockHandle);
            }
            if (ctx.spikeHandle.IsValid()) {
                Addressables.Release(ctx.spikeHandle);
            }
            if (ctx.leaderHandle.IsValid()) {
                Addressables.Release(ctx.leaderHandle);
            }
        }

    }

}
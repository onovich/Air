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
                var handle = Addressables.LoadAssetsAsync<BoidTM>("TM_Boid", null);
                var boidList = await handle.Task;
                foreach (var tm in boidList) {
                    ctx.Boid_Add(tm);
                }
                ctx.boidHandle = handle;
            }

        }

        public static void Release(TemplateInfraContext ctx) {
            if (ctx.configHandle.IsValid()) {
                Addressables.Release(ctx.configHandle);
            }
            if (ctx.boidHandle.IsValid()) {
                Addressables.Release(ctx.boidHandle);
            }
        }

    }

}
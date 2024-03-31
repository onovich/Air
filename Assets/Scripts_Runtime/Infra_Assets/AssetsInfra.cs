using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AddressableAssets;

namespace Air {

    public static class AssetsInfra {

        public static async Task LoadAssets(AssetsInfraContext ctx) {
            {
                var handle = Addressables.LoadAssetsAsync<GameObject>("Entity", null);
                var list = await handle.Task;
                foreach (var asset in list) {
                    ctx.Entity_Add(asset.name, asset);
                }
                ctx.entityHandle = handle;
            }
            {
                var handle = Addressables.LoadAssetsAsync<GameObject>("Mod", null);
                var list = await handle.Task;
                foreach (var asset in list) {
                    ctx.Mod_Add(asset.name, asset);
                }
                ctx.entityHandle = handle;
            }

            // {
            // var handle = Addressables.LoadAssetsAsync<TileBase>("Tile", null);
            // var list = await handle.Task;
            // if (list != null) {
            //     foreach (var asset in list) {
            //         ctx.Tile_Add(asset.name, asset);
            //     }
            // }
            // ctx.tileHandle = handle;
            // }
        }

        public static void ReleaseAssets(AssetsInfraContext ctx) {
            if (ctx.entityHandle.IsValid()) {
                Addressables.Release(ctx.entityHandle);
            }
            if (ctx.tileHandle.IsValid()) {
                Addressables.Release(ctx.tileHandle);
            }
        }

    }

}
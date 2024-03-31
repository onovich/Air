using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;

namespace Air {

    public class AssetsInfraContext {

        Dictionary<string, GameObject> entityDict;
        Dictionary<string, GameObject> modDict;
        Dictionary<string, TileBase> tileDict;

        public AsyncOperationHandle entityHandle;
        public AsyncOperationHandle tileHandle;

        public AssetsInfraContext() {
            entityDict = new Dictionary<string, GameObject>();
            modDict = new Dictionary<string, GameObject>();
            tileDict = new Dictionary<string, TileBase>();
        }

        // Entity
        public void Entity_Add(string name, GameObject prefab) {
            entityDict.Add(name, prefab);
        }

        bool Entity_TryGet(string name, out GameObject asset) {
            var has = entityDict.TryGetValue(name, out asset);
            return has;
        }

        public GameObject Entity_GetBoid() {
            var has = Entity_TryGet("Entity_Boid", out var prefab);
            if (!has) {
                ALog.LogError($"Entity Boid not found");
            }
            return prefab;
        }

        public GameObject Entity_GetBullet() {
            var has = Entity_TryGet("Entity_Bullet", out var prefab);
            if (!has) {
                ALog.LogError($"Entity Bullet not found");
            }
            return prefab;
        }

        public GameObject Entity_GetBlock() {
            var has = Entity_TryGet("Entity_Block", out var prefab);
            if (!has) {
                ALog.LogError($"Entity Block not found");
            }
            return prefab;
        }

        public GameObject Entity_GetLoot() {
            var has = Entity_TryGet("Entity_Loot", out var prefab);
            if (!has) {
                ALog.LogError($"Entity Loot not found");
            }
            return prefab;
        }

        public GameObject Entity_GetPlant() {
            var has = Entity_TryGet("Entity_Plant", out var prefab);
            if (!has) {
                ALog.LogError($"Entity Plant not found");
            }
            return prefab;
        }

        public GameObject Entity_GetMap() {
            var has = Entity_TryGet("Entity_Map", out var prefab);
            if (!has) {
                ALog.LogError($"Entity Map not found");
            }
            return prefab;
        }

        // Mod
        public void Mod_Add(string name, GameObject prefab) {
            modDict.Add(name, prefab);
        }

        public bool Mod_TryGet(string name, out GameObject asset) {
            var has = modDict.TryGetValue(name, out asset);
            return has;
        }

        // Tile
        public void Tile_Add(string name, TileBase prefab) {
            tileDict.Add(name, prefab);
        }

        public bool Tile_Get(string name, out TileBase asset) {
            var has = tileDict.TryGetValue(name, out asset);
            if (!has) {
                ALog.LogError($"Entity {name} not found");
            }
            return has;
        }

    }

}
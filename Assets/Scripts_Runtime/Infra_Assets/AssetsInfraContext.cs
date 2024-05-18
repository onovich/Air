using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;

namespace Air {

    public class AssetsInfraContext {

        Dictionary<string, GameObject> entityDict;
        public AsyncOperationHandle entityHandle;

        public AssetsInfraContext() {
            entityDict = new Dictionary<string, GameObject>();
        }

        // Entity
        public void Entity_Add(string name, GameObject prefab) {
            entityDict.Add(name, prefab);
        }

        bool Entity_TryGet(string name, out GameObject asset) {
            var has = entityDict.TryGetValue(name, out asset);
            return has;
        }

        public GameObject Entity_GetMap() {
            var has = Entity_TryGet("Entity_Map", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Map not found");
            }
            return prefab;
        }

        public GameObject Entity_GetBoid() {
            var has = Entity_TryGet("Entity_Boid", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Boid not found");
            }
            return prefab;
        }

        public GameObject Entity_GetBlock() {
            var has = Entity_TryGet("Entity_Block", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Block not found");
            }
            return prefab;
        }

        public GameObject Entity_GetSpike() {
            var has = Entity_TryGet("Entity_Spike", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Spike not found");
            }
            return prefab;
        }

        public GameObject Entity_GetLeader() {
            var has = Entity_TryGet("Entity_Leader", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Leader not found");
            }
            return prefab;
        }

    }

}
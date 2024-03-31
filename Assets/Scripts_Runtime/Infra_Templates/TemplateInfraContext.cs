using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Air {

    public class TemplateInfraContext {

        GameConfig config;
        public AsyncOperationHandle configHandle;

        Dictionary<int, BoidTM> boidDict;
        public AsyncOperationHandle boidHandle;

        public TemplateInfraContext() {
            boidDict = new Dictionary<int, BoidTM>();
        }

        // Game
        public void Config_Set(GameConfig config) {
            this.config = config;
        }

        public GameConfig Config_Get() {
            return config;
        }

        // Boid
        public void Boid_Add(BoidTM boid) {
            boidDict.Add(boid.typeID, boid);
        }

        public bool Boid_TryGet(int typeID, out BoidTM boid) {
            var has = boidDict.TryGetValue(typeID, out boid);
            if (!has) {
                ALog.LogError($"Map {typeID} not found");
            }
            return has;
        }

        // Clear
        public void Clear() {
            boidDict.Clear();
        }

    }

}
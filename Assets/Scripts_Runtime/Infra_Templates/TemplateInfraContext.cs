using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Air {

    public class TemplateInfraContext {

        GameConfig config;
        public AsyncOperationHandle configHandle;

        Dictionary<int, MapTM> mapDict;
        public AsyncOperationHandle mapHandle;

        Dictionary<int, BoidTM> boidDict;
        public AsyncOperationHandle boidHandle;

        Dictionary<int, BlockTM> blockDict;
        public AsyncOperationHandle blockHandle;

        Dictionary<int, SpikeTM> spikeDict;
        public AsyncOperationHandle spikeHandle;

        public TemplateInfraContext() {
            mapDict = new Dictionary<int, MapTM>();
            boidDict = new Dictionary<int, BoidTM>();
            blockDict = new Dictionary<int, BlockTM>();
            spikeDict = new Dictionary<int, SpikeTM>();
        }

        // Game
        public void Config_Set(GameConfig config) {
            this.config = config;
        }

        public GameConfig Config_Get() {
            return config;
        }

        // Map
        public void Map_Add(MapTM map) {
            mapDict.Add(map.typeID, map);
        }

        public bool Map_TryGet(int typeID, out MapTM map) {
            var has = mapDict.TryGetValue(typeID, out map);
            if (!has) {
                GLog.LogError($"Map {typeID} not found");
            }
            return has;
        }

        // Boid
        public void Boid_Add(BoidTM Boid) {
            boidDict.Add(Boid.typeID, Boid);
        }

        public bool Boid_TryGet(int typeID, out BoidTM Boid) {
            var has = boidDict.TryGetValue(typeID, out Boid);
            if (!has) {
                GLog.LogError($"Boid {typeID} not found");
            }
            return has;
        }

        // Block
        public void Block_Add(BlockTM block) {
            blockDict.Add(block.typeID, block);
        }

        public bool Block_TryGet(int typeID, out BlockTM block) {
            var has = blockDict.TryGetValue(typeID, out block);
            if (!has) {
                GLog.LogError($"Block {typeID} not found");
            }
            return has;
        }

        // Spike
        public void Spike_Add(SpikeTM spike) {
            spikeDict.Add(spike.typeID, spike);
        }

        public bool Spike_TryGet(int typeID, out SpikeTM spike) {
            var has = spikeDict.TryGetValue(typeID, out spike);
            if (!has) {
                GLog.LogError($"Spike {typeID} not found");
            }
            return has;
        }

        // Clear
        public void Clear() {
            mapDict.Clear();
            boidDict.Clear();
            blockDict.Clear();
            spikeDict.Clear();
        }

    }

}
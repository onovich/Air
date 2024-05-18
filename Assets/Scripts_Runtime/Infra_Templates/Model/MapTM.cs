using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Air {

    [CreateAssetMenu(fileName = "SO_Map", menuName = "Air/MapTM")]
    public class MapTM : ScriptableObject {

        public int typeID;

        public Vector2 mapSize;
        public Vector2 mapPos;

        // Boid Spawn 
        public Vector2 SpawnPoint;

        // Boid Spawn
        public BoidTM[] boidSpawnArr;
        public Vector2[] boidSpawnPosArr;
        public AllyStatus[] boidSpawnAllyStatusArr;

        // Block Spawn
        public BlockTM[] blockSpawnArr;
        public Vector2[] blockSpawnPosArr;
        public Vector2[] blockSpawnSizeArr;

        // Spike Spawn
        public SpikeTM[] spikeSpawnArr;
        public Vector2[] spikeSpawnPosArr;
        public Vector2[] spikeSpawnSizeArr;

        // Leader Spawn
        public LeaderTM[] leaderSpawnArr;
        public Vector2[] leaderSpawnPosArr;
        public AllyStatus[] leaderSpawnAllyStatusArr;

        // Camera
        public Vector2 cameraConfinerWorldMax;
        public Vector2 cameraConfinerWorldMin;

    }

}
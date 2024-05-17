using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Air {

    [CreateAssetMenu(fileName = "SO_Map", menuName = "Air/MapTM")]
    public class MapTM : ScriptableObject {

        public int typeID;

        public Vector2 mapSize;
        public Vector2 mapPos;
        public TileBase tileBase_terrain;

        // Boid Spawn 
        public Vector2 SpawnPoint;

        // Boid Spawn
        public BoidTM[] boidSpawnArr;
        public Vector2[] boidSpawnPosArr;
        public int[] boidSpawnIndexArr;

        // Block Spawn
        public BlockTM[] blockSpawnArr;
        public Vector2[] blockSpawnPosArr;
        public Vector2[] blockSpawnSizeArr;
        public int[] blockSpawnIndexArr;

        // Spike Spawn
        public SpikeTM[] spikeSpawnArr;
        public Vector2[] spikeSpawnPosArr;
        public Vector2[] spikeSpawnSizeArr;
        public int[] spikeSpawnIndexArr;

        // Camera
        public Vector2 cameraConfinerWorldMax;
        public Vector2 cameraConfinerWorldMin;

    }

}
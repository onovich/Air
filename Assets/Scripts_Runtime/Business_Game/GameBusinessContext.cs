using System;
using System.Collections.Generic;
using UnityEngine;

namespace Air {

    public class GameBusinessContext {

        // Entity
        public GameEntity gameEntity;
        public PlayerEntity playerEntity;
        public InputEntity inputEntity; // External
        public MapEntity currentMapEntity;

        public BoidRepository boidRepo;
        public BlockRepository blockRepo;
        public SpikeRepository spikeRepo;
        public LeaderRepository leaderRepo;

        // App
        public UIAppContext uiContext;
        public VFXAppContext vfxContext;
        public CameraAppContext cameraContext;

        // Camera
        public Camera mainCamera;

        // Service
        public IDRecordService idRecordService;
        public RandomService randomService;

        // Infra
        public TemplateInfraContext templateInfraContext;
        public AssetsInfraContext assetsInfraContext;

        // Timer
        public float fixedRestSec;

        // SpawnPoint
        public Vector2 ownerSpawnPoint;

        // TEMP
        public RaycastHit2D[] hitResults;
        public BoidData[] boidDataTemp;
        public ComputeBuffer boidBuffer;

        public GameBusinessContext() {
            gameEntity = new GameEntity();
            playerEntity = new PlayerEntity();
            idRecordService = new IDRecordService();
            randomService = new RandomService();
            boidRepo = new BoidRepository();
            blockRepo = new BlockRepository();
            spikeRepo = new SpikeRepository();
            leaderRepo = new LeaderRepository();
            hitResults = new RaycastHit2D[100];
            boidDataTemp = new BoidData[1000];
        }

        public void Reset() {
            idRecordService.Reset();
            boidRepo.Clear();
            blockRepo.Clear();
            spikeRepo.Clear();
            leaderRepo.Clear();
            boidBuffer?.Release();
        }

        // Leader
        public LeaderEntity Leader_GetOwner() {
            leaderRepo.TryGetLeader(playerEntity.ownerLeaderEntityID, out var leader);
            return leader;
        }

    }

}
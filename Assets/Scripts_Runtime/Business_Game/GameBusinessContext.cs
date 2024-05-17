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

        // App
        public UIAppContext uiContext;
        public VFXAppContext vfxContext;
        public CameraAppContext cameraContext;

        // Camera
        public Camera mainCamera;

        // Service
        public IDRecordService idRecordService;

        // Infra
        public TemplateInfraContext templateInfraContext;
        public AssetsInfraContext assetsInfraContext;

        // Timer
        public float fixedRestSec;

        // SpawnPoint
        public Vector2 ownerSpawnPoint;

        // TEMP
        public RaycastHit2D[] hitResults;

        public GameBusinessContext() {
            gameEntity = new GameEntity();
            playerEntity = new PlayerEntity();
            idRecordService = new IDRecordService();
            boidRepo = new BoidRepository();
            blockRepo = new BlockRepository();
            spikeRepo = new SpikeRepository();
            hitResults = new RaycastHit2D[100];
        }

        public void Reset() {
            idRecordService.Reset();
            boidRepo.Clear();
            blockRepo.Clear();
            spikeRepo.Clear();
        }

        // Boid
        public BoidEntity Boid_GetOwner() {
            boidRepo.TryGetBoid(playerEntity.ownerBoidEntityID, out var boid);
            return boid;
        }

        public void Boid_ForEach(Action<BoidEntity> onAction) {
            boidRepo.ForEach(onAction);
        }

        // Block
        public void Block_ForEach(Action<BlockEntity> onAction) {
            blockRepo.ForEach(onAction);
        }

    }

}
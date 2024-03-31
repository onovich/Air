using System;
using System.Collections.Generic;
using UnityEngine;

namespace Air {

    public class GameBusinessContext {

        // Entity
        public GameEntity gameEntity;
        public PlayerEntity playerEntity;
        public InputEntity inputEntity; // External

        public BoidRepository boidRepo;

        // UI
        public UIAppContext uiContext;

        // Camera
        public Camera mainCamera;

        // Service
        public IDRecordService idRecordService;

        // Infra
        public TemplateInfraContext templateInfraContext;
        public AssetsInfraContext assetsInfraContext;

        public GameBusinessContext() {
            gameEntity = new GameEntity();
            playerEntity = new PlayerEntity();
            idRecordService = new IDRecordService();
            boidRepo = new BoidRepository();
        }

        public void Reset() {
            boidRepo.Clear();
        }

        // Boid
        public BoidEntity Boid_GetOwner() {
            boidRepo.TryGetBoid(playerEntity.ownerBoidEntityID, out var boid);
            return boid;
        }

        public void Boid_UpdatePosDict(BoidEntity boid) {
            boidRepo.UpdatePosDict(boid);
        }

        public void Boid_ForEach(Action<BoidEntity> onAction) {
            boidRepo.ForEach(onAction);
        }

    }

}
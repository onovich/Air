using UnityEngine;

namespace Air {

    public static class GameFactory {

        public static MapEntity Map_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 int typeID) {

            var has = templateInfraContext.Map_TryGet(typeID, out var mapTM);
            if (!has) {
                GLog.LogError($"Map {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetMap();
            var map = GameObject.Instantiate(prefab).GetComponent<MapEntity>();
            map.Ctor();
            map.typeID = typeID;
            map.mapSize = mapTM.mapSize;
            map.mapOffset = mapTM.mapPos;
            map.tileBase_terrain = mapTM.tileBase_terrain;

            return map;
        }

        public static BoidEntity Boid_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 IDRecordService idRecordService,
                                 int typeID,
                                 Vector2 pos) {

            var has = templateInfraContext.Boid_TryGet(typeID, out var boidTM);
            if (!has) {
                GLog.LogError($"Boid {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetBoid();
            var boid = GameObject.Instantiate(prefab).GetComponent<BoidEntity>();
            boid.Ctor();

            // Base Info
            boid.entityID = idRecordService.PickBoidEntityID();
            boid.typeID = typeID;
            boid.allyStatus = boidTM.allyStatus;

            // Set Attr
            boid.moveSpeed = boidTM.moveSpeed;
            boid.rotationSpeed = boidTM.rotationSpeed;
            boid.hp = boidTM.hpMax;
            boid.hpMax = boidTM.hpMax;

            // Set Pos
            boid.Pos_SetPos(pos);

            // Set Mesh
            boid.Mesh_Set(boidTM.mesh);

            // Set FSM
            boid.FSM_EnterIdle();

            // Set VFX
            boid.deadVFXName = boidTM.deadVFX.name;
            boid.deadVFXDuration = boidTM.deadVFXDuration;

            return boid;
        }

        public static BlockEntity Block_Spawn(TemplateInfraContext templateInfraContext,
                                  AssetsInfraContext assetsInfraContext,
                                  IDRecordService idRecordService,
                                  int typeID,
                                  Vector2 pos,
                                  Vector2 size,
                                  int index) {

            var has = templateInfraContext.Block_TryGet(typeID, out var blockTM);
            if (!has) {
                GLog.LogError($"Boid {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetBlock();
            var block = GameObject.Instantiate(prefab).GetComponent<BlockEntity>();
            block.Ctor();

            // Base Info
            block.entityIndex = index;
            block.typeID = typeID;

            // Set Pos
            block.Pos_SetPos(pos);

            // Set Size
            block.Size_SetSize(size);

            // Set Mesh
            block.Mesh_Set(blockTM.mesh);

            // Rename
            block.Rename();

            return block;
        }

        public static SpikeEntity Spike_Spawn(TemplateInfraContext templateInfraContext,
                                  AssetsInfraContext assetsInfraContext,
                                  IDRecordService idRecordService,
                                  int typeID,
                                  Vector2 pos,
                                  Vector2 size,
                                  int index) {

            var has = templateInfraContext.Spike_TryGet(typeID, out var blockTM);
            if (!has) {
                GLog.LogError($"Boid {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetSpike();
            var spike = GameObject.Instantiate(prefab).GetComponent<SpikeEntity>();
            spike.Ctor();

            // Base Info
            spike.entityIndex = index;
            spike.typeID = typeID;

            // Set Pos
            spike.Pos_SetPos(pos);

            // Set Size
            spike.Size_SetSize(size);

            // Set Mesh
            spike.Mesh_Set(blockTM.mesh);

            // Rename
            spike.Rename();

            return spike;
        }

    }

}
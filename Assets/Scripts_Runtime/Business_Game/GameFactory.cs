using UnityEngine;

namespace Air {

    public static class GameFactory {

        public static LeaderEntity Leader_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 IDRecordService idRecordService,
                                 int typeID,
                                 Vector2 pos,
                                 AllyStatus allyStatus) {

            var has = templateInfraContext.Leader_TryGet(typeID, out var leaderTM);
            if (!has) {
                GLog.LogError($"Leader {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetLeader();
            var leader = GameObject.Instantiate(prefab).GetComponent<LeaderEntity>();
            leader.Ctor();

            // Base Info
            leader.entityID = idRecordService.PickLeaderEntityID();
            leader.typeID = typeID;
            leader.allyStatus = allyStatus;

            // Set Attr
            leader.moveSpeed = leaderTM.moveSpeed;
            leader.rotationSpeed = leaderTM.rotationSpeed;

            // Set Pos
            leader.Pos_SetPos(pos);

            // Set Mesh
            leader.Mesh_Set(leaderTM.mesh);

            // Set FSM
            leader.FSM_EnterIdle();

            // Set VFX
            leader.deadVFXName = leaderTM.deadVFX.name;
            leader.deadVFXDuration = leaderTM.deadVFXDuration;

            return leader;
        }

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

            return map;
        }

        public static BoidEntity Boid_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 IDRecordService idRecordService,
                                 int typeID,
                                 Vector2 pos,
                                 AllyStatus allyStatus) {

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
            boid.allyStatus = allyStatus;

            // Set Attr
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
                                  Vector2 size) {

            var has = templateInfraContext.Block_TryGet(typeID, out var blockTM);
            if (!has) {
                GLog.LogError($"Boid {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetBlock();
            var block = GameObject.Instantiate(prefab).GetComponent<BlockEntity>();
            block.Ctor();

            // Base Info
            block.entityID = idRecordService.PickBlockEntityID();
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
                                  Vector2 size) {

            var has = templateInfraContext.Spike_TryGet(typeID, out var blockTM);
            if (!has) {
                GLog.LogError($"Boid {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetSpike();
            var spike = GameObject.Instantiate(prefab).GetComponent<SpikeEntity>();
            spike.Ctor();

            // Base Info
            spike.entityID = idRecordService.PickSpikeEntityID();
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
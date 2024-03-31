using UnityEngine;

namespace Air {

    public static class GameFactory {

        public static BoidEntity Boid_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 IDRecordService idRecordService,
                                 int typeID,
                                 Vector2 pos) {

            var has = templateInfraContext.Boid_TryGet(typeID, out var boidTM);
            if (!has) {
                ALog.LogError($"Boid {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetBoid();
            var boid = GameObject.Instantiate(prefab).GetComponent<BoidEntity>();
            boid.Ctor();

            // Base Info
            boid.entityID = idRecordService.PickBoidEntityID();
            boid.typeID = typeID;
            boid.allyStatus = boidTM.allyStatus;
            boid.aiType = boidTM.aiType;

            // Set Attr
            boid.moveSpeed = boidTM.moveSpeed;

            // Set Pos
            boid.Pos_SetPos(pos);

            // Set Mesh
            boid.Mesh_Set(boidTM.mesh);

            // Set FSM
            boid.FSM_EnterIdle();

            return boid;
        }

    }

}
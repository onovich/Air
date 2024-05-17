using UnityEngine;

namespace Air {

    public static class GameBoidFSMController {

        public static void FixedTickFSM(GameBusinessContext ctx, BoidEntity boid, float fixdt) {

            FixedTickFSM_Any(ctx, boid, fixdt);

            BoidFSMStatus status = boid.FSM_GetStatus();
            if (status == BoidFSMStatus.Normal) {
                FixedTickFSM_Idle(ctx, boid, fixdt);
            } else if (status == BoidFSMStatus.Dead) {
                FixedTickFSM_Dead(ctx, boid, fixdt);
            } else {
                GLog.LogError($"GameBoidFSMController.FixedTickFSM: unknown status: {status}");
            }

        }

        static void FixedTickFSM_Any(GameBusinessContext ctx, BoidEntity boid, float fixdt) {

        }

        static void FixedTickFSM_Idle(GameBusinessContext ctx, BoidEntity boid, float fixdt) {
            BoidFSMComponent fsm = boid.FSM_GetComponent();
            if (fsm.normal_isEntering) {
                fsm.normal_isEntering = false;
            }

            // Move
            GameBoidDomain.ApplyMove(ctx, boid, fixdt);

            // Constraint
            GameBoidDomain.ApplyConstraint(ctx, boid, fixdt);

            // Dead
            if (boid.hp <= 0) {
                fsm.EnterDead();
            }
        }

        static void FixedTickFSM_Dead(GameBusinessContext ctx, BoidEntity boid, float fixdt) {
            BoidFSMComponent fsm = boid.FSM_GetComponent();
            if (fsm.dead_isEntering) {
                fsm.dead_isEntering = false;
            }

            // VFX
            VFXApp.AddVFXToWorld(ctx.vfxContext, boid.deadVFXName, boid.deadVFXDuration, boid.Pos);

            // Camera
            CameraApp.ShakeOnce(ctx.cameraContext, ctx.cameraContext.mainCameraID);
            boid.needTearDown = true;
        }

    }

}
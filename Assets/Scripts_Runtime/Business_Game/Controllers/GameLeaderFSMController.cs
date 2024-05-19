using UnityEngine;

namespace Air {

    public static class GameLeaderFSMController {

        public static void TickFSM(GameBusinessContext ctx, LeaderEntity leader, float dt) {

            FixedTickFSM_Any(ctx, leader, dt);
            LeaderFSMStatus status = leader.FSM_GetStatus();

            if (status == LeaderFSMStatus.Normal) {
                FixedTickFSM_Idle(ctx, leader, dt);
            } else if (status == LeaderFSMStatus.Dead) {
                FixedTickFSM_Dead(ctx, leader, dt);
            } else {
                GLog.LogError($"GameLeaderFSMController.FixedTickFSM: unknown status: {status}");
            }

        }

        static void FixedTickFSM_Any(GameBusinessContext ctx, LeaderEntity leader, float dt) {

        }

        static void FixedTickFSM_Idle(GameBusinessContext ctx, LeaderEntity leader, float dt) {
            LeaderFSMComponent fsm = leader.FSM_GetComponent();
            if (fsm.normal_isEntering) {
                fsm.normal_isEntering = false;
            }

            // Move
            GameLeaderDomain.ApplyMove(ctx, leader, dt);

            // Constraint
            GameLeaderDomain.ApplyConstraint(ctx, leader, dt);

            // Dead
        }

        static void FixedTickFSM_Dead(GameBusinessContext ctx, LeaderEntity leader, float dt) {
            LeaderFSMComponent fsm = leader.FSM_GetComponent();
            if (fsm.dead_isEntering) {
                fsm.dead_isEntering = false;
            }

            // VFX
            VFXApp.AddVFXToWorld(ctx.vfxContext, leader.deadVFXName, leader.deadVFXDuration, leader.Pos);

            // Camera
            CameraApp.ShakeOnce(ctx.cameraContext, ctx.cameraContext.mainCameraID);
            leader.needTearDown = true;
        }

    }

}
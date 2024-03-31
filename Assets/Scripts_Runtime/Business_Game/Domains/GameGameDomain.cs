using UnityEngine;

namespace Air {

    public static class GameGameDomain {

        public static void NewGame(GameBusinessContext ctx) {

            var config = ctx.templateInfraContext.Config_Get();

            // Game
            var game = ctx.gameEntity;
            game.fsmComponent.Gaming_Enter();

            // Boid
            var player = ctx.playerEntity;

            // - Owner
            var owner = GameBoidDomain.Spawn(ctx,
                                             config.ownerBoidTypeID,
                                             new Vector2(0, 0));
            player.ownerBoidEntityID = owner.entityID;

            // - NPC
            // foreach (var npc in mapTM.boidSpawnArr) {
            //     GameBoidDomain.Spawn(ctx,
            //                          npc.typeID,
            //                          npc.pos);
            // }

            // Camera

            // UI

            // Cursor

        }

        public static void ExitGame(GameBusinessContext ctx) {
            // Game
            var game = ctx.gameEntity;
            game.fsmComponent.NotInGame_Enter();

            // Boid
            int boidLen = ctx.boidRepo.TakeAll(out var boids);
            for (int i = 0; i < boidLen; i++) {
                var boid = boids[i];
                GameBoidDomain.UnSpawn(ctx, boid);
            }

            // UI
        }

    }
}
using MortiseFrame.Swing;
using TenonKit.Prism;
using UnityEngine;

namespace Air {

    public static class GameGameDomain {

        public static void NewGame(GameBusinessContext ctx) {

            var config = ctx.templateInfraContext.Config_Get();

            // Game
            var game = ctx.gameEntity;
            game.fsmComponent.Gaming_Enter();

            // Map
            var mapTypeID = config.originalMapTypeID;
            var map = GameMapDomain.Spawn(ctx, mapTypeID);
            var has = ctx.templateInfraContext.Map_TryGet(mapTypeID, out var mapTM);
            if (!has) {
                GLog.LogError($"MapTM Not Found {mapTypeID}");
            }

            // Leader
            var player = ctx.playerEntity;

            // - Player
            var spawnPoint = mapTM.SpawnPoint;
            var owner = GameLeaderDomain.Spawn(ctx,
                                             config.playerLeaderTypeID,
                                             spawnPoint,
                                             AllyStatus.Friend);
            player.ownerLeaderEntityID = owner.entityID;
            ctx.ownerSpawnPoint = spawnPoint;

            // Boid
            var boidTMArr = mapTM.boidSpawnArr;
            var boidPosArr = mapTM.boidSpawnPosArr;
            var boidAllyStatusArr = mapTM.boidSpawnAllyStatusArr;
            GameBoidDomain.SpawnAll(ctx, boidTMArr, boidPosArr, boidAllyStatusArr);

            //// CS
            var playerBoidTypeID = 1;
            GameBoidDomain.InitCS(ctx, playerBoidTypeID);

            // Block
            var blockTMArr = mapTM.blockSpawnArr;
            var blockPosArr = mapTM.blockSpawnPosArr;
            var blockSizeArr = mapTM.blockSpawnSizeArr;
            GameBlockDomain.SpawnAll(ctx, blockTMArr, blockPosArr, blockSizeArr);

            // Spike
            var spikeTMArr = mapTM.spikeSpawnArr;
            var spikePosArr = mapTM.spikeSpawnPosArr;
            var spikeSizeArr = mapTM.spikeSpawnSizeArr;
            GameSpikeDomain.SpawnAll(ctx, spikeTMArr, spikePosArr, spikeSizeArr);

            // Camera
            CameraApp.Init(ctx.cameraContext, owner.transform, owner.Pos, mapTM.cameraConfinerWorldMax, mapTM.cameraConfinerWorldMin);

            // UI

            // Cursor

        }

        public static void ApplyRestartGame(GameBusinessContext ctx) {

            var spawnPoint = ctx.ownerSpawnPoint;
            var game = ctx.gameEntity;
            var enterTime = game.fsmComponent.gameOver_enterTime;
            var gameOver_isEntering = game.fsmComponent.gameOver_isEntering;

            if (gameOver_isEntering) {
                game.fsmComponent.gameOver_isEntering = false;
                CameraApp.SetMoveToTarget(ctx.cameraContext, spawnPoint, enterTime, EasingType.Linear, EasingMode.None, onComplete: () => {
                    ExitGame(ctx);
                    NewGame(ctx);
                    game.fsmComponent.Gaming_Enter();
                });
            }

        }

        public static void ApplyGameResult(GameBusinessContext ctx) {
            var owner = ctx.Leader_GetOwner();
            var game = ctx.gameEntity;
            var config = ctx.templateInfraContext.Config_Get();
            // game.fsmComponent.GameOver_Enter(config.gameResetEnterTime);
        }

        public static void ExitGame(GameBusinessContext ctx) {
            // Game
            var game = ctx.gameEntity;
            game.fsmComponent.NotInGame_Enter();

            // Map
            GameMapDomain.UnSpawn(ctx);

            // Leader
            int leaderLen = ctx.leaderRepo.TakeAll(out var leaderArr);
            for (int i = 0; i < leaderLen; i++) {
                var leader = leaderArr[i];
                GameLeaderDomain.UnSpawn(ctx, leader);
            }

            // Boid
            int boidLen = ctx.boidRepo.TakeAll(out var boidArr);
            for (int i = 0; i < boidLen; i++) {
                var boid = boidArr[i];
                GameBoidDomain.UnSpawn(ctx, boid);
            }

            // Block
            int blockLen = ctx.blockRepo.TakeAll(out var blockArr);
            for (int i = 0; i < blockLen; i++) {
                var block = blockArr[i];
                GameBlockDomain.UnSpawn(ctx, block);
            }

            //// CS
            GameBoidDomain.ReleaseCS(ctx);

            // Spike
            int spikeLen = ctx.spikeRepo.TakeAll(out var spikeArr);
            for (int i = 0; i < spikeLen; i++) {
                var spike = spikeArr[i];
                GameSpikeDomain.UnSpawn(ctx, spike);
            }

            // UI
        }

    }
}
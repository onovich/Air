using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Air {

    [CreateAssetMenu(fileName = "SO_GameConfig", menuName = "Air/GameConfig")]
    public class GameConfig : ScriptableObject {

        // Game
        [Header("Game Config")]
        public float gameResetEnterTime;

        // Leader
        [Header("Leader Config")]
        public int playerLeaderTypeID;

        // Map
        [Header("Map Config")]
        public int originalMapTypeID;

        // Camera
        [Header("DeadZone Config")]
        public Vector2 cameraDeadZoneNormalizedSize;

        [Header("Shake Config")]
        public float boidDeadShakeFrequency;
        public float boidDeadShakeAmplitude;
        public float boidDeadShakeDuration;
        public EasingType boidDeadShakeEasingType;
        public EasingMode boidDeadShakeEasingMode;

        [Header("CS Config")]
        public ComputeShader boidCS;

    }

}
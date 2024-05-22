using System;
using UnityEngine;

namespace Air {

    [CreateAssetMenu(fileName = "SO_Boid", menuName = "Air/BoidTM")]
    public class BoidTM : ScriptableObject {

        [Header("Base Info")]
        public int typeID;

        [Header("Attr")]
        public int hpMax;
        public float maxSteerForce;
        public float maxSpeed;
        public float minSpeed;

        [Header("Boids AI Weight")]
        [Header("分离")]
        public float separationWeight;
        [Header("对齐")]
        public float alignmentWeight;
        [Header("聚集")]
        public float cohesionWeight;
        [Header("跟随Leader")]
        public float followWeight;
        [Header("躲避Leader")]
        public float avoidWeight;

        [Header("Boids AI Radius")]
        [Header("分离")]
        public float separationRadius;
        [Header("对齐")]
        public float alignmentRadius;
        [Header("聚集")]
        public float cohesionRadius;
        [Header("跟随Leaer")]
        public float followRadius;
        [Header("躲避Leader")]
        public float avoidRadius;

        [Header("Render")]
        public Sprite mesh;
        public Color color;

        [Header("VFX")]
        public GameObject deadVFX;
        public float deadVFXDuration;

    }

}
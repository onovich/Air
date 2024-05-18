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
        public float separationWeight;
        public float separationRadius;
        public float alignmentWeight;
        public float alignmentRadius;
        public float cohesionWeight;
        public float cohesionRadius;

        [Header("Render")]
        public Sprite mesh;
        public Color color;

        [Header("VFX")]
        public GameObject deadVFX;
        public float deadVFXDuration;

    }

}
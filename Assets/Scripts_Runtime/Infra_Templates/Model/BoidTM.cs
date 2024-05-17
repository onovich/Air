using System;
using UnityEngine;

namespace Air {

    [CreateAssetMenu(fileName = "SO_Boid", menuName = "Air/BoidTM")]
    public class BoidTM : ScriptableObject {

        [Header("Base Info")]
        public int typeID;
        public AllyStatus allyStatus;

        [Header("Attr")]
        public float moveSpeed;
        public float rotationSpeed;
        public int hpMax;

        [Header("Move")]
        public float separationWeight;
        public float alignmentWeight;
        public float cohesionWeight;

        [Header("Render")]
        public Sprite mesh;
        public Color color;

        [Header("VFX")]
        public GameObject deadVFX;
        public float deadVFXDuration;

    }

}
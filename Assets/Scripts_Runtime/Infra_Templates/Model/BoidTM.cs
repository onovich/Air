using System;
using UnityEngine;

namespace Air {

    [CreateAssetMenu(fileName = "SO_Boid", menuName = "Air/BoidTM")]
    public class BoidTM : ScriptableObject {

        public int typeID;
        public AllyStatus allyStatus;
        public AIType aiType;

        public float moveSpeed;

        public Sprite mesh;
    }

}
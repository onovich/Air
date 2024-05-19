using UnityEngine;

namespace Air {
    public struct BoidCSModel {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 alignment;
        public Vector3 cohesionCenter;
        public Vector3 separation;
        public Vector3 follow;

        public int cohesionCount;

        public static int Size {
            get {
                return sizeof(float) * 3 * 6 + sizeof(int);
            }
        }
    }
}
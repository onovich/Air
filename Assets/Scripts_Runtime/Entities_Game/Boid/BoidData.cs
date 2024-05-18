using UnityEngine;

namespace Air {
    public struct BoidData {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 alignment;
        public Vector3 cohesionCenter;
        public Vector3 separation;
        public int otherCount;

        public static int Size {
            get {
                return sizeof(float) * 3 * 5 + sizeof(int);
            }
        }
    }
}
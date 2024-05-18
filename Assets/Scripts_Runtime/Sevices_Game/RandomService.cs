using System;
using UnityEngine;

namespace Air {

    public class RandomService {

        System.Random random;

        public RandomService() {
            random = new System.Random();
        }

        public int Next(int min, int max) {
            return random.Next(min, max);
        }

        public Quaternion Rotation() {
            return Quaternion.Euler(0, 0, (float)random.NextDouble() * 360);
        }

        public Vector2 InsideUnitCircle() {
            return new Vector2(Mathf.Sin((float)random.NextDouble() * 2 * Mathf.PI),
                               Mathf.Cos((float)random.NextDouble() * 2 * Mathf.PI));
        }

    }

}
using System;
using System.Collections.Generic;
using GameFunctions;
using UnityEngine;

namespace Air {

    public class BoidRepository {

        Dictionary<int, BoidEntity> all;

        BoidEntity[] allTemp;
        BoidEntity[] aroundTemp;

        public BoidRepository() {
            all = new Dictionary<int, BoidEntity>();
            allTemp = new BoidEntity[1000];
            aroundTemp = new BoidEntity[20];
        }

        public void Add(BoidEntity boid) {
            all.Add(boid.entityID, boid);
        }

        public int TakeAll(out BoidEntity[] boids) {
            int count = all.Count;
            if (count > allTemp.Length) {
                allTemp = new BoidEntity[(int)(count * 1.5f)];
            }
            all.Values.CopyTo(allTemp, 0);
            boids = allTemp;
            return count;
        }

        public void Remove(BoidEntity boid) {
            all.Remove(boid.entityID);
        }

        public bool TryGetBoid(int entityID, out BoidEntity boid) {
            return all.TryGetValue(entityID, out boid);
        }

        public void ForEach(Action<BoidEntity> action) {
            foreach (var boid in all.Values) {
                action(boid);
            }
        }

        public void Clear() {
            all.Clear();
        }

    }

}
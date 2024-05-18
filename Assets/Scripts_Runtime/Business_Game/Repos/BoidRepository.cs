using System;
using System.Collections.Generic;
using UnityEngine;

namespace Air {

    public class BoidRepository {

        Dictionary<int, BoidEntity> all;

        BoidEntity[] temp;

        public BoidRepository() {
            all = new Dictionary<int, BoidEntity>();
            temp = new BoidEntity[1000];
        }

        public void Add(BoidEntity boid) {
            all.Add(boid.entityID, boid);
        }

        public int TakeAll(out BoidEntity[] boids) {
            int count = all.Count;
            if (count > temp.Length) {
                temp = new BoidEntity[(int)(count * 1.5f)];
            }
            all.Values.CopyTo(temp, 0);
            boids = temp;
            return count;
        }

        public void Remove(BoidEntity boid) {
            all.Remove(boid.entityID);
        }

        public bool TryGetBoid(int entityID, out BoidEntity boid) {
            return all.TryGetValue(entityID, out boid);
        }

        public bool IsInRange(int entityID, in Vector2 pos, float range) {
            bool has = TryGetBoid(entityID, out var boid);
            if (!has) {
                return false;
            }
            return Vector2.SqrMagnitude(boid.Pos - pos) <= range * range;
        }

        public void ForEach(Action<BoidEntity> action) {
            foreach (var boid in all.Values) {
                action(boid);
            }
        }

        public BoidEntity GetNeareast(AllyStatus allyStatus, Vector2 pos, float radius) {
            BoidEntity nearestBoid = null;
            float nearestDist = float.MaxValue;
            float radiusSqr = radius * radius;
            foreach (var boid in all.Values) {
                if (boid.allyStatus != allyStatus) {
                    continue;
                }
                float dist = Vector2.SqrMagnitude(boid.Pos - pos);
                if (dist <= radiusSqr && dist < nearestDist) {
                    nearestDist = dist;
                    nearestBoid = boid;
                }
            }
            return nearestBoid;
        }

        public void Clear() {
            all.Clear();
        }

    }

}
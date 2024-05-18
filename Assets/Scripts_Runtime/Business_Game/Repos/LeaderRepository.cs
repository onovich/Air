using System;
using System.Collections.Generic;
using UnityEngine;

namespace Air {

    public class LeaderRepository {

        Dictionary<int, LeaderEntity> all;

        LeaderEntity[] temp;

        public LeaderRepository() {
            all = new Dictionary<int, LeaderEntity>();
            temp = new LeaderEntity[1000];
        }

        public void Add(LeaderEntity leader) {
            all.Add(leader.entityID, leader);
        }

        public int TakeAll(out LeaderEntity[] leaders) {
            int count = all.Count;
            if (count > temp.Length) {
                temp = new LeaderEntity[(int)(count * 1.5f)];
            }
            all.Values.CopyTo(temp, 0);
            leaders = temp;
            return count;
        }

        public void Remove(LeaderEntity leader) {
            all.Remove(leader.entityID);
        }

        public bool TryGetLeader(int entityID, out LeaderEntity leader) {
            return all.TryGetValue(entityID, out leader);
        }

        public bool IsInRange(int entityID, in Vector2 pos, float range) {
            bool has = TryGetLeader(entityID, out var leader);
            if (!has) {
                return false;
            }
            return Vector2.SqrMagnitude(leader.Pos - pos) <= range * range;
        }

        public void ForEach(Action<LeaderEntity> action) {
            foreach (var leader in all.Values) {
                action(leader);
            }
        }

        public LeaderEntity GetNeareast(AllyStatus allyStatus, Vector2 pos, float radius) {
            LeaderEntity nearestLeader = null;
            float nearestDist = float.MaxValue;
            float radiusSqr = radius * radius;
            foreach (var leader in all.Values) {
                if (leader.allyStatus != allyStatus) {
                    continue;
                }
                float dist = Vector2.SqrMagnitude(leader.Pos - pos);
                if (dist <= radiusSqr && dist < nearestDist) {
                    nearestDist = dist;
                    nearestLeader = leader;
                }
            }
            return nearestLeader;
        }

        public void Clear() {
            all.Clear();
        }

    }

}
using System;
using System.Collections.Generic;
using GameFunctions;
using UnityEngine;

namespace Air {

    public class BoidRepository {

        Dictionary<int, BoidEntity> all;
        Dictionary<Vector2Int, List<BoidEntity>> gridDict;

        BoidEntity[] allTemp;
        BoidEntity[] aroundTemp;
        Vector2Int[] gridTemp;

        public BoidRepository() {
            all = new Dictionary<int, BoidEntity>();
            gridDict = new Dictionary<Vector2Int, List<BoidEntity>>();
            allTemp = new BoidEntity[1000];
            aroundTemp = new BoidEntity[20];
            gridTemp = new Vector2Int[1000];
        }

        public void Add(BoidEntity boid) {
            all.Add(boid.entityID, boid);
            Vector2Int gridPos = new Vector2Int((int)boid.Pos.x, (int)boid.Pos.y);
            if (!gridDict.TryGetValue(gridPos, out var list)) {
                list = new List<BoidEntity>();
                gridDict.Add(gridPos, list);
            }
            list.Add(boid);
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

        public void UpdatePos(BoidEntity boid, Vector2 oldPos) {
            Vector2Int oldGridPos = new Vector2Int((int)oldPos.x, (int)oldPos.y);
            Vector2Int newGridPos = new Vector2Int((int)boid.Pos.x, (int)boid.Pos.y);
            if (oldGridPos != newGridPos) {
                if (gridDict.TryGetValue(oldGridPos, out var list)) {
                    list.Remove(boid);
                }
                if (!gridDict.TryGetValue(newGridPos, out list)) {
                    list = new List<BoidEntity>();
                    gridDict.Add(newGridPos, list);
                }
                list.Add(boid);
            }
        }

        public void Remove(BoidEntity boid) {
            all.Remove(boid.entityID);
            Vector2Int gridPos = new Vector2Int((int)boid.Pos.x, (int)boid.Pos.y);
            if (gridDict.TryGetValue(gridPos, out var list)) {
                list.Remove(boid);
            }
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

        public int TryGetAround(int entityID, AllyStatus allyStatus, Vector2Int centerPos, int radius, int maxCount, out BoidEntity[] roles) {
            int gridCount = GFGrid.RectCycle_GetCellsBySpirals(centerPos, radius, gridTemp);
            int roleCount = 0;
            for (int i = 0; i < gridCount; i++) {
                var key = new Vector2Int(gridTemp[i].x, gridTemp[i].y);
                if (!gridDict.TryGetValue(key, out var list)) {
                    continue;
                }

                for (int j = 0; j < list.Count; j++) {
                    var boid = list[j];
                    if (boid.allyStatus != allyStatus) {
                        continue;
                    }
                    if (boid.IsDead()) {
                        continue;
                    }
                    if (boid.entityID == entityID) {
                        continue;
                    }
                    aroundTemp[roleCount++] = boid;
                    if (roleCount >= maxCount) {
                        break;
                    }
                }

                if (roleCount >= maxCount) {
                    break;
                }
            }
            roles = aroundTemp;
            return roleCount;
        }

        public BoidEntity GetNeareast(AllyStatus allyStatus, Vector2 pos, float radius) {
            BoidEntity nearestRole = null;
            float nearestDist = float.MaxValue;
            float radiusSqr = radius * radius;
            foreach (var role in all.Values) {
                if (role.IsDead()) {
                    continue;
                }
                if (role.allyStatus != allyStatus) {
                    continue;
                }
                float dist = Vector2.SqrMagnitude(role.Pos - pos);
                if (dist <= radiusSqr && dist < nearestDist) {
                    nearestDist = dist;
                    nearestRole = role;
                }
            }
            return nearestRole;
        }

        public void Clear() {
            all.Clear();
        }

    }

}
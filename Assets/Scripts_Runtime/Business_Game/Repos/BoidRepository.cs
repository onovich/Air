using System;
using System.Collections.Generic;
using UnityEngine;

namespace Air {

    public class BoidRepository {

        Dictionary<int, BoidEntity> all;
        Dictionary<Vector2Int, List<BoidEntity>> posDict;

        BoidEntity[] temp;
        Vector2Int[] cellsTemp;

        public BoidRepository() {
            all = new Dictionary<int, BoidEntity>();
            posDict = new Dictionary<Vector2Int, List<BoidEntity>>();
            temp = new BoidEntity[1000];
            cellsTemp = new Vector2Int[1000];
        }

        public void Add(BoidEntity boid) {
            all.Add(boid.entityID, boid);
            var pos = boid.Pos_GetPosInt();
            if (posDict.TryGetValue(pos, out var boids)) {
                boids.Add(boid);
            } else {
                var list = new List<BoidEntity>();
                list.Add(boid);
                posDict.Add(pos, list);
            }
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
            
            var pos = boid.Pos_GetPosInt();
            if (posDict.ContainsKey(pos)) {
                posDict[pos].Remove(boid);
            }
        }

        public void UpdatePosDict(BoidEntity boid) {
            var lastPos = boid.Pos_GetLastPosInt();
            var newPos = boid.Pos_GetPosInt();
            if (posDict.ContainsKey(lastPos)) {
                posDict[lastPos].Remove(boid);
            } else {
                ALog.LogError("Dont's has old key in PosDict, boid entityID = " + boid.entityID);
            }
            if (posDict.ContainsKey(newPos)) {
                posDict[newPos].Add(boid);
            } else {
                var list = new List<BoidEntity>();
                list.Add(boid);
                posDict.Add(newPos, list);
            }
        }

        public bool TryGetBoidByPosInt(Vector2Int pos, out List<BoidEntity> boids) {
            if (!posDict.TryGetValue(pos, out boids)) {
                return false;
            }
            return true;
        }

        public int TryGetAround(Vector2Int centerPos, float radius, int maxCount, out BoidEntity[] boids) {
            int gridCount = GameFunctions.GFGrid.CircleCycle_GetCells(centerPos, radius, cellsTemp);
            int boidCount = 0;
            for (int i = 0; i < gridCount; i++) {
                if (!posDict.TryGetValue(cellsTemp[i], out var list)) {
                    continue;
                }
                list.ForEach((boid) => {
                    temp[boidCount++] = boid;
                    if (boidCount >= maxCount) {
                        return;
                    }
                });
                if (boidCount >= maxCount) {
                    break;
                }
            }
            boids = temp;
            return boidCount;
        }

        public int TryGetAroundWithAlly(int entityID, AllyStatus allyStatus, Vector2Int centerPos, float radius, int maxCount, out BoidEntity[] boids) {
            int gridCount = GameFunctions.GFGrid.CircleCycle_GetCells(centerPos, radius, cellsTemp);
            int boidCount = 0;
            for (int i = 0; i < gridCount; i++) {
                if (!posDict.TryGetValue(cellsTemp[i], out var list)) {
                    continue;
                }
                list.ForEach((boid) => {
                    if (boid.allyStatus != allyStatus) {
                        return;
                    }
                    if (boid.entityID == entityID) {
                        return;
                    }
                    temp[boidCount++] = boid;
                    if (boidCount >= maxCount) {
                        return;
                    }
                });
                if (boidCount >= maxCount) {
                    break;
                }
            }
            boids = temp;
            return boidCount;
        }

        public bool TryGetBoid(int entityID, out BoidEntity boid) {
            return all.TryGetValue(entityID, out boid);
        }

        public bool IsInRange(int entityID, in Vector2 pos, float range) {
            bool has = TryGetBoid(entityID, out var boid);
            if (!has) {
                return false;
            }
            return Vector2.SqrMagnitude(boid.Pos_GetPos() - pos) <= range * range;
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
                float dist = Vector2.SqrMagnitude(boid.Pos_GetPos() - pos);
                if (dist <= radiusSqr && dist < nearestDist) {
                    nearestDist = dist;
                    nearestBoid = boid;
                }
            }
            return nearestBoid;
        }

        public void Clear() {
            all.Clear();
            posDict.Clear();
        }

    }

}
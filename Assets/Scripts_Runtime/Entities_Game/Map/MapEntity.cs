using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Air {

    public class MapEntity : MonoBehaviour {

        public int typeID;
        public Vector3 mapSize;
        public Vector3 mapOffset;

        public Dictionary<Vector2, int> terrainTypeIDDict;
        [SerializeField] public SpriteRenderer spr;

        public void Ctor() {
            terrainTypeIDDict = new Dictionary<Vector2, int>();
        }

        public void SetSize(Vector3 size) {
            spr.size = size;
        }

        public bool Terrain_GetTypeID(Vector2 pos, out int typeID) {
            if (terrainTypeIDDict.ContainsKey(pos)) {
                typeID = terrainTypeIDDict[pos];
                return true;
            }
            typeID = -1;
            return false;
        }

        public void Terrain_ClearAll() {
        }

        public void TearDown() {
            Destroy(gameObject);
        }

    }

}
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Air.Modifier {

    public class MapEditorEntity : MonoBehaviour {

        [SerializeField] int typeID;
        [SerializeField] GameObject mapSize;
        [SerializeField] MapTM mapTM;
        [SerializeField] Tilemap tilemap_terrain;
        [SerializeField] TileBase tilebase_terrain;
        [SerializeField] Transform blockGroup;
        [SerializeField] Transform spikeGroup;
        [SerializeField] Transform spawnPointGroup;
        [SerializeField] Vector2 cameraConfinerWorldMax;
        [SerializeField] Vector2 cameraConfinerWorldMin;

        IndexService indexService;

        [Button("Bake")]
        void Bake() {
            indexService = new IndexService();
            indexService.ResetIndex();
            BakeMapInfo();
            BakeBlock();
            BakeSpike();
            BakeSpawnPoint();

            EditorUtility.SetDirty(mapTM);
            AssetDatabase.SaveAssets();
            Debug.Log("Bake Sucess");
        }

        void BakeMapInfo() {
            mapTM.typeID = typeID;
            mapTM.tileBase_terrain = tilebase_terrain;
            mapTM.mapSize = mapSize.transform.localScale;
            mapTM.mapPos = mapSize.transform.localPosition;
            mapSize.transform.localScale = mapTM.mapSize;
            mapSize.transform.localPosition = mapTM.mapPos;
        }

        void BakeSpike() {
            List<SpikeTM> spikeTMList = new List<SpikeTM>();
            List<Vector2> spikeSpawnPosList = new List<Vector2>();
            List<Vector2> spikeSpawnSizeList = new List<Vector2>();
            List<int> spikeIndexList = new List<int>();
            var spikeEditors = spikeGroup.GetComponentsInChildren<SpikerEditorEntity>();
            if (spikeEditors == null) {
                Debug.Log("BlockEditors Not Found");
            }
            for (int i = 0; i < spikeEditors.Length; i++) {
                var editor = spikeEditors[i];

                var tm = editor.spikeTM;
                spikeTMList.Add(tm);

                var pos = editor.GetPos();
                spikeSpawnPosList.Add(pos);

                var sizeInt = editor.GetSizeInt();
                spikeSpawnSizeList.Add(sizeInt);

                var index = indexService.PickSpikeIndex();
                spikeIndexList.Add(index);
                editor.index = index;

                editor.Rename();
            }
            mapTM.spikeSpawnArr = spikeTMList.ToArray();
            mapTM.spikeSpawnPosArr = spikeSpawnPosList.ToArray();
            mapTM.spikeSpawnSizeArr = spikeSpawnSizeList.ToArray();
            mapTM.spikeSpawnIndexArr = spikeIndexList.ToArray();
            mapTM.cameraConfinerWorldMax = cameraConfinerWorldMax;
            mapTM.cameraConfinerWorldMin = cameraConfinerWorldMin;
        }

        void BakeBlock() {
            List<BlockTM> blockTMList = new List<BlockTM>();
            List<Vector2> blockSpawnPosList = new List<Vector2>();
            List<Vector2> blockSpawnSizeList = new List<Vector2>();
            List<int> blockIndexList = new List<int>();
            var blockEditors = blockGroup.GetComponentsInChildren<BlockEditorEntity>();
            if (blockEditors == null) {
                Debug.Log("BlockEditors Not Found");
            }
            for (int i = 0; i < blockEditors.Length; i++) {
                var editor = blockEditors[i];

                var tm = editor.blockTM;
                blockTMList.Add(tm);

                var pos = editor.GetPos();
                blockSpawnPosList.Add(pos);

                var sizeInt = editor.GetSizeInt();
                blockSpawnSizeList.Add(sizeInt);

                var index = indexService.PickBlockIndex();
                blockIndexList.Add(index);
                editor.index = index;

                editor.Rename();
            }
            mapTM.blockSpawnArr = blockTMList.ToArray();
            mapTM.blockSpawnPosArr = blockSpawnPosList.ToArray();
            mapTM.blockSpawnSizeArr = blockSpawnSizeList.ToArray();
            mapTM.blockSpawnIndexArr = blockIndexList.ToArray();
        }

        void BakeSpawnPoint() {
            var editor = spawnPointGroup.GetComponent<SpawnPointEditorEntity>();
            if (editor == null) {
                Debug.Log("SpawnPointEditor Not Found");
            }
            editor.Rename();
            var pos = editor.GetPos();
            mapTM.SpawnPoint = pos;
        }

        void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((cameraConfinerWorldMax + cameraConfinerWorldMin) / 2, cameraConfinerWorldMax - cameraConfinerWorldMin);
        }

    }

}
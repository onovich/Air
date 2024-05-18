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
        [SerializeField] Transform blockGroup;
        [SerializeField] Transform spikeGroup;
        [SerializeField] Transform boidGroup;
        [SerializeField] Transform leaderGroup;
        [SerializeField] Transform spawnPointGroup;
        [SerializeField] Vector2 cameraConfinerWorldMax;
        [SerializeField] Vector2 cameraConfinerWorldMin;

        [Button("Bake")]
        void Bake() {
            BakeMapInfo();
            BakeBlock();
            BakeSpike();
            BakeBoid();
            BakeSpawnPoint();

            EditorUtility.SetDirty(mapTM);
            AssetDatabase.SaveAssets();
            Debug.Log("Bake Sucess");
        }

        void BakeMapInfo() {
            mapTM.typeID = typeID;
            mapTM.mapSize = mapSize.transform.localScale;
            mapTM.mapPos = mapSize.transform.localPosition;
            mapSize.transform.localScale = mapTM.mapSize;
            mapSize.transform.localPosition = mapTM.mapPos;
        }

        void BakeSpike() {
            List<SpikeTM> spikeTMList = new List<SpikeTM>();
            List<Vector2> spikeSpawnPosList = new List<Vector2>();
            List<Vector2> spikeSpawnSizeList = new List<Vector2>();
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

                var sizeInt = editor.GetSize();
                spikeSpawnSizeList.Add(sizeInt);

                editor.index = i;
                editor.Rename();
            }
            mapTM.spikeSpawnArr = spikeTMList.ToArray();
            mapTM.spikeSpawnPosArr = spikeSpawnPosList.ToArray();
            mapTM.spikeSpawnSizeArr = spikeSpawnSizeList.ToArray();
            mapTM.cameraConfinerWorldMax = cameraConfinerWorldMax;
            mapTM.cameraConfinerWorldMin = cameraConfinerWorldMin;
        }

        void BakeLeader(){
            List<LeaderTM> leaderTMList = new List<LeaderTM>();
            List<Vector2> leaderSpawnPosList = new List<Vector2>();
            List<AllyStatus> leaderAllyStatusList = new List<AllyStatus>();
            var leaderEditors = leaderGroup.GetComponentsInChildren<LeaderEditorEntity>();
            if (leaderEditors == null) {
                Debug.Log("LeaderEditors Not Found");
            }
            for (int i = 0; i < leaderEditors.Length; i++) {
                var editor = leaderEditors[i];

                var tm = editor.leaderTM;
                leaderTMList.Add(tm);

                var pos = editor.GetPos();
                leaderSpawnPosList.Add(pos);

                editor.index = i;
                editor.Rename();
            }
            mapTM.leaderSpawnArr = leaderTMList.ToArray();
            mapTM.leaderSpawnPosArr = leaderSpawnPosList.ToArray();
        }

        void BakeBoid() {
            List<BoidTM> boidTMList = new List<BoidTM>();
            List<Vector2> boidSpawnPosList = new List<Vector2>();
            List<AllyStatus> boidAllyStatusList = new List<AllyStatus>();
            var boidEditors = boidGroup.GetComponentsInChildren<BoidEditorEntity>();
            if (boidEditors == null) {
                Debug.Log("BoidEditors Not Found");
            }
            for (int i = 0; i < boidEditors.Length; i++) {
                var editor = boidEditors[i];

                var tm = editor.boidTM;
                boidTMList.Add(tm);

                var pos = editor.GetPos();
                boidSpawnPosList.Add(pos);

                editor.index = i;
                editor.Rename();
            }
            mapTM.boidSpawnArr = boidTMList.ToArray();
            mapTM.boidSpawnPosArr = boidSpawnPosList.ToArray();
        }

        void BakeBlock() {
            List<BlockTM> blockTMList = new List<BlockTM>();
            List<Vector2> blockSpawnPosList = new List<Vector2>();
            List<Vector2> blockSpawnSizeList = new List<Vector2>();
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

                var sizeInt = editor.GetSize();
                blockSpawnSizeList.Add(sizeInt);

                editor.index = i;
                editor.Rename();
            }
            mapTM.blockSpawnArr = blockTMList.ToArray();
            mapTM.blockSpawnPosArr = blockSpawnPosList.ToArray();
            mapTM.blockSpawnSizeArr = blockSpawnSizeList.ToArray();
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
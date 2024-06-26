using System;
using UnityEngine;

namespace Air {

    public class LeaderEntity : MonoBehaviour {

        // Base Info
        public int entityID;
        public int typeID;
        public AllyStatus allyStatus;

        // Attr
        public float moveSpeed;
        public float rotationSpeed;

        // Physics
        public Vector2 velocity;

        // FSM
        public LeaderFSMComponent fsmCom;

        // Input
        public LeaderInputComponent inputCom;

        // Render
        [SerializeField] SpriteRenderer spr;

        // Pos
        public Vector3 Pos => Pos_GetPos();
        public Vector2 Dir => transform.up;

        // VFX
        public string deadVFXName;
        public float deadVFXDuration;

        // TearDown
        public bool needTearDown;

        public void Ctor() {
            inputCom = new LeaderInputComponent();
            fsmCom = new LeaderFSMComponent();
        }

        // Pos
        public void Pos_SetPos(Vector2 pos) {
            transform.position = pos;
        }

        Vector2 Pos_GetPos() {
            return transform.position;
        }

        // Attr
        public float Attr_GetMoveSpeed() {
            return moveSpeed;
        }

        // Move

        // FSM
        public LeaderFSMStatus FSM_GetStatus() {
            return fsmCom.status;
        }

        public LeaderFSMComponent FSM_GetComponent() {
            return fsmCom;
        }

        public void FSM_EnterIdle() {
            fsmCom.EnterNormal();
        }

        public void FSM_EnterDead() {
            fsmCom.EnterDead();
        }

        // Mesh
        public void Mesh_Set(Sprite sp) {
            this.spr.sprite = sp;
        }

        public void TearDown() {
            Destroy(this.gameObject);
        }

    }

}
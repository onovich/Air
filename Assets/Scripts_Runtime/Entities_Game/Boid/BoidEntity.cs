using System;
using UnityEngine;

namespace Air {

    public class BoidEntity : MonoBehaviour {

        // Base Info
        public int entityID;
        public int typeID;
        public AllyStatus allyStatus;

        // Attr
        public int hp;
        public int hpMax;

        // Physics
        public Vector2 velocity;

        // FSM
        public BoidFSMComponent fsmCom;

        // Input
        public BoidInputComponent inputCom;

        // Render
        [SerializeField] public Transform body;
        [SerializeField] SpriteRenderer spr;

        // VFX
        public string deadVFXName;
        public float deadVFXDuration;

        // Pos
        public Vector2 Pos => transform.position;
        public Vector2Int GridPos => new Vector2Int((int)Pos.x, (int)Pos.y);

        // TearDown
        public bool needTearDown;

        public void Ctor() {
            fsmCom = new BoidFSMComponent();
            inputCom = new BoidInputComponent();
        }

        // Pos
        public void Pos_SetPos(Vector2 pos) {
            transform.position = pos;
        }

        // Velocity
        public void Velocity_Set(Vector2 velocity) {
            this.velocity = velocity;
        }

        // Attr
        public void Attr_GetHurt() {
            hp -= 1;
        }

        public void Attr_DeadlyHurt() {
            hp = 0;
        }

        public bool IsDead() {
            return hp <= 0 || needTearDown || fsmCom.status == BoidFSMStatus.Dead;
        }

        // Move
        public void Move_SetUp(Vector2 dir) {
            this.transform.up = dir;
        }

        // FSM
        public BoidFSMStatus FSM_GetStatus() {
            return fsmCom.status;
        }

        public BoidFSMComponent FSM_GetComponent() {
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
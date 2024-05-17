using System;
using UnityEngine;

namespace Air {

    public class BoidEntity : MonoBehaviour {

        // Base Info
        public int entityID;
        public int typeID;
        public AllyStatus allyStatus;

        // Attr
        public float moveSpeed;
        public float rotationSpeed;
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
        public Vector2 Pos => Pos_GetPos();

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

        Vector2 Pos_GetPos() {
            return transform.position;
        }

        // Attr
        public float Attr_GetMoveSpeed() {
            return moveSpeed;
        }

        public void Attr_GetHurt() {
            hp -= 1;
        }

        public void Attr_DeadlyHurt() {
            hp = 0;
        }

        // Move
        public void Move_ApplyMove(float dt) {
            Move_Apply(inputCom.moveAxis.x, Attr_GetMoveSpeed(), dt);
        }

        public void Move_Stop() {
            Move_Apply(0, 0, 0);
        }

        void Move_Apply(float xAxis, float moveSpeed, float fixdt) {
            velocity.x = xAxis * moveSpeed;
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
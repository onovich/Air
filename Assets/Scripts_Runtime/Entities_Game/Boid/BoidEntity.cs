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
        public Vector2Int GridPos => new Vector2Int((int)Pos.x, (int)Pos.y);
        public Vector2 dir;

        // TearDown
        public bool needTearDown;

        public void Ctor() {
            fsmCom = new BoidFSMComponent();
            inputCom = new BoidInputComponent();
            dir = Vector2.up;
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

        public bool IsDead() {
            return hp <= 0 || needTearDown || fsmCom.status == BoidFSMStatus.Dead;
        }

        // Move
        public void Move_ApplyMove(float dt) {
            Move_Apply(inputCom.moveAxis, Attr_GetMoveSpeed(), dt);
        }

        public void Move_Stop() {
            Move_Apply(Vector2.zero, 0, 0);
        }

        void Move_Apply(Vector2 axis, float moveSpeed, float fixdt) {
            if (axis != Vector2.zero) {
                velocity.x = axis.x * moveSpeed;
                velocity.y = axis.y * moveSpeed;
            }
            this.transform.position += (Vector3)velocity * fixdt;
            dir = velocity.normalized;
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
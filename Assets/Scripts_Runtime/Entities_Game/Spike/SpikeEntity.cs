using System;
using UnityEngine;

namespace Air {

    public class SpikeEntity : MonoBehaviour {

        // Base Info
        public int entityID;
        public int typeID;

        // Render
        [SerializeField] public Transform body;
        [SerializeField] SpriteRenderer spr;
        [SerializeField] BoxCollider2D boxCollider;

        // Pos
        public Vector2 Pos => transform.position;

        public void Ctor() {
        }

        // Pos
        public void Pos_SetPos(Vector2 pos) {
            transform.position = pos;
        }

        Vector2 Pos_GetPos() {
            return transform.position;
        }

        // Size
        public void Size_SetSize(Vector2 size) {
            spr.size = size;
            size.y *= 0.5f;
            boxCollider.size = size;
        }

        // Mesh
        public void Mesh_Set(Sprite sp) {
            this.spr.sprite = sp;
        }

        // Rename
        public void Rename() {
            this.name = $"Spike - {typeID} - {entityID}";
        }

        public void TearDown() {
            Destroy(gameObject);
        }

    }

}
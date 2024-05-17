using UnityEngine;

namespace Air.Modifier {

    public class SpawnPointEditorEntity : MonoBehaviour {

        public void Rename() {
            this.gameObject.name = "Spawn Point";
        }

        public Vector2 GetPos() {
            var pos = this.transform.position;
            this.transform.position = pos;
            return pos;
        }

        public Vector2 GetSizeInt() {
            var size = transform.localScale;
            var sizeInt = size;
            return sizeInt;
        }

    }

}
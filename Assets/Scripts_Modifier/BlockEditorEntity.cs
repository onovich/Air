using UnityEngine;

namespace Air.Modifier {

    public class BlockEditorEntity : MonoBehaviour {

        [SerializeField] public BlockTM blockTM;
        public int index;

        public void Rename() {
            this.gameObject.name = $"Block - {blockTM.typeID} - {index}";
        }

        public Vector2 GetPos() {
            var pos = this.transform.position;
            this.transform.position = pos;
            return pos;
        }

        public Vector2 GetSizeInt() {
            var size = GetComponent<SpriteRenderer>().size;
            var sizeInt = size;
            return sizeInt;
        }

    }

}
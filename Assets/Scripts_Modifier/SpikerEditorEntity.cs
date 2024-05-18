using UnityEngine;

namespace Air.Modifier {

    public class SpikerEditorEntity : MonoBehaviour {

        [SerializeField] public SpikeTM spikeTM;
        public int index;

        public void Rename() {
            this.gameObject.name = $"Spike - {spikeTM.typeID} - {index}";
        }

        public Vector2 GetPos() {
            var pos = this.transform.position;
            this.transform.position = pos;
            return pos;
        }

        public Vector2 GetSize() {
            var size = GetComponent<SpriteRenderer>().size;
            return size;
        }

    }

}
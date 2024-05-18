using UnityEngine;

namespace Air.Modifier {

    public class BoidEditorEntity : MonoBehaviour {

        [SerializeField] public BoidTM boidTM;
        public int index;
        [SerializeField] AllyStatus allyStatus;

        public void Rename() {
            this.gameObject.name = $"Boid - {boidTM.typeID} - {index}";
        }

        public AllyStatus GetAllyStatus() {
            return allyStatus;
        }

        public Vector2 GetPos() {
            var pos = this.transform.position;
            this.transform.position = pos;
            return pos;
        }

    }

}
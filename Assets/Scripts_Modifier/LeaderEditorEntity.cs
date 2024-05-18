using UnityEngine;

namespace Air.Modifier {

    public class LeaderEditorEntity : MonoBehaviour {

        [SerializeField] public LeaderTM leaderTM;
        public int index;
        [SerializeField] AllyStatus allyStatus;

        public void Rename() {
            this.gameObject.name = $"Leader - {leaderTM.typeID} - {index}";
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
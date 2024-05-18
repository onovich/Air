namespace Air {

    public class LeaderFSMComponent {

        public LeaderFSMStatus status;

        public bool normal_isEntering;
        public bool dead_isEntering;

        public LeaderFSMComponent() { }

        public void EnterNormal() {
            status = LeaderFSMStatus.Normal;
            normal_isEntering = true;
        }

        public void EnterDead() {
            status = LeaderFSMStatus.Dead;
            dead_isEntering = true;
        }

    }

}
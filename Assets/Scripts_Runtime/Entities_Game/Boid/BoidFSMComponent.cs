namespace Air {

    public class BoidFSMComponent {

        public BoidFSMStatus status;

        public bool normal_isEntering;
        public bool dead_isEntering;

        public BoidFSMComponent() { }

        public void EnterNormal() {
            status = BoidFSMStatus.Normal;
            normal_isEntering = true;
        }

        public void EnterDead() {
            status = BoidFSMStatus.Dead;
            dead_isEntering = true;
        }

    }

}
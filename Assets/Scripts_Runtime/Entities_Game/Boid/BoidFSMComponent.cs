namespace Air {

    public class BoidFSMComponent {

        public BoidFSMStatus status;

        public bool idle_isEntering;
        public bool dead_isEntering;

        public BoidFSMComponent() { }

        public void EnterIdle() {
            status = BoidFSMStatus.Idle;
            idle_isEntering = true;
        }

        public void EnterDead() {
            status = BoidFSMStatus.Dead;
            dead_isEntering = true;
        }

    }

}
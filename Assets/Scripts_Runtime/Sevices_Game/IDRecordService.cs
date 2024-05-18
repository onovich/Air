namespace Air {

    public class IDRecordService {

        int boidEntityID;
        int leaderEntityID;
        int blockEntityID;
        int spikeEntityID;

        public IDRecordService() { }

        public int PickBoidEntityID() {
            return ++boidEntityID;
        }

        public int PickLeaderEntityID() {
            return ++leaderEntityID;
        }

        public int PickBlockEntityID() {
            return ++blockEntityID;
        }

        public int PickSpikeEntityID() {
            return ++spikeEntityID;
        }

        public void Reset() {
            boidEntityID = 0;
            leaderEntityID = 0;
            blockEntityID = 0;
            spikeEntityID = 0;
        }
    }

}
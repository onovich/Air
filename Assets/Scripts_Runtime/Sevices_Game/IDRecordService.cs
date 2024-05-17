namespace Air {

    public class IDRecordService {

        int boidEntityID;

        public IDRecordService() { }

        public int PickBoidEntityID() {
            return ++boidEntityID;
        }

        public void Reset() {
            boidEntityID = 0;
        }
    }

}
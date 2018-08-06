namespace MultithreadedCarConveyor
{
    public class Detail
    {
        public int numModel { get; set; }
        public int numPosInModel { get; set; }

        public Detail(int nm, int np)
        {
            numModel = nm;
            numPosInModel = np;
        }
    }
}

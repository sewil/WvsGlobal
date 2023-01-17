namespace WvsBeta.Common.Objects
{
    public class WorldRanking
    {
        public int worldRanking;
        public int worldChange;
        public int classRanking;
        public int classChange;
        public WorldRanking(int worldRanking, int worldChange, int classRanking, int classChange)
        {
            this.worldRanking = worldRanking;
            this.worldChange = worldChange;
            this.classRanking = classRanking;
            this.classChange = classChange;
        }
    }
}

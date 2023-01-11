namespace WvsBeta.Common.Objects
{
    public class World
    {
        public class Ranking
        {
            public int worldRanking;
            public int worldChange;
            public int classRanking;
            public int classChange;
            public Ranking(int worldRanking, int worldChange, int classRanking, int classChange)
            {
                this.worldRanking = worldRanking;
                this.worldChange = worldChange;
                this.classRanking = classRanking;
                this.classChange = classChange;
            }
        }
        public enum Ribbon : byte
        {
            Normal,
            Event,
            New,
            Hot
        }
        public enum Warning : byte
        {
            NoWarning,
            HighUsers,
            MaxUsers
        }
        public enum Marker : byte
        {
            NoMarker,
            HighlyPopulated,
            Overpopulated
        }
    }
}

namespace WvsBeta.Common.Objects
{
    public class CompletedQuest
    {
        public short QuestID { get; private set; }
        long _endTime;
        public long EndTime
        {
            get => _endTime;
            private set
            {
                _endTime = value;
                FileTime = MasterThread.GetFileTime(value);
            }
        }
        public long FileTime { get; private set; }
        public CompletedQuest(short questid, long endtime)
        {
            QuestID = questid;
            EndTime = endtime;
        }
    }
}

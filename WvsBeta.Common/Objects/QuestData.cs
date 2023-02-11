using WvsBeta.Common.Enums;

namespace WvsBeta.Common.Objects
{
    public class QuestData
    {
        public int ID { get; set; }
        public short QuestID { get; set; }
        public string Data { get; set; }
        public QuestState State { get; set; }
        long _endTime;
        public long EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                FileTime = MasterThread.GetFileTime(value);
            }
        }
        public long FileTime { get; private set; }
    }
}

namespace WvsBeta.Login.Objects
{
    public class Player : Common.Objects.Player
    {
        public ClientSession Socket { get; set; }
        public string PIN { get; set; }
        public string MachineID { get; set; }
        public int MachineBanCount { get; set; }
        public int UniqueBanCount { get; set; }
        public int IPBanCount { get; set; }
        public int UniqueID { get; set; }
    }
}

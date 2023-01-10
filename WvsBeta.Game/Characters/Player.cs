namespace WvsBeta.Game
{
    public class Player
    {
        public string SessionHash { get; set; }
        public Character Character { get; set; }
        public ClientSession Socket { get; set; }
        public bool IsCC { get; set; } = false;
    }
}

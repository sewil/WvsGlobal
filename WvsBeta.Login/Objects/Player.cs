namespace WvsBeta.Login.Objects
{
    public class Player : Common.Objects.Player
    {
        public ClientSession Socket { get; set; }
        public string PIN { get; set; }
    }
}

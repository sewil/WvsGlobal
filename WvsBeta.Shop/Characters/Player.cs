namespace WvsBeta.Shop
{
    public class Player
    {
        public string SessionHash { get; set; }
        public ShopCharacter Character { get; set; }
        public ClientSocket Socket { get; set; }
        public bool IsCC { get; set; } = false;
    }
}

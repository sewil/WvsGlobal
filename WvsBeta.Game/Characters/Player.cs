using WvsBeta.Common.Objects;

namespace WvsBeta.Game
{
    public class Player : Common.Objects.Player
    {
        public Character Character { get; set; }
        public ClientSession Socket { get; set; }
        public bool IsCC { get; set; } = false;
    }
}

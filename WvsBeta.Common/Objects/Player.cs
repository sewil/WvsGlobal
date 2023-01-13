using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Common.Enums;

namespace WvsBeta.Common.Objects
{
    public partial class Player
    {
        public string Username { get; set; }
        public int ID { get; set; }
        public PlayerGender Gender { get; set; }
        public byte GMLevel { get; set; }
        public bool IsGM { get => GMLevel > 0; }
        public bool IsAdmin { get => GMLevel >= 3; }
        public bool LoggedOn { get; set; } = false;
        public int DateOfBirth { get; set; }
        public GameState State { get; set; } = GameState.LoginScreen;
        public byte World { get; set; }
        public byte Channel { get; set; }
        public string SessionHash { get; set; }

        public Dictionary<int, string> Characters { get; } = new Dictionary<int, string>();

        public bool HasCharacterWithID(int id)
        {
            return Characters.ContainsKey(id);
        }

    }
}

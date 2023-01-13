using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WvsBeta.Common.Enums
{
    public enum GameState
    {
        None,
        LoginScreen,
        SetupGender,
        ConfirmEULA,
        PinCheck,
        WorldSelect,
        ChannelSelect,
        CharacterSelect,
        CharacterCreation,
        Field
    }
}

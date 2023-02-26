using WvsBeta.Game;
using WvsBeta.Game.Packets;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    #region Reactors
    [Script("boss")]
    class boss : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            if (target.Owner == null) return;
            FieldSet.Instances["ZakumEnter"].SetVar("summoned", "true");
            FieldSet.Instances["ZakumEnter"].SetVar("canEnter", "false");
            target.Field.SendPacket(FieldEffectPacket.EffectMusic("Bgm06/FinalFight"));
            var mobOwner = target.Spawn(-11, (8800000, 1, -4, null))[0];
            for (var i = 8800003; i < 8800011; i++)
            {
                target.Spawn(-11, (i, 1, -2, mobOwner));
            }
            //target.Field.createMapMonitor(280030000, "ps00");
            ChatPacket.SendText(ChatPacket.MessageTypes.RedText, "Zakum is summoned by the force of Eye of Fire.", target.Owner, ChatPacket.MessageMode.ToMap);
        }
    }
    #endregion
}

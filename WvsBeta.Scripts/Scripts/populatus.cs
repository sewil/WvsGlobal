using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    // The portal that leads to the Boss Map
    [Script("Populatus00")]
    class Populatus00 : IPortalScript
    {
        public void Run(IPortalHost self, GameCharacter target)
        {
            var inven = target.Inventory;
            var instance = FieldSet.Instances["Populatus"];
            var qr = target.QuestRecord;
            var lTime = qr.Get(7200);
            int.TryParse(qr.Get(7201), out int enterCount);

            if (inven.ItemCount(4031172) < 1) return;
            var cTime = MasterThread.CurrentTimeStr;
            var aTime = MasterThread.CompareTime(cTime, lTime);
            if (aTime >= 1440) enterCount = 0;
            if (!target.IsAdmin && aTime < 1440 && enterCount >= 2)
            {
                target.Message("You can only enter The Origin of Clocktower twice a day.");
                return;
            }
            else if (instance.Started)
            {
                target.Message("The battle against Papulatus has already begun, so you may not enter this place.");
            }
            else
            {
                var status = instance.Enter(target, 0, "st00", false);
                if (status == FieldSet.EnterStatus.Full)
                {
                    target.Message("The room is already in full capacity with people battling against Papulatus.");
                }
                else if (status != FieldSet.EnterStatus.Success)
                {
                    target.Message("There are mysterious forces are at work, so you may not enter at this time.");
                }
                else
                {
                    qr.Set(7200, cTime);
                    qr.Set(7201, (++enterCount).ToString());
                    target.PlayPortalSE();
                }
            }
        }
    }
    // �����ġ NPC : ��������...
    [Script("Populatus01")]
    class Populatus01 : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var nRet = self.AskYesNo("Beep... beep... you can make your escape to a safer place through me. Beep ... beep ... would you like to leave this place?");
            if (nRet == 1) target.ChangeMap(220080000, "st00");
        }
    }
    #region Reactors
    [Script("boss2")]
    class Boss2 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            var instance = FieldSet.Instances["Populatus"];
            instance.OnEnd += (obj, _) => TriggerGate();
            instance.Start();
            TriggerGate();
            target.Field.Message("The crack of dimension was filled by the <Piece of Cracked Dimension>.");
            target.Field.EffectMusic("Bgm09/TimeAttack");
            var offset = new Pos((short)(-410 - target.Position.X), (short)(-400 - target.Position.Y));
            target.SpawnMob(offset, (8500000, 1, SummonType.Poof, null));
        }
        void TriggerGate()
        {
            GameDataProvider.Maps[MapIds.DeepInsideClocktower].ReactorPool.TriggerReactor("ludigate1");
        }
    }
    #endregion
}

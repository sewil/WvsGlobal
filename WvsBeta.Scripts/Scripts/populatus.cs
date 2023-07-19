using System;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class populatus
    {
        // The portal that leads to the Boss Map
        [Script("Populatus00")]
        class Populatus00 : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var inven = target.Inventory;
                var quest = FieldSet.Instances["Populatus"];
                var result = quest.GetVar("ludiboss");
                var users = quest.UserCount;
                var qr = target.QuestRecord;
                var lTime = qr.Get(7200);
                var count = qr.Get(7201);

                if (inven.ItemCount(4031172) >= 1)
                {
                    var cTime = MasterThread.CurrentTimeStr;
                    var aTime = MasterThread.CompareTime(cTime, lTime);
                    // After one day
                    if (aTime >= 1440 || target.IsAdmin || MasterThread.IsDebug)
                    {
                        if (result == "yes") target.Message("The battle against Papulatus has already begun, so you may not enter this place.");
                        else
                        {
                            if (users < 12)
                            {
                                qr.Set(7200, cTime);
                                qr.Set(7201, "1");
                                target.PlayPortalSE();
                                target.ChangeMap(220080001, "st00");
                            }
                            else target.Message("The room is already in full capacity with people battling against Papulatus.");
                        }
                    }
                    else
                    {
                        if (result == "yes") target.Message("The battle against Papulatus has already begun, so you may not enter this place.");
                        else
                        {
                            if (users < 12)
                            {
                                if (count == "")
                                {
                                    qr.Set(7200, cTime);
                                    qr.Set(7201, "1");
                                    target.PlayPortalSE();
                                    target.ChangeMap(220080001, "st00");
                                }
                                else if (count == "1")
                                {
                                    qr.Set(7201, "2");
                                    target.PlayPortalSE();
                                    target.ChangeMap(220080001, "st00");
                                }
                                else target.Message("You can only enter The Origin of Clocktower twice a day.");
                            }
                            else target.Message("The room is already in full capacity with people battling against Papulatus.");
                        }
                    }
                }
            }
        }
        // ±â°èÀåÄ¡ NPC : Žë±âžÊÀž·Î...
        [Script("Populatus01")]
        class Populatus01 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var nRet = self.AskYesNo("Beep... beep... you can make your escape to a safer place through me. Beep ... beep ... would you like to leave this place?");
                if (nRet == 1) target.ChangeMap(220080000, "st00");
            }
        }
    }
}

using WvsBeta.Common.Extensions;
using WvsBeta.Game;
using WvsBeta.Game.Handlers.Guild;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class ossyria2
    {
        static INpcHost self;
        static GameCharacter target;
        // The Magic Rock @ Orbis 
        [Script("ossyria3_1")]
        class ossyria3_1 : INpcScript
        {
            dynamic inven, ret1, ret2;
            public void Run(INpcHost self, GameCharacter target)
            {
                ossyria2.self = self;
                ossyria2.target = target;
                inven = target.Inventory;

                if (inven.ItemCount(4001019) >= 1)
                {
                    ret1 = self.AskYesNo("Você pode usar #b#t4001019##k para ativar #b#p2012014##k. Você vai se teletransportar para onde está #b#p2012015##k?");
                    if (ret1 != 0)
                    {
                        ret2 = inven.Exchange(0, 4001019, -1);
                        if (ret2 == 0) self.Say("Impossível ativar #b#p2012014##k porque você não tem #b#t4001019##k.");
                        else target.ChangeMap(200082100, "sp");
                    }
                }
                else self.Say("Existe um #b#p2012014##k que permite que você se teletransporte para onde está #b#p2012015##k, mas você não pode ativá-lo sem o pergaminho.");
            }
        }
        // The Magic Rock @ El Nath 
        [Script("ossyria3_2")]
        class ossyria3_2 : INpcScript
        {
            dynamic inven, ret1, ret2;
            public void Run(INpcHost self, GameCharacter target)
            {
                ossyria2.self = self;
                ossyria2.target = target;
                inven = target.Inventory;

                if (inven.ItemCount(4001019) >= 1)
                {
                    ret1 = self.AskYesNo("Você pode usar #b#t4001019##k para ativar #b#p2012015##k. Você vai se teletransportar para onde está #b#p2012014##k?");
                    if (ret1 != 0)
                    {
                        ret2 = inven.Exchange(0, 4001019, -1);
                        if (ret2 == 0) self.Say("Impossível ativar #b#p2012015##k porque você não tem #b#t4001019##k.");
                        else target.ChangeMap(200080200, "sp");
                    }
                }
                else self.Say("Existe um #b#p2012015##k que permite que você se teletransporte para onde está #b#p2012014##k, mas você não pode ativá-lo sem o pergaminho.");
            }
        }
        // Guild 
        [Script("guild_proc")]
        class guild_proc : INpcScript
        {
            bool isGuildMaster, isGuildMember;
            int nRequiredMeso, nRet1, v1, nRet, nCountMax;
            public void Run(INpcHost self, GameCharacter target)
            {
                ossyria2.self = self;
                ossyria2.target = target;
                isGuildMaster = target.IsGuildMaster;
                isGuildMember = target.IsGuildMember;
                if (!isGuildMember)
                {
                    self.Say("Hey... would you happen to be interested in GUILDs by any chance?");

                    v1 = self.AskMenu("#b#L0#What's a guild?#l\r\n#L1#What do I do to form a guild?#l\r\n#L2# I want to start a guild#l");
                    if (v1 == 0)
                    {
                        self.Say("A guild is... well, you can think of it as a small group full of people with similar interests and goals. Furthermore, it will be registered at our Guild Headquarters to be validated.");
                    }
                    else if (v1 == 1)
                    {
                        self.Say("To form your own guild, you will need to be at least level 10. You will also need to have at least 1,500,000 mesos with you. This is the price for registering your guild.");
                        self.Say("To make a guild, you will need 6 people in total. Those 6 must be in the same group and the leader must come and talk to me. Also be aware that the party leader also becomes the Guild Master. Once the Guild Master is designated, the position remains the same until the Guild is disbanded.");
                        self.Say("Once 6 people are gathered, you will need 1,500,000 mesos. This is the price for registering your guild.");
                        self.Say("Okay, to register your guild, bring people here~ You can't create one without all 6.\r\nOh, of course, the 6 can't be part of another guild!");
                    }
                    else if (v1 == 2)
                    {
                        nRet1 = self.AskYesNo("Okay, now, do you want to create a guild?");

                        if (nRet1 == 1)
                        {
                            int nRet2 = (int)target.IsCreateGuildPossible(1500000);
                            if (nRet2 == 0)
                            {
                                self.Say("Enter your guild name and it will be created. The Guild will also be officially registered at our Guild Headquarters. So good luck to you and your guild!");
                                target.CreateNewGuild(-1500000);
                            }
                            else if (nRet2 == 1) self.Say("Hmm... I don't think you have the qualifications to be a Guild Master. Please train more to become Guild Master.");
                            else if (nRet2 == 2) self.Say("I don't care how strong you think you are... To form a guild, you need to be in a party of 6. Create a party and then bring all members here if you really want to create a guild.");
                            else if (nRet2 == 3) self.Say("You are not the leader of a party.");
                            else if (nRet2 == 4) self.Say("It looks like you don't have enough members in your party or some of the members aren't present. I need all 6 members here to register your guild. If your party can't coordinate this simple task, you should think twice about forming a guild.");
                            else if (nRet2 == 5) self.Say("It seems there is a traitor among us. Someone in your party is already part of another guild. To form a guild, everyone in your party needs to be guildless. Come back when you've solved the problem with the traitor."); // cue among us theme
                            else if (nRet2 == 6)
                            {
                                self.Say("Do you have enough mesos or people to form a guild...? Please check again and come back here.");
                                self.Say("Check again. You need to pay the service cost to create a guild and register it.");
                            }
                        }
                    }
                }
                else
                {
                    v1 = self.AskMenu("So, how can I help?\r\n#b#L0#I want to expand my guild#l\r\n#L1#I want to disband my guild#l");

                    if (isGuildMaster)
                    {
                        if (v1 == 0)
                        {
                            nCountMax = target.GetGuildCountMax;

                            if (nCountMax > 95) self.Say("Your guild seems to have grown quite a bit. I can no longer expand your guild.");
                            else
                            {
                                self.Say("Are you here to expand your guild? Your guild must have grown quite a bit~ To expand your guild, it needs to be re-registered at our Guild Headquarters and this will require a payment for the service...");
                                if (nCountMax <= 10) nRequiredMeso = 50;
                                else if (nCountMax <= 15) nRequiredMeso = 150;
                                else if (nCountMax <= 20) nRequiredMeso = 250;
                                else if (nCountMax <= 25) nRequiredMeso = 350;
                                else if (nCountMax <= 30) nRequiredMeso = 450;
                                else if (nCountMax <= 35) nRequiredMeso = 500;
                                else if (nCountMax <= 40) nRequiredMeso = 500;
                                else if (nCountMax <= 45) nRequiredMeso = 500;
                                else if (nCountMax <= 50) nRequiredMeso = 500;
                                else if (nCountMax <= 55) nRequiredMeso = 500;
                                else if (nCountMax <= 60) nRequiredMeso = 500;
                                else if (nCountMax <= 65) nRequiredMeso = 500;
                                else if (nCountMax <= 70) nRequiredMeso = 500;
                                else if (nCountMax <= 75) nRequiredMeso = 500;
                                else if (nCountMax <= 80) nRequiredMeso = 500;
                                else if (nCountMax <= 85) nRequiredMeso = 500;
                                else if (nCountMax <= 90) nRequiredMeso = 500;
                                else if (nCountMax <= 95) nRequiredMeso = 500;
                                nRequiredMeso *= 10000;

                                nRet = self.AskYesNo("The cost of the service will only be #r" + nRequiredMeso.Culture() + " mesos#k. Would you like to expand your guild?");

                                if (nRet == 1)
                                {
                                    bool fRet = target.IncGuildCountMax(5, -nRequiredMeso);
                                    if (!fRet)
                                    {
                                        self.Say("Do you have enough mesos or people to form a guild...? Please check again and come back here.");
                                        self.Say("Check again.You will need to pay the service cost to expand and re-register your guild...");
                                    }
                                }
                            }
                        }
                        else if (v1 == 1)
                        {
                            nRet = self.AskYesNo("Are you sure you want to disband your guild? Seriously... Remember, if you disband the guild, it will be gone forever. Oh, and one more thing. If you want to disband your guild, you will need to pay 200,000 mesos for the service cost. Are you sure you still want to do this?");
                            if (nRet == 0) self.Say("Well thought. I wouldn't want to disband my already strong guild...");
                            else
                            {
                                bool fRet = target.RemoveGuild(-200000);
                                if (!fRet) self.Say("Hey, you don't have the mesos for the service... are you sure you have enough mesos there?");
                            }
                        }
                    }
                    else self.Say("Hey, you're not the Guild Master! This decision can only be made by the Guild Master.");
                }
            }
        }
        [Script("guild_mark")]
        class guild_mark : INpcScript
        {
            dynamic v1, nRet;
            public void Run(INpcHost self, GameCharacter target)
            {
                ossyria2.self = self;
                ossyria2.target = target;
                if (target.IsGuildMaster)
                {
                    v1 = self.AskMenu("Hey? My name is #bLea#k. I am responsible for the #bGuild Emblem#k.\r\n#b#L0#I'd like to register a guild emblem.#l\r\n#L1#I' like to delete a guild emblem.#l");
                    if (v1 == 0)
                    {
                        if (target.IsGuildMarkExist) self.Say("Looks like you've already registered a Guild Emblem. Please delete your guild emblem first in order to register a new one.");
                        else
                        {
                            nRet = self.AskYesNo("There is a fee of #r5,000,000 mesos#k for creating a Guild Emblem. To further explain, a Guild Emblem is like a coat of arms that is unique to a guild. It will be displayed to the left of the guild name. How does that sound? Would you like to create a Guild Emblem?");
                            if (nRet == 1)
                            {
                                if (target.Inventory.Mesos >= 5000000) target.SetGuildMark(-5000000);
                                else self.Say("You don't have enough mesos. You need at least #b5,000,000 mesos# to register a Guild Emblem.");
                            }
                            else self.Say("Oh... I see... The emblem would bring the guild closer together. Do you need more time to prepare the Guild Emblem? Please come back whenever you want.");
                        }
                    }
                    else if (v1 == 1)
                    {
                        if (target.IsGuildMarkExist)
                        {
                            nRet = self.AskYesNo("If you delete your current Guild Emblem, you can create a new one. You need #r1,000,000 mesos#k to delete it. Would you like to delete your Guild Emblem?");
                            if (nRet == 1)
                            {
                                if (target.Inventory.Mesos >= 1000000) target.RemoveGuildMark(-1000000);
                                else self.Say("You don't have enough mesos to delete your Guild Emblem. You need #b1,000,000 mesos #k to delete it.");
                            }
                            else self.Say("Please come back whenever you want.");
                        }
                        else self.Say("Huh? Weird... You don't have a Guild Emblem to delete.");
                    }
                }
                else self.Say("Oh... You are not the Guild Master. The Guild Emblem can only be created, deleted or modified by the #rGuild Master#k.");
            }
        }
    }
}

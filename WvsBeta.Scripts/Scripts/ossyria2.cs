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
            dynamic v1, nRet, nCountMax, isGuildMaster, isGuildMember, retPos, nRet2, nRet1;
            int nRequiredMeso;
            public void Run(INpcHost self, GameCharacter target)
            {
                var members = new GuildMember[]
                {
                    new GuildMember { ID = 4, Rank = GuildRank.Master }
                };
                var rankNames = new string[] { "pelle1", "pelle2", "pelle3", "pelle4", "pelle5", };
                var guild = new GuildData(1, "guildmaster", 100, rankNames, members);
                GuildPacket.SendGuildCreate(target, guild);
                return;
                /**/
                
                ossyria2.self = self;
                ossyria2.target = target;
                isGuildMaster = target.IsGuildMaster;
                isGuildMember = target.IsGuildMember;
                if (isGuildMember == 0)
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
                            nRet2 = target.IsCreateGuildPossible(1500000);
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
                    v1 = self.AskMenu("So how can I help?\r\n#b#L0#I want to expand my guild#l\r\n#L1#I want to disband my guild#l");

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
                            nRet = self.AskYesNo("Tem certeza de que deseja desfazer seu clã? Sério... lembre-se, se você desfizer o clã, ele será eliminado para sempre. Ah, e mais uma coisa. Se você quiser desfazer seu clã, vai precisar pagar 200.000 mesos pelo custo do serviço. Ainda quer fazer isto?");
                            if (nRet == 0) self.Say("Bem pensado. Eu não gostaria de desfazer meu clã que já está tão forte...");
                            else
                            {
                                bool fRet = target.RemoveGuild(-200000);
                                if (!fRet) self.Say("Ei, você não tem o dinheiro para o serviço... tem certeza de que tem dinheiro suficiente aí?");
                            }
                        }
                    }
                    else self.Say("Ei, você não é o Mestre do Clã!! Esta decisão só pode ser tomada pelo Mestre do Clã.");
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
                    v1 = self.AskMenu("Oi? Meu nome é #bLea#k. Estou responsável pelo #bEmblema do Clã#k.\r\n#b#L0#Eu gostaria de cadastrar o emblema de um clã.#l\r\n#L1#Eu gostaria de excluir o emblema de um clã.#l");
                    if (v1 == 0)
                    {
                        if (target.IsGuildMarkExist) self.Say("O Emblema do Clã já foi feito. Por favor, exclua o emblema do clã primeiro para refazê-lo.");
                        else
                        {
                            nRet = self.AskYesNo("Você precisa de #r5.000.000 de Mesos#k para criar um emblema de clã. Para explicar melhor, o emblema de um clã é um modelo único para cada clã. Ele irá aparecer ao lado do nome do clã no jogo.\nEntão, você vai criar um emblema de clã?");
                            if (nRet == 1)
                            {
                                if (target.Inventory.Mesos >= 5000000) target.SetGuildMark(-5000000);
                                else self.Say("Você não tem Mesos suficientes. Você precisa de #b5.000.000 de Mesos#.");
                            }
                            else self.Say("Oh... ok... O emblema deixaria o clã mais unido. Você precisa de mais tempo para preparar o emblema do clã? Por favor, volte quando quiser.");
                        }
                    }
                    else if (v1 == 1)
                    {
                        if (target.IsGuildMarkExist)
                        {
                            nRet = self.AskYesNo("Se você excluir o atual emblema do clã, poderá criar um novo. Você precisa de #r1.000.000 de Mesos#k para excluir um emblema de clã. Você gostaria de excluir?");
                            if (nRet == 1)
                            {
                                if (target.Inventory.Mesos >= 1000000) target.RemoveGuildMark(-1000000);
                                else self.Say("Você não tem Mesos suficientes para excluir o emblema do clã. Você precisa de #b1.000.000 de Mesos#k para excluir o emblema do clã.");
                            }
                            else self.Say("Por favor, volte quando quiser.");
                        }
                        else self.Say("Hã? Estranho... você não tem um emblema de clã para excluir.");
                    }
                }
                else self.Say("Oh... Você não é o mestre do clã. O Emblema do Clã pode ser feito, excluído ou modificado apenas pelo #rMestre do Clã#k.");
            }
        }
    }
}

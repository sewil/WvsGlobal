using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;
using static WvsBeta.Common.Strings;

namespace WvsBeta.Scripts.Scripts
{
    static class ludibrium0
    {
        readonly static Dictionary<int, Rock> EosRocks = new Dictionary<int, Rock>()
        {
            { NpcIds.FirstEosRock, new Rock(NpcIds.FirstEosRock, MapIds.EosTower100thFloor, 100, NpcIds.SecondEosRock) },
            { NpcIds.SecondEosRock, new Rock(NpcIds.SecondEosRock, MapIds.EosTower71stFloor, 71, NpcIds.FirstEosRock, NpcIds.ThirdEosRock) },
            { NpcIds.ThirdEosRock, new Rock(NpcIds.ThirdEosRock, MapIds.EosTower41stFloor, 41, NpcIds.SecondEosRock, NpcIds.FourthEosRock) },
            { NpcIds.FourthEosRock, new Rock(NpcIds.FourthEosRock, MapIds.EosTower1stFloor, 1, NpcIds.ThirdEosRock) },
        };
        struct Rock
        {
            public readonly int npcID;
            public readonly int mapID;
            public readonly int floorNumber;
            public readonly int[] destinationRocks;
            public Rock(int npcID, int mapID, int floorNumber, params int[] destinationRocks)
            {
                if (destinationRocks.Length == 0) throw new ArgumentException("Rock destinations can not be empty.");
                this.npcID = npcID;
                this.mapID = mapID;
                this.floorNumber = floorNumber;
                this.destinationRocks = destinationRocks;
            }

            public IEnumerable<Rock> GetDestinations()
            {
                return destinationRocks.Select(i => EosRocks[i]);
            }
        }
        static void RockGo(INpcHost self, GameCharacter target, Rock rock)
        {
            var inven = target.Inventory;

            if (inven.ItemCount(ItemEtcIds.EosRockScroll) >= 1)
            {
                string askText = $"You can use #b#t{ItemEtcIds.EosRockScroll}##k to activate #b#p{rock.npcID}##k. ";
                Rock destination;
                Rock[] destinations = rock.GetDestinations().ToArray();
                if (rock.destinationRocks.Length > 1)
                {
                    string[] menuOptions = destinations.Select(d => $"#p{d.npcID}# ({Ordinal(d.floorNumber)} floor)").ToArray();
                    int opt = self.AskMenu(askText + "Which of these rocks would you like to teleport to?", menuOptions);
                    destination = destinations[opt];
                }
                else
                {
                    destination = destinations[0];
                }

                askText += $"Will you teleport to the {Ordinal(destination.floorNumber)} floor of #b#p{destination.npcID}##k?";
                int ret1 = self.AskYesNo(askText);
                if (ret1 != 0)
                {
                    var ret2 = inven.Exchange(0, ItemEtcIds.EosRockScroll, -1);
                    if (ret2 == 0) self.Say($"You cannot activate #b#p{rock.npcID}##k without #b#t{ItemEtcIds.EosRockScroll}##k.");
                    else target.ChangeMap(destination.mapID, "go00");
                }
            }
            else
            {
                string npcs = string.Join(" or ", rock.destinationRocks.Select(npcID => $"#p{npcID}#"));
                self.Say($"There is a rock that teleports you to #b{npcs}#k, but it cannot be activated without the scroll.");
            }
        }

        // 1st eos rock : 2040024, 100th floor
        [Script("ludi014")]
        class ludi014 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                RockGo(self, target, EosRocks[NpcIds.FirstEosRock]);
            }
        }
        // 2nd eos rock : 2040025, 71st floor
        [Script("ludi015")]
        class ludi015 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                RockGo(self, target, EosRocks[NpcIds.SecondEosRock]);
            }
        }
        // 3rd eos rock : 2040026, 41st floor
        [Script("ludi016")]
        class ludi016 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                RockGo(self, target, EosRocks[NpcIds.ThirdEosRock]);
            }
        }
        // the 4th eos rock : 2040027, 1st floor
        [Script("ludi017")]
        class ludi017 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                RockGo(self, target, EosRocks[NpcIds.FourthEosRock]);
            }
        }
        // chang the assistant : 2040003
        [Script("ludi020")]
        class ludi020 : INpcScript
        {
            private void Leave(INpcHost self, GameCharacter target)
            {
                var nRet = self.AskYesNo("Are you sure you want to quit? Well, I can let you out, but you'll have to start over from the very beginning the next time you pay a visit here. Do you still want to leave this place?");
                if (nRet == 0) self.Say("That's the kind of attitude I've been looking for! If you've started something, you have to learn how to finish it! Now please search the plastic barrels and find #b10 #t4031092##k for me.");
                else target.ChangeMap(922000009, "");
            }
            public void Run(INpcHost self, GameCharacter target)
            {
                var field = self.Field;
                var qr = target.QuestRecord;
                var val = qr.GetState(3239);

                if (val == 1)
                {
                    string instructions = "Inside this room, you'll see a lot of plastic barrels lying around. Hit the barrels to make them fall and see if you can find the missing #b#t4031092##k.";
                    if (field.ID == 220020000)
                    {
                        var inven = target.Inventory;
                        self.Say($"Right then. {instructions}. You must collect #b10 #t4031092##k and then come back to me. There is a time limit to do this, so make sure to be quick!");
                        var hItem = inven.ItemCount(4031092);
                        if (hItem > 0) inven.Exchange(0, 4031092, -hItem);
                        var setParty = FieldSet.Instances["Ludi020"];
                        var res = setParty.Enter(target, 0);
                        if (res != 0) self.Say("Sorry, but it looks like someone else is inside inspecting the barrels. Only one person is allowed here at a time, so you'll have to wait your turn.");
                    }
                    else if (field.ID == 922000000)
                    {
                        var inven = target.Inventory;
                        var nItem = inven.ItemCount(4031092);
                        if (nItem == 0)
                        {
                            var v1 = self.AskMenu($"{instructions} You must collect #b10 #t4031092##k before the time limit expires and then come back to me. Time is ticking as we speak, so please hurry!", "I want to get out of here.");
                            if (v1 == 0) Leave(self, target);
                        }
                        else if (nItem >= 10)
                        {
                            self.Say("Well done! You managed to collect #b10 #t4031092##k. Well, since you've done us this huge favor, I'll reward you with something really nice. Before doing so, please check that you have space available in your use inventory.");
                            if (inven.SlotCount(2) > inven.HoldCount(2))
                            {
                                var rnum = Rand32.NextBetween(1, 4);
                                var nNewItemID = 0;
                                if (rnum == 1) nNewItemID = 2040704;
                                else if (rnum == 2) nNewItemID = 2040705;
                                else if (rnum == 3) nNewItemID = 2040707;
                                else if (rnum == 3) nNewItemID = 2040708;
                                var nNeed = inven.ItemCount(4031092);

                                var ret = inven.Exchange(0, 4031092, -nNeed, nNewItemID, 1);
                                if (ret == 0) self.Say("Are you sure you have #b10 #t4031092#s#k? If you're sure, please check that you have space available in your use inventory.");
                                else
                                {
                                    target.IncEXP(2700, 0);
                                    qr.SetComplete(3239);
                                    self.Say("What do you think? Do you like the #b#t" + nNewItemID + "##k I gave you? I don't even know how to thank you for helping me. Thanks to you, the Toy Factory is running smoothly again. I'm going to send you out now. Take care!");
                                    target.ChangeMap(220020000, "q000");
                                }
                            }
                            else self.Say("Hmm... your use inventory seems to be full at the moment. Therefore, you will not be able to receive my reward. Please free up some space in your inventory and come talk to me again.");
                        }
                        else
                        {
                            var v1 = self.AskMenu("Looks like you didn't collect the 10 missing #b#t4031092#s#k. Break the plastic barrels you see in this room and see if any of them contain the missing #b#t4031092##k. If you manage to get the 10 #b#t4031092#s#k before the time limit expires, then come and bring them to me. If you want to leave this place at any time, come talk to me.", "I want to leave this place.");
                            if (v1 == 0) Leave(self, target);
                        }
                    }
                }
                else if (val == 2) self.Say("Thanks to you, the Toy Factory is running smoothly again. I'm so glad you came to help us. We've taken good care of our extra parts, so don't worry. Well that's it! I need to get back to work!");
                else self.Say("Lately, mechanical parts have been disappearing from the Toy Factory, and that worries me a lot. I want to ask for help, but you don't seem strong enough to help us. Who should I ask to help us?");
            }
        }
        // kicking out of the toy factory quest
        [Script("ludi021")]
        class ludi021 : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var inven = target.Inventory;
                var qItem = inven.ItemCount(4031092);
                if (qItem > 0) inven.Exchange(0, 4031092, -qItem);
                target.ChangeMap(220020000, "q000");
            }
        }
        // olson the toy soldier : 2040002
        [Script("ludi023")]
        class ludi023 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var qr = target.QuestRecord;
                var val = qr.GetState(3230);
                var inven = target.Inventory;

                if (val == 1)
                {
                    var nItem = inven.ItemCount(4031145);
                    var nRet1 = self.AskYesNo("Hmm... I've heard a lot about you from #b#p2040001##k. You got him some #b#t4031093##k so he could escape boredom at work. Well... this is it. There's a very, very dangerous monster inside. I want to ask you to help us by finding the monster. Would you like to help me?");
                    if (nRet1 == 0) self.Say("I understand. It's totally understandable considering the fact that you'll be facing a very dangerous monster. If you ever change your mind, come and talk to me again. I'm sure your assistance would be very helpful.");
                    else
                    {
                        self.Say("Thank you very much. In fact, #b#p2040001##k asked you to get #b#t4031093#s#k as a way to test your skills and see if you can handle it. So, don't think of this as a request coming out of nowhere. I think someone like you can tackle this challenge.");
                        var nRet2 = self.AskYesNo("A while ago, a monster from another dimension came here due to an interdimensional rift, and stole the clock's pendulum. It hid in that room and disguised itself as a dollhouse. To me they all look the same, so there's no way to figure out which is which. Would you help us find it?");
                        if (nRet2 == 0) self.Say("I understand. Come talk to me when you're ready to accept this task. I advise you not to take too long, as the monster can disguise itself as something else entirely. We have to act like we don't know anything.");
                        else
                        {
                            self.Say("Great! I'll take you to a room, where you'll find dollhouses scattered around. One of them will look a little different from the others. Your job is to locate and break down its door. If you can find it, you'll find the #b#t4031145##k. If you break one of the real dollhouses, you'll be sent back here, so pay close attention.");
                            self.Say("You'll also find monsters there, and they've gotten too powerful due to the monster from the other dimension, so powerful that you won't be able to eliminate them. Please find the #b#t4031145##k before the time runs out and notify #b#p2040028##k, who should be inside. Let's get started soon!");
                            if (nItem > 0) inven.Exchange(0, 4031145, -nItem);
                            var setParty = FieldSet.Instances["Ludi023"];
                            var res = setParty.Enter(target, 0);
                            if (res != 0) self.Say("Someone else must be inside looking for the dollhouse. Unfortunately, I can only let one person in at a time, so please wait your turn.");
                        }
                    }
                }
                else if (val == 2) self.Say("Thanks to " + target.SCharacterName + ", we recovered the #b#t4031145##k and destroyed the monster from the other dimension. Good thing we haven't found one like that since. I don't even know how to thank you for helping us. Hope you enjoy your stay here in Ludibrium!");
                else self.Say("We are the toy soldiers who guard this room to prevent anyone else from entering. I can't tell you why. Now, if you'll excuse me, I have work to do.");
            }
        }
        // mark the toy soldier : 2040028
        [Script("ludi024")]
        class ludi024 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var qr = target.QuestRecord;
                var val = qr.GetState(3230);
                var inven = target.Inventory;

                if (val == 1)
                {
                    if (inven.ItemCount(4031145) >= 1)
                    {
                        self.Say("Oh wow, you did locate the different-looking dollhouse and found the #b#t4031145##k! That was just incredible!! With this, the Ludibrium Clocktower will be running again! Thank you for your work and here's a little reward for your effort. Before that, though, please check your use inventory and see if it's full or not.");
                        var ret = inven.Exchange(0, 4031145, -1, 2000010, 100);
                        if (ret == 0) self.Say("Are you sure you have 1 #b#t4031145##k? If you're sure, please check your use inventory, it might be full!!");
                        else
                        {
                            target.IncEXP(2400, 0);
                            qr.SetComplete(3230);
                            self.Say("What do you think? Do you like the #b100 #t2000010#s#k that I gave you? Thank you so much for helping us out. The clocktower will be running again thanks to your heroic effort, and the monsters from the other dimension seem to have disappeared, too. I'll let you out now. I'll see you around!");
                            target.ChangeMap(221024400, "q000");
                        }
                    }
                    else
                    {
                        var v1 = self.AskMenu("Hello! I am #b#p2040028##k, and I'm in charge of protecting this room. Here, you'll find a bunch of dollhouses and you might find the one that looks a little different from the others. Your job is to find it, break down its door and find #b#t4031145##k, which is an integral part of the Ludibrium Clocktower. You will have a time limit to do this, and if you break the wrong dollhouse, you will have to come back here, so please be careful.", "I want to get out of here.");
                        if (v1 == 0)
                        {
                            var nRet = self.AskYesNo("Are you sure you want to quit now? Alright then... but remember that the next time you visit this place, the dollhouses will switch places and you'll have to look at each one very carefully again. What do you think? Do you still want to leave this place?");
                            if (nRet == 0) self.Say("I knew you'd stay! It's important to finish what you started! Now, please go find the dollhouse that is different from the others, break it and bring me the #b#t4031145##k!");
                            else target.ChangeMap(221024400, "q000");
                        }
                    }
                }
                else
                {
                    self.Say("What's this... we have prohibited people from entering this room because a monster from another dimension is hiding here. I don't know how you got here, but I'll have to ask you to leave immediately, because inside this room there is great danger.");
                    target.ChangeMap(221024400, "");
                }
            }
        }
        // wisp : 2040030
        [Script("ludi026")]
        class ludi026 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var v1 = self.AskMenu("Olá, você aí! Eu sou #b#p1032102##k, o discípulo principal da Ilha Victoria. #p1032102# me chamou para verificar se os bichinhos estão sendo bem tratados aqui em Ludibrium. O que posso fazer por você?\r\n#b#L0# Meu bichinho voltou a ser um boneco. Por favor, me ajude a fazê-lo se mover de novo!#l\r\n#L1#Me fale mais sobre Bichinhos.#l\r\n#L2#Como eu cuido dos Bichinhos?#l\r\n#L3#Os Bichinhos também morrem?#l\r\n#L4#Quais são os comandos dos Gatinhos Marrom e Preto?#l\r\n#L5#Quais são os comandos do Cachorrinho Marrom?#l\r\n#L6#Quais são os comandos dos Coelhinhos Rosa e Branco?#l\r\n#L7#Quais são os comandos do Mini Kargo?#l\r\n#L8#Quais são os comandos de Rudolph e Dasher?#l\r\n#L9#Quais são os comandos do Porquinho Preto?#l\r\n#L10#Quais são os comandos do Panda?#l\r\n#L11#Quais são os comandos do Husky?#l\r\n#L12#Quais são os comandos do Dino Boy e Dino Girl?#l\r\n#L13#Quais são os comandos do Macaco?#l\r\n#L14#Quais são os comandos do Peru?#l\r\n#L15#Quais são os comandos do Tigre Branco?#l\r\n#L16#Quais são os comandos do Pingüim?#l\r\n#L17#Quais são os comandos do Porquinho Dourado?#l\r\n#L18#Quais são os comandos do Robô?#l\r\n#L19#Quais são os comandos do Mini Yeti?#l\r\n#L20#Quais são os comandos do Balrog Jr.?#l\r\n#L21#Quais são os comandos do Bebê Dragão?#l\r\n#L22#Quais sãos os comandos dos Dragões Verde/Vermelho/Azul?#l\r\n#L23#Quais são os comandos do Dragão Negro?#l\r\n#L24#Quais são os comandos do Anjo da Morte Jr.?#l\r\n#L25#Quais são os comandos do porco-espinho?#l\r\n#L26#Quais são os comandos do Boneco de Neve?#l#k");
                if (v1 == 0)
                {
                    var qr = target.QuestRecord;
                    var val = qr.GetState(3240);
                    var inven = target.Inventory;

                    bool havePetDoll = self.HavePetDoll();

                    if (havePetDoll)
                    {
                        if (val == 0 || val == 2)
                        {
                            var nRet1 = self.AskYesNo("Parece que você já conheceu #p1012005#. #p1012005# é alguém que estudava a mágia da vida com meu mestre, #p1032102#. Eu ouvi dizer que ele usou uma magia de vida incompleta em um boneco para criar um animal vivo... o boneco que você tem é o mesmo boneco que #p1012005# criou, chamado de #bBichinho#k?");
                            if (nRet1 == 0) self.Say("Mas com certeza parece como se fosse um trabalho de #p1012005#. Ah, bem! Não importa. Eu vi #p1012005# nos últimos anos e tenho certeza de que ele não pode criar magia da vida em bonecos. Bem, é isso...");
                            else
                            {
                                self.Say("Entendo. Esse boneco costumava ser um animal vivo... mas o mesmo item que #p1012005# usou para dar vida ao bichinho, #b#t5180000##k, se esgotou e o bichinho voltou a ser apenas um boneco... ele não se move mais porque agora é um boneco de novo... humm... a vida não é algo que você possa criar com mágica...?");
                                self.Say("Você quer fazer o boneco voltar a ser o que era? Um ser com vida? Você quer que o seu bichinho volte a te acompanhar, andando por aí, te fazendo companhia, certo? Claro! Isso é totalmente possível! Já que eu sou o discípulo da Fada que estudou mágica com #p1012005#... Talvez eu possa fazer que ele viva novamente...");
                                var nRet2 = self.AskYesNo("Se eu puder conseguir #b#t5180000##k e #b#t4031034##k, talvez eu consiga fazer que seu boneco volte à vida. O que você acha? Você quer ir buscar estes itens? Traga os itens para mim e eu tentarei fazer que seu boneco volte à vida...");
                                if (nRet2 == 0) self.Say("Você quer deixar o boneco do jeito que está? É um boneco agora, mas... será difícil apagar suas memórias também. Se você mudar de idéia, é só vir falar comigo...");
                                else
                                {
                                    qr.Set(3240, "");
                                    self.Say("Muito bem. Eu vou dizer isso novamente, mas o que preciso é #b#t5180000##k e #b#t4031034##k. Me traga estes dois itens e eu posso fazer seu boneco voltar à vida. #b#t4031034##k é difícil de achar... em algum lugar de Ludibrium você irá encontrar a casa de #b#p1012005##k. A casa está vazia no momento, mas dê uma procurada e talvez você o encontre.");
                                }
                            }
                        }
                        else if (val == 1)
                        {
                            if (inven.ItemCount(5180000) > 0 && inven.ItemCount(4031034) > 0)
                            {
                                var nRet3 = self.AskYesNo("Você me trouxe #b#t5180000##k e #b#t4031034##k... com esses itens, eu posso trazer o boneco de volta à vida, usando o poder mágico de meu mestre. O que você acha? Você deseja usar os itens e reanimar seu bichinho?");
                                if (nRet3 == 0) self.Say("Eu entendo, você não tem certeza sobre isso, não é? Você não acha que deixar o bichinho como um boneco não é errado? Por favor, volte aqui se você mudar de idéia...");
                                else
                                {
                                    var petCode = self.AskPetDoll("Então, qual bichinho você deseja restaurar? Por favor, escolha o bichinho que você deseja reviver...");
                                    bool okPet = inven.SetPetLife(petCode, (5180000, -1), (4031034, -1));
                                    if (!okPet) self.Say("Alguma coisa está errada... você tem certeza de que possui #b#t5180000##k e #b#t4031034##k? Sem esses dois itens eu não posso fazer seu boneco voltar a ser um bichinho.");
                                    else
                                    {
                                        qr.SetComplete(3240);
                                        self.Say("Seu boneco agora voltou a ser um bichinho. Entretanto, minha mágica não é perfeita, por isso, eu não posso te prometer que ele vai viver para sempre ... cuide do seu bichinho antes que o #t5180000# se esgote. Bem, acho que é isso... adeus!");
                                    }
                                }
                            }
                            else self.Say("Eu acho que você não conseguiu #b#t5180000##k e #b#t4031034##k ainda. Em algum lugar de Ludibrium você vai encontrar a casa de #b#p1012005##k. A casa está vazia no momento, mas dê uma procurada e talvez você o encontre. Viva!");
                        }
                    }
                    else
                    {
                        if (val == 2) self.Say("Olá! Como está indo o seu bichinho reanimado? Fico contente em saber que você está feliz com seu bichinho. Bem, agora tenho que voltar aos estudos que meu mestre me passou, por isso...");
                        else self.Say("Eu sou #p2040030#, continuando com os estudos que meu mestre #p1032102# me deu. Parece que há muitos bichinhos até mesmo aqui em Ludibrium. Eu preciso voltar aos meus estudos, então, se puder me dar licença...");
                    }
                }
                else if (v1 == 1)
                {
                    self.Say("Humm... você deve ter muitas perguntas sobre bichinhos. Há muito tempo, uma pessoa com o nome de #b#p1012005##k jogou #t5180000# nele, e lançou um feitiço nele para criar um animal mágico. Eu sei que parece inacreditável, mas é um boneco que se tornou um ser vivo. Eles entendem e acompanham as pessoas.");
                    self.Say("Mas #t5180000# só aparece aos pouqinhos na base da Árvore do Mundo, por isso, esses bebês não podem viver para sempre... Eu sei, é uma coisa chata... mas, mesmo que ele se torne um boneco de novo, sempre poderão ser reanimados e voltarem a viver. Seja bom com ele enquanto ele está vivo.");
                    self.Say("Ah, sim! Eles irão reagir quando você falar comandos especiais. Você pode brigar com eles, amá-los... tudo depende de como você toma conta deles. Eles têm medo de abandonar seus mestres, por isso, seja bom com eles, tenha amor por eles. Eles podem se sentir tristes e solitários muito rápido...");
                }
                else if (v1 == 2)
                {
                    self.Say("Dependendo do comando que você der, os bichinhos podem gostar, odiar ou mostrar outros tipos de reação a eles. Se você der um comando ao bichinho e ele obedecer sem reclamar, o seu laço de amizade aumenta. Clique duas vezes no bichinho e poderá visualizar os níveis de intimidade, energia e etc...");
                    self.Say("Fale com o bichinho, dê atenção a ele, e seu nível de intimidade irá aumentar, e seu nível geral também irá aumentar. À medida que o seu nível de intimidade aumenta, o nível geral do bichinho também irá aumentar. À medida que o nível geral aumenta, algum dia o bichinho pode até mesmo falar um pouco parecido como uma pessoa, então tente criar seu bichinho da melhor forma possível. Claro que não será fácil fazer isso...");
                    self.Say("Eles são bonecos com vida, e, por isso, eles também sentem fome, por exemplo. #bEnergia Completa#k Mostra o nível de fome do bichinho, sendo que 100 é o máximo, e, quanto mais baixo esse nível ficar, mais fome o seu bichinho está sentindo. Depois de um tempo, ele não vai seguir seus comandos e ficará irritado, então é bom tomar cuidado.");
                    self.Say("Isso aí! Bichinhos não podem comer comida humana normal. Um urso de pelúcia chamado #b#p2041014##k, que mora em Ludibrium, vende #b#t2120000##k então, se você precisar de comida para seu bichinho, vá falar com #b#p2041014##k. É uma boa idéia comprar comida extra e alimentar seu bichinho antes que ele fique com muita fome.");
                    self.Say("Oh, e se você não der comida ao bichinho por muito tempo, ele volta para casa sozinho. Você pode tirá-lo de casa e lhe dar comida, mas isso não é muito bom para a saúde do bichinho, então tente alimentá-lo regularmente para evitar esses problemas, certo? Acho que é só isso.");
                }
                else if (v1 == 3)
                {
                    self.Say("Morrendo... bem, veja, eles não estão VIVOS na verdade, então não sei se morrer seja a palavra certa a ser usada. Eles são bonecos com o poder mágico de #p1012005#k e #t5180000# que os torna um objeto com vida. Claro que, enquanto ele está vivo, ele se parece com um animal vivo...");
                    self.Say("Após algum tempo... Sim! É isso que você está pensando! Eles param de se movimentar. Eles voltam a ser apenas um boneco, quando o efeito da mágica acaba e o #t5180000# se esgota. Mas isso não significa que ele fica imóvel para sempre, pois, quando você derramar #t5180000# sobre ele, ele volta à vida.");
                    self.Say("Mesmo sabendo disso, é triste vê-los parar de se movimentar. Seja legal com eles enquanto eles estão vivos e se movendo. E não se esqueça de lhes dar comida também. Não é legal saber que há uma coisinha viva que te segue e só obedece a você?");
                }
                else if (v1 == 4)
                {
                    self.Say("Estes são os comandos dos #rGatinhos Marrom e Preto#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garota má, garoto mau#k(nível 1 ~ 30)\r\n#bestúpido, eu te odeio, bobão#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bCocô#k(nível 1 ~ 30)\r\n#bconverse, diga algo, bata um papo#k(nível 10 ~ 30)\r\n#bbonitinho#k(nível 10 ~ 30)\r\n#bacorde, fique de pé, levante-se#k(nível 20 ~ 30)");
                }
                else if (v1 == 5)
                {
                    self.Say("Estes são os comandos do #rCachorrinho Marrom#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garota má, garoto mau#k(nível 1 ~ 30)\r\n#bestúpido, eu te odeio, cachorro mau, bobão#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bxixi#k(nível 1 ~ 30)\r\n#bconverse, diga algo, bata um papo#k(nível 10 ~ 30)\r\n#babaixe#k(nível 10 ~ 30)\r\n#bacorde, fique de pé, levante-se#k(nível 20 ~ 30)");
                }
                else if (v1 == 6)
                {
                    self.Say("Estes são os comandos dos #rCoelhinhos Rosa e Branco#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garota má, garoto mau#k(nível 1 ~ 30)\r\n#bacorde, fique de pé, levante-se#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bconverse, diga algo, bata um papo#k(nível 10 ~ 30)\r\n#babraçar#k(nível 10 ~ 30)\r\n#bdurma, fique com sono, vá para a cama#k(nível 20 ~ 30)");
                }
                else if (v1 == 7)
                {
                    self.Say("Estes são os comandos do #rMini Kargo#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garota má, garoto mau#k(nível 1 ~ 30)\r\n#bacorde, fique de pé, levante-se#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bxixi#k(nível 1 ~ 30)\r\n#bconverse, diga algo, bata um papo#k(nível 10 ~ 30)\r\n#bsuplique, mostre carisma#k(nível 10 ~ 30)\r\n#babaixe#k(nível 10 ~ 30)\r\n#bbom garoto, boa garota#k(nível 20 ~ 30)");
                }
                else if (v1 == 8)
                {
                    self.Say("Estes são os comandos de #rRudolph e Dasher#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garota má, garoto mau#k(nível 1 ~ 30)\r\n#blevantar, ficar de pé#k(nível 1 ~ 30)\r\n#bestúpido, eu te odeio, bobão#k(nível 1 ~ 30)\r\n#bfeliz natal, feliz natal#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bconverse, diga algo, bata um papo#k(nível 11 ~ 30)\r\n#bsolitário, sozinho#k(nível 11 ~ 30)\r\n#bbonitinho#k(nível 11 ~ 30)\r\n#bbusque, vá#k(level 21 ~ 30)");
                }
                else if (v1 == 9)
                {
                    self.Say("Estes são os comandos do #rPorquinho Preto#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garota má, garoto mau#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bmão#k(nível 1 ~ 30)\r\n#bestúpido, eu te odeio, bobão#k(nível 1 ~ 30)\r\n#bconverse, bata um papo, diga algo#k(nível 10 ~ 30)\r\n#bsorria#k(nível 10 ~ 30)\r\n#bsuplique, mostre carisma#k(nível 20 ~ 30)");
                }
                else if (v1 == 10)
                {
                    self.Say("Estes são os comandos do #rPanda#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bdescansar, relaxar#k(nível 1 ~ 30)\r\n#bmau, não, garota má, garoto mau#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bacorde, fique de pé, levante-se#k(nível 1 ~ 30)\r\n#bconverse, bata um papo, diga algo#k(nível 10 ~ 30)\r\n#bvamos brincar#k(nível 10 ~ 30)\r\n#bmé, blé#k(nível 10 ~ 30)\r\n#bdormir#k(nível 20 ~ 30)");
                }
                else if (v1 == 11)
                {
                    self.Say("Estes são os comandos do #rHusky#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garota má, garoto mau#k(nível 1 ~ 30)\r\n#bestúpido, eu te odeio, cachorro mau, bobão#k(nível 1 ~ 30)\r\n#bmão#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#babaixe#k(nível 10 ~ 30)\r\n#bconverse, bata um papo, diga algo#k(nível 10 ~ 30)\r\n#bacorde, fique de pé, levante-se#k(nível 20 ~ 30)");
                }
                else if (v1 == 12)
                {
                    self.Say("Estes são os comandos para #rDino Boy, Dino Girl#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garota má, garoto mau#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bsorria, gargalhe#k(nível 1 ~ 30)\r\n#bestúpido, eu te odeio, bobão#k(nível 1 ~ 30)\r\n#bconverse, bata um papo, diga algo#k(nível 10 ~ 30)\r\n#bbonitinho#k(nível 10 ~ 30)\r\n#bdurma, tire uma soneca, fique com sono#k(nível 20 ~ 30)");
                }
                else if (v1 == 13)
                {
                    self.Say("Estes são os comandos do #rMacaco#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bdescansar#k(nível 1 ~ 30)\r\n#bmau, não, garoto mau, garota má#k(nível 1 ~ 30)\r\n#bxixi#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#blevantar, ficar de pé#k(nível 1 ~ 30)\r\n#bconverse, bata um papo, diga algo#k(nível 10 ~ 30)\r\n#bbrincar#k(nível 10 ~ 30)\r\n#beu quero#k(nível 10 ~ 30)\r\n#bdurma, vá para a cama, fique com sono#k(nível 20 ~ 30)");
                }
                else if (v1 == 14)
                {
                    self.Say("Estes são os comandos do #rPeru#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bnão, garoto mal-educado, malvado#k(nível 1 ~ 30)\r\n#bestúpido#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#blevantar, ficar de pé#k(nível 1 ~ 30)\r\n#bconverse, bata um papo, glu#k(nível 10 ~ 30)\r\n#bsim, bom garoto#k(nível 10 ~ 30)\r\n#bfique com sono, sonequinha, dormir muito#k(nível 20 ~ 30)\r\n#bolho de pássaro, ação de graças, voar, pássaro frito, eu estou com fome#k(nível 30)");
                }
                else if (v1 == 15)
                {
                    self.Say("Estes são os comandos do #rTigre Branco#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garoto mau, garota má#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bdescansar, relaxar#k(nível 1 ~ 30)\r\n#bestúpido, eu te odeio, bobão#k(nível 1 ~ 30)\r\n#bconverse, bata um papo, diga algo#k(nível 10 ~ 30)\r\n#bparecer triste, olhar triste#k(nível 10 ~ 30)\r\n#besperar#k(nível 20 ~ 30)");
                }
                else if (v1 == 16)
                {
                    self.Say("Estes são os comandos do #rPingüim#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garoto mau, garota má#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bacorde, fique de pé, levante-se#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bconverse, bata um papo, diga algo#k(nível 10 ~ 30)\r\n#babraçar, me abrace#k(nível 10 ~ 30)\r\n#basa, mão#k(nível 10 ~ 30)\r\n#bdormir#k(nível 20 ~ 30)\r\n#bbeijar, beijar bochecha, bicota#k(nível 20 ~ 30)\r\n#bvoar#k(nível 20 ~ 30)\r\n#bbonito, adorável#k(nível 20 ~ 30)");
                }
                else if (v1 == 17)
                {
                    self.Say("Estes são os comandos do #rPorquinho Dourado#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garoto mau, garota má#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bconverse, bata um papo, diga algo#k(nível 11 ~ 30)\r\n#bme ame, me abraçe#k(nível 11 ~ 30)\r\n#bdurma, fique com sono, vá para a cama#k(nível 21 ~ 30)\r\n#bignorar / impressionado / saia daqui#k(nível 21 ~ 30)\r\n#brole, mostre o dinheiro#k(nível 21 ~ 30)");
                }
                else if (v1 == 18)
                {
                    self.Say("Estes são os comandos do #rRobô#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bacorde, fique de pé, levante-se#k(nível 1 ~ 30)\r\n#bestúpido, eu te odeio, bobão#k(nível 1 ~ 30)\r\n#bmau, não, garota má, garoto mau#k(nível 1 ~ 30)\r\n#batacar, avançar#k(nível 1 ~ 30)\r\n#bTe amo#k(nível 1 ~ 30)\r\n#bbom, suplique, mostre carisma#k(nível 11 ~ 30)\r\n#bfale, converse, bata um papo, diga algo#k(nível 11 ~ 30)\r\n#bdisfarce, mude, transforme#k(nível 11 ~ 30)");
                }
                else if (v1 == 19)
                {
                    self.Say("Estes são os comandos do #rMini Yeti#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bmau, não, garoto mau, garota má#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bdançar, requebrar, rebolar#k(nível 1 ~ 30)\r\n#bbonito, bonitinho, lindo, adoráveç#k(nível 1 ~ 30)\r\n#bTe amo, gosto de você, meu amor#k(nível 1 ~ 30)\r\n#bconverse, bata um papo, diga algo#k(nível 11 ~ 30)\r\n#bdurma, soneca, fique com sono, vá para a cama#k(nível 11 ~ 30)");
                }
                else if (v1 == 20)
                {
                    self.Say("Estes são os comandos do #rBalrog Jr.#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bdeitar#k(nível 1 ~ 30)\r\n#bnão|mau|garota má|garoto mau#k(nível 1 ~ 30)\r\n#bTe amo|meu amor|gosto de você#k(nível 1 ~ 30)\r\n#bbonito|bonitinho|lindo|adorável#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bsorria|ria|gargalhe#k(nível 1 ~ 30)\r\n#beu quero#k(nível 11 ~ 30)\r\n#bbom|suplique|mostre carisma#k(nível 11 ~ 30)\r\n#bfale|converse|bata um papo|diga algo#k(nível 11 ~ 30)\r\n#bdurma|soneca|fique com sono#k(nível 11 ~ 30)\r\n#bsolte pum#k(nível 21 ~ 30)");
                }
                else if (v1 == 21)
                {
                    self.Say("Estes são os comandos do #rBebê-dragão#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bnão|mau|garota má|garoto mau#k(nível 1 ~ 30)\r\n#bTe amo|amo você#k(nível 1 ~ 30)\r\n#bcocô#k(nível 1 ~ 30)\r\n#bestúpido|eu te odeio|bobão#k(nível 1 ~ 30)\r\n#bbonitinho#k(nível 11 ~ 30)\r\n#bconverse|bata um papo|diga algo#k(nível 11 ~ 30)\r\n#bdurma|fique com sono|vá para a cama#k(nível 11 ~ 30)");
                }
                else if (v1 == 22)
                {
                    self.Say("Estes são os comandos dos #rDragões Verde/Vermelho/Azul#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 15 ~ 30)\r\n#bnão|mau|garota má|garoto mau#k(nível 15 ~ 30)\r\n#bTe amo|amo você#k(nível 15 ~ 30)\r\n#bcocô#k(nível 15 ~ 30)\r\n#bestúpido|eu te odeio|bobão#k(nível 15 ~ 30)\r\n#bconverse|bata um papo|diga algo#k(nível 15 ~ 30)\r\n#bdurma|fique com sono|vá para a cama#k(nível 15 ~ 30)\r\n#bmudar#k(nível 21 ~ 30)");
                }
                else if (v1 == 23)
                {
                    self.Say("Estes são os comandos do #rDragão Negro#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 15 ~ 30)\r\n#bnão|mau|garota má|garoto mau#k(nível 15 ~ 30)\r\n#bTe amo|amo você#k(nível 15 ~ 30)\r\n#bcocô#k(nível 15 ~ 30)\r\n#bestúpido|eu te odeio|bobão#k(nível 15 ~ 30)\r\n#bconverse|bata um papo|diga algo#k(nível 15 ~ 30)\r\n#bdurma|fique com sono|vá para a cama#k(nível 15 ~ 30)\r\n#bbonitinho, mudar#k(nível 21 ~ 30)");
                }
                else if (v1 == 24)
                {
                    self.Say("Estes são os comandos para o #rAnjo da Morte Jr.#k O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bnão|mau|garota má|garoto mau#k(nível 1 ~ 30)\r\n#bfingir de morto, cocô#k(nível 1 ~ 30)\r\n#bconverse|bata um papo|diga algo#k(nível 1 ~ 30)\r\n#bTe amo, abraçar#k(nível 1 ~ 30)\r\n#bcheire meu pé, dance rock, bu#k(nível 1 ~ 30)\r\n#btravessuras ou gostosuras#k(nível 1 ~ 30)\r\n#bmonstromash#k(nível 1 ~ 30)");
                }
                else if (v1 == 25)
                {
                    self.Say("Estes são os comandos do #rPorco-espinho#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bnão|mau|garota má|garoto mau#k(nível 1 ~ 30)\r\n#bTe amo abraçar bom garoto#k(nível 1 ~ 30)\r\n#bconverse|bata um papo|diga algo#k(nível 1 ~ 30)\r\n#balmofada durma tricote cocô#k(nível 1 ~ 30)\r\n#bpenteie praia#k(nível 10 ~ 30)\r\n#bárvore ninja#k(nível 20 ~ 30)\r\n#bdardo#k(nível 20 ~ 30)");
                }
                else if (v1 == 26)
                {
                    self.Say("Estes são os comandos do #rBoneco de neve#k. O nível descrito próximo ao comando mostra o nível necessário ao bichinho para que ele responda.\r\n#bsenta#k(nível 1 ~ 30)\r\n#bestúpido,eu te odeio,bobão#k(nível 1 ~ 30)\r\n#bte amo,meu amor,eu gosto de você#k(nível 1 ~ 30)\r\n#bfeliz natal#k(nível 1 ~ 30)\r\n#bbonitinho,adorável,bonito,lindo#k(nível 1 ~ 30)\r\n#bpenteie praia/mau,não,garota má,garoto mau#k(nível 1 ~ 30)\r\n#bconverse,bata um papo,diga algo/durma,fique com sono,vá para a cama#k(nível 10 ~ 30)\r\n#bmudar#k(nível 20 ~ 30)");
                }
            }
        }
        // ¹®¼­¹¶Ä¡ : 2040031
        [Script("ludi027")]
        class ludi027 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var qr = target.QuestRecord;
                var val = qr.GetState(3240);
                var inven = target.Inventory;

                if (val == 1)
                {
                    if (inven.ItemCount(4031034) >= 1)
                    {
                        self.Say("You already have #b#t4031034##k.");
                        return;
                    }

                    var nRet = self.AskYesNo("Among so many scrolls left by #b#p1012005##k, I see one that emits a bright light. Should I take it?");
                    if (nRet != 0)
                    {
                        var ret = inven.Exchange(0, 4031034, 1);
                        if (ret == 0) self.Say("I don't have any free space in my inventory. I'd better clear up some space so I can carry this scroll with me.");
                    }
                }
            }
        }
        // weaver the trainer : 2040032
        [Script("ludi028")]
        class ludi028 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var inven = target.Inventory;
                if (inven.ItemCount(4031128) > 0) self.Say("Pegue essa carta, pule sobre os obstáculos com seu bichinho e leve a carta para meu irmão #p2040033#. Leve a carta para ele e algo bom vai acontecer com seu bichinho.");
                else
                {
                    var nRet = self.AskYesNo("Essa é a estrada onde você pode levar seu bichinho para dar uma volta. Você pode andar nela ou pode treinar seu bichinho para que ele pule os obstáculos. Se você ainda não é muito intimo de seu bichinho, isso pode ser um prolema e ele não vai seguir suas ordens tão bem... então... o que você acha? Quer treinar seu bichinho?");
                    if (nRet == 0) self.Say("Humm... muito #Gocupado:ocupada# agora? Se você quiser fazer isso depois, é só voltar e falar comigo.");
                    else
                    {
                        var ret = inven.Exchange(0, 4031128, 1);
                        if (ret == 0) self.Say("Seu inventário de etc. está cheio! Eu não posso te dar a carta a nao ser que exista espaço livre em seu inventário. Libere espaço no seu inventário e venha falar comigo novamente.");
                        else self.Say("Ok, aqui está a carta. Se eu te mandasse lá sem a carta, ele não saberia quem é você, por isso, passe pelos obstáculos com seu bichinho, vá até o topo e depois fale com #p2040033# e entregue a carta. Não será difícil se você prestar atenção no seu bichinho enquanto estiver ultrapassando os obstáculos. Boa sorte!");
                    }
                }
            }
        }
        // nerr the trainer : 2040033
        [Script("ludi029")]
        class ludi029 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var inven = target.Inventory;
                if (inven.ItemCount(4031128) > 0)
                {
                    self.Say("Ei! Essa carta é do meu irmão! Provavelmente me dando uma bronca por achar que eu não estou trabalhando com vontade... O quê? Ahhh... você seguiu os conselhos de meu irmão e treinou o seu bichinho até chegar aqui em cima? Lega!! Já que você se esforçou tanto para chegar aqui, eu vou vou aumentar seu nível de intimidade com seu bichinho.");
                    PetItem pet = target.GetSpawnedPet();
                    if (pet == null) self.Say("Humm... você conseguiu chegar aqui com seu bichinho!? Esses obstáculos são para os bichinhos. O que você está fazendo aqui sem ele? Saia daqui!");
                    else
                    {
                        var ret = inven.Exchange(0, 4031128, -1);
                        if (ret == 0) self.Say("Ei! Você tem certeza de que está com a carta do meu irmão? Verifique no seu inventário de etc. para ver se a carta está lá ou não!");
                        else
                        {
                            target.IncPetTame(4);
                            self.Say("O que você acha? Você não acha que ficou muito íntimo do seu bichinho? Se você estiver com tempo, treine seu bichinho novamente nesse curso de obstáculos... com a permissão de meu irmão, é claro!");
                        }
                    }
                }
                else self.Say("Meu irmão me disse para tomar conta do curso de obstáculos, mas... já que estou tão longe dele, não consigo evitar de ficar brincando, hehe... já que ele não pode me controlar, eu aproveito para relaxar um pouco.");
            }
        }
        [Script("library")]
        class library : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var qr = target.QuestRecord;
                var count = 0;
                var bookname = "";

                var book1 = qr.GetState(3615); //ÈïºÎ¿Í ³îºÎ µ¿È­Ã¥ È¸¼ö
                if (book1 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031235##t4031235##k\r\n";
                }

                var book2 = qr.GetState(3616); //ÄáÁã ÆÏÁã
                if (book2 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031236##t4031236##k\r\n";
                }

                var book3 = qr.GetState(3617); //ÀÇÁÁÀº ÇüÁ¦
                if (book3 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031237##t4031237##k\r\n";
                }

                var book4 = qr.GetState(3618); //µµ±úºñ ÀÌ¾ß±â
                if (book4 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031238##t4031238##k\r\n";
                }

                var book5 = qr.GetState(3630); //ÇØ¿Í ´ÞÀÌ µÈ ¿À´©ÀÌ
                if (book5 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031270##t4031270##k\r\n";
                }

                var book6 = qr.GetState(3633); //È¿³à ½ÉÃ»
                if (book6 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031280##t4031280##k\r\n";
                }

                var book7 = qr.GetState(3639); //±Ýµµ³¢ Àºµµ³¢
                if (book7 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031298##t4031298##k\r\n";
                }

                if (count != 0)
                {
                    self.Say("Vamos ver... #b" + target.SCharacterName + "#k Eu devolvi um total de #b" + count + "#k livros.\r\nA lista de livros devolvidos é:\r\n\r\n" + bookname);
                    self.Say("A biblioteca está se ajeitando agora, principalmente graças a você, #b" + target.SCharacterName + "#k. Se a história se misturar mais uma vez, então, eu poderei contar com você para repará-la novamente.");
                }
                else
                {
                    self.Say("#b" + target.SCharacterName + "#k ainda não devolveu nenhum livro de história...");
                }
            }
        }
    }
    #region Reactors
    [Script("ludiquest0")]
    public class LudiQuest0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("go221024400")]
    public class go221024400 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Owner.ChangeMap(221024400, "q000");
            target.Owner.Message("You will be expelled from this map by an unknown force.");
        }
    }
    [Script("ludiquest1")]
    public class LudiQuest1 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    #endregion
}

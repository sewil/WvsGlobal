using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Game;
using WvsBeta.Game.Handlers.Contimove;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public struct StationData
    {
        public (string,string) destination;
        public int ticket;
        public bool sail;
        public (int, int) fee;
        public CONTIMOVE conti;
        public StationData(string name, (string,string) destination, int ticket, bool sail, (int,int) fee)
        {
            this.destination = destination;
            this.ticket = ticket;
            this.sail = sail;
            conti = ContinentMan.Contimoves[name];
            this.fee = fee;
        }
    }
    public static class contimove
    {
        public static IDictionary<int, StationData> Data = new Dictionary<int, StationData>()
        {
            { MapIds.ElliniaStation, new StationData("Ellinia2Orbis", ("Orbis Station in Ossyria", "Orbis"), ItemEtcIds.TicketToOrbisBasic, true, (1000,5000)) },
            { MapIds.Orbis2ElliniaStation, new StationData("Orbis2Ellinia", ("Ellinia of Victoria Island", "Ellinia"), ItemEtcIds.TicketToElliniaBasic, true, (1000,5000)) },
            { MapIds.Orbis2LudiStation, new StationData("Orbis2Ludi", ("Ludibrium", "Ludibrium"), ItemEtcIds.TicketToLudiBasic, false, (2000,6000)) },
            { MapIds.LudiStation, new StationData("Ludi2Orbis", ("Orbis Station in Ossyria", "Orbis"), ItemEtcIds.TicketToOrbisBasic, false, (2000,6000)) },
        };
    }

    // Selling Tickets ( Orbis : 2012000, Ludibrium : 2040000, Ellinia : ? )
    [Script("sell_ticket")]
    class sell_ticket : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var field = target.Field;

            if (target.Level < 10) self.Say("Your level seems to be too low for this. For your own safety, we do not allow anyone below level 10 to enter this course.");
            else
            {
                int[] stations = new int[] {};

                // Orbis 
                if (field.ID == MapIds.OrbisStationEntrance)
                {
                    stations = new int[] { MapIds.Orbis2ElliniaStation, MapIds.Orbis2LudiStation };
                }
                // Ludibrium
                else if (field.ID == MapIds.LudiTicketingPlace)
                {
                    stations = new int[] { MapIds.LudiStation };
                }
                // Ellinia
                else if (field.ID == MapIds.ElliniaStation)
                {
                    stations = new int[] { MapIds.ElliniaStation };
                }

                StationData station;
                int key;
                int menuIdx = 0;
                string ask = "";
                if (stations.Length > 1)
                {
                    string[] options = stations.Select(s => contimove.Data[s].destination.Item1).ToArray();
                    menuIdx = self.AskMenu($"Hello, I am responsible for selling tickets for the ship ride to all destinations. Which ticket would you like to buy?", options);
                }
                key = stations[menuIdx];
                station = contimove.Data[key];
                if (stations.Length == 0) ask += $"Hello, I am responsible for selling tickets for the boat trip to {station.destination.Item1}. ";

                bool discount = target.Level < 30;
                CONTIMOVE conti = station.conti;
                int fee = discount ? station.fee.Item1 : station.fee.Item2;
                int ticket = station.ticket + (discount ? 0 : 1);
                ask += $"The ride to {station.destination.Item2} leaves every {conti.DepartureMin} minutes, starting {(conti.DelayMin == 0 ? "on the hour" : "at " + conti.DelayMin + " minutes past")}, and will cost you #b{fee.Culture()} mesos#k. Are you sure you want to buy #b#t{ticket}##k?";
                var nRet = self.AskYesNo(ask);

                if (nRet == 0) self.Say("You must have some business to take care of, right?");
                else
                {
                    var inventory = target.Inventory;
                    var ret = inventory.Exchange(-fee, ticket, 1);
                    if (ret == 0) self.Say($"Are you sure you have #b{fee.Culture()} mesos#k? If so, then please check your etc. inventory and see if it is full or not.");
                    else self.Say("Have a safe trip!");
                }
            }
        }
    }
    // Getting the Ticket
    [Script("get_ticket")]
    class get_ticket : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            Map field = target.Field;
            if (!contimove.Data.TryGetValue(field.ID, out StationData station)) return;
            var conti = station.conti;
            long cTime = MasterThread.CurrentTime;
            long tWait = conti.WaitMin;
            long tDeparture = (conti.DepartingTime - cTime)/60000;
            if (MasterThread.IsDebug) target.Notice($"tDeparture: {tDeparture}m, tWait: {tWait}m");
            if (tDeparture <= 0) self.Say($"We will start boarding {tWait} minutes before departure. Please be patient and wait a few minutes. Please be assured that the ship will depart on time and we will not receive any more tickets 1 minute before it departs, so please be here on time.");
            else if (tDeparture <= 1) self.Say($"The ship is about to {(station.sail ? "set sail" : "leave")}. Sorry, but you'll have to get on the next one. Travel schedules are available through the ticket seller agent.");
            else
            {
                Map wField = GameDataProvider.Maps[field.ID + 1];
                if (wField.Characters.Count >= 500) self.Say("I'm sorry, but this ride is already full. We cannot accept any more passengers. Please board the next one.");
                else
                {
                    var nRet = self.AskYesNo("It looks like there's plenty of room for this ride. Please have your ticket ready so I can let you in. This ride will be long, but you'll get to your destination just fine. What do you think? Do you want to get on this ride?");
                    if (nRet == 0) self.Say("You must have some business to attend to here, right?");
                    else
                    {
                        var inven = target.Inventory;
                        bool regular = target.Level >= 30 || inven.ItemCount(station.ticket + 1) > 0;
                        int ticketID = station.ticket + (regular ? 1 : 0);
                        var ret = inven.Exchange(0, ticketID, -1);
                        if (ret == 0) self.Say("Oh no... I don't think you have your ticket with you. You can't board without it. Please buy the ticket at the ticket counter.");
                        else target.ChangeMap(wField.ID, "");
                    }
                }
            }
        }
    }
    // 240000100 : ¸®ÇÁ·¹ ¸ÅÇ¥¼Ò
    [Script("goOutWaitingRoom")]
    class goOutWaitingRoom : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var field = target.Field;

            var nRet = self.AskYesNo("Do you want to leave the waiting room? You can, but there will be NO ticket refunds. Are you sure you want to leave this room?");
            if (nRet == 0) self.Say("You will arrive at your destination soon. Go ahead and talk to other people and before you know it, you'll be there.");
            else
            {
                if (field.ID == 200000122) target.ChangeMap(200000100, "");
                else if (field.ID == 220000111) target.ChangeMap(220000100, "");
                else if (field.ID == 101000301) target.ChangeMap(101000300, "");
                else if (field.ID == 200000112) target.ChangeMap(200000100, "");
                else if (field.ID == 240000111) target.ChangeMap(240000100, "");
                else if (field.ID == 200000132) target.ChangeMap(200000100, "");
            }
        }
    }
    [Script("elevator")]
    class elevator : IPortalScript
    {
        public void Run(IPortalHost self, GameCharacter target)
        {
            var cTime = MasterThread.CurrentDateStr;
            var field = target.Field;

            var tMinute = int.Parse(cTime.Substring(12, 2)) % 4;
            if (field.ID == 222020200)
            {
                if (tMinute == 3) target.ChangeMap(222020210, "st00");
                else target.Message("No momento, o elevador não está disponível para este trajeto. Por favor, tente mais tarde.");
            }
            else if (field.ID == 222020100)
            {
                if (tMinute == 1) target.ChangeMap(222020110, "st00");
                else target.Message("No momento, o elevador não está disponível para este trajeto. Por favor, tente mais tarde.");
            }
        }
    }
    [Script("minar_elli")]
    class minar_elli : IPortalScript
    {
        dynamic ptname, ret, nItem, map, inven, say;
        public void Run(IPortalHost host, GameCharacter target)
        {
            var field = target.Field;
            inven = target.Inventory;
            nItem = inven.ItemCount(4031346);
            if (nItem > 0)
            {
                say = "Usou a semente mágica e será transferido para outro lugar.";
                target.Message(say);
                ret = inven.Exchange(0, 4031346, -1);
                if (ret != 0)
                {
                    if (field.ID == 101010000)
                    {
                        map = 240010100;
                        ptname = "elli00";
                    }
                    else if (field.ID == 240010100)
                    {
                        map = 101010000;
                        ptname = "minar00";
                    }
                    target.ChangeMap(map, ptname);
                }
            }
            else
            {
                say = "A Semente Mágica é necessária para passar pelo portal.";
                target.Message(say);
            }
        }
    }
    [Script("world_trip")]
    class world_trip : INpcScript
    {
        dynamic backmap, mapcode, cJob, mon, map, v0, qr;
        public void Run(INpcHost self, GameCharacter target)
        {
            var field = target.Field;
            qr = target.QuestRecord;

            cJob = target.Job;

            if (field.ID == 100000000 || field.ID == 101000000 || field.ID == 102000000 || field.ID == 103000000 || field.ID == 104000000 || field.ID == 200000000 || field.ID == 220000000 || field.ID == 680000000 || field.ID == 250000000 || field.ID == 240000000)
            {
                if (cJob == 0) self.Say("Se está entediado com a vida cotidiana, que tal cair fora, pra variar? Não há nada como mergulhar numa nova cultura, aprender algo novo em um minuto! Aproveite para cair fora e viajar. Recomendamos uma #bTour Mundial#k! Está preocupado com a despesa da viagem? Sem preocupações! A #bAgência de Viagnes Maple#k oferece primeira classe pelo precinho de #b300 mesos#k");
                else self.Say("Se está entediado com a vida cotidiana, que tal cair fora, pra variar? Não há nada como mergulhar numa nova cultura, aprender algo novo em um minuto! Aproveite para cair fora e viajar. Nós da Agência de Viagens Maple recomendamos você embarcar em um #b Tour Mundial#k! Está preocupado com a despesa da viagem? Não deveria! Nós da #bAgência de Viagens Maple#k, temos um pacote para você por SOMENTE #b3.000 mesos!#k");

                v0 = self.AskMenu("Agora lhe oferecemos este lugar para ter o prazer de viajar: #bSantuário dos Cogumelos no Japão#k. Estaremos lá, como seu guia turístico. Descanso garantido, o número de destinos ainda vão aumentar. Gostaria de ir agora para o Santuário dos Cogumelos?\r\n#b#L0# Sim, leve-me ao Santuário dos Cogumelos (Japão)#k#l");
                if (v0 == 0)
                {
                    self.Say("Gostaria de viajar para o #bSantuário dos Cogumelos no Japão#k? Se deseja sentir a essência do Japão, nada como visitar o Templo, um pólo cultural japonês. O Santuário dos Cogumelos é um lugar mítico que adora o Incomparável Deus Cogumelo dos tempos antigos.");
                    self.Say("Veja a sacerdotisa servir ao Deus Cogumelo e não deixe de provar Takoyaki, Yakisoba e outras delícias vendidas nas ruas do Japão. Agora vamos para o #bSantuário dos Cogumelos#k, um lugar mítico, como se um dia tivesse sido de fato.");
                    if (cJob == 0) mon = target.IncMoney(-300, 1);
                    else mon = target.IncMoney(-3000, 1);
                    if (mon == 0)
                    {
                        self.Say("Por favor, confira se tem mesos suficientes para ir.");
                        return;
                    }
                    else
                    {
                        mapcode = field.ID;
                        //map = wt_mapcode(mapcode);
                        qr.Set(8792, map);
                        target.ChangeMap(800000000, "st00");
                    }
                }
            }
            else if (field.ID == 800000000)
            {
                //backmap = wt_mapcode2(qr.Get(8792));
                v0 = self.AskMenu("Como está a viagem? Está curtindo?\r\n#b#L0#Sim, já basta de viajar. Posso voltar #m" + backmap + "#? #l\r\n#b#L1#NNão, gostaria de continuar a explorar este lugar.#l");
                if (v0 == 0)
                {
                    self.Say("Muito bem. Agora levarei você de volta para onde estava antes de visitar o Japão. Caso queira por o pé na estrada novamente, por favor, me avise!");
                    target.ChangeMap(backmap, "");
                }
                else if (v0 == 1)
                {
                    self.Say("Ok. Caso mude de idéia, por favor, me avise.");
                    return;
                }
            }
        }
    }
    //¹«¸ª-¿À¸£ºñ½º
    [Script("crane")]
    class crane : INpcScript
    {
        dynamic meso2, val2, ret, meso, field, val, v0;
        public void Run(INpcHost self, GameCharacter target)
        {
            if (target.Job == 0)
            {
                meso = 600;
                meso2 = 150;
            }
            else
            {
                meso = 6000;
                meso2 = 1500;
            }

            field = target.Field;
            if (field.ID == 250000100)
            {
                //ÃÊº¸ÀÚ
                if (target.Job == 0)
                {
                    val = self.AskMenu("Olá! Eu sou a cegonha que voa de #bMu Lung#k para #bOrbis#k e volta. Vôo o tempo todo, então imaginei por que não fazer algum dinheiro levando viajantes como você por um preço baixo? É um bom negócio para mim. O que você acha? Quer voar para #bOrbis#k agora mesmo? Para iniciantes, cobro só #b" + meso + "mesos#k.\r\n#b#L0# Está bem, leve-me a Orbis.#l");
                    if (val == 0)
                    {
                        var craneinst = FieldSet.Instances["Crane_SS"];
                        if (craneinst.Started)
                        {
                            if (target.Inventory.Mesos < meso) self.Say("Tem certeza de que você tem mesos suficientes?");
                            else
                            {
                                ret = craneinst.Enter(target, 0);
                                if (ret != 0) self.Say("Alguém mais está indo a Orbis neste exato momento. Fale comigo daqui a pouco.");
                                else
                                {
                                    ret = target.IncMoney(-meso, 1);
                                }
                            }
                        }
                        else self.Say("Alguém mais está indo a Orbis neste exato momento. Fale comigo daqui a pouco.");
                    }
                    else
                    {
                        val2 = self.AskYesNo("Quer mudar para #b#m251000000##k agora? Se tiver #b" + meso2 + "mesos#k, te levarei agora mesmo.");
                        if (val2 == 0) self.Say("Ok. Caso mude de idéia, por favor, me avise.");
                        else
                        {
                            ret = target.IncMoney(-meso2, 1);
                            if (ret == 0) self.Say("Tem certeza de que você tem mesos suficientes?");
                            else target.ChangeMap(251000000, "");
                        }
                    }
                    //ÀÏ¹Ý
                }
                else
                {
                    val = self.AskMenu("Olá! Eu sou a cegonha que voa de #bMu Lung#k para #bOrbis#k e volta. Vôo o tempo todo, então imaginei por que não fazer algum dinheiro levando viajantes como você por um preço baixo? É um bom negócio para mim. O que você acha? \r\n#b#L0# Orbis(" + meso + "mesos)#l\r\n#L1# Vila das Ervas(" + meso2 + "mesos#l");
                    if (val == 0)
                    {
                        var cNum = GameDataProvider.Maps[200090310].Characters.Count;
                        if (cNum <= 0)
                        {
                            if (target.Inventory.Mesos < meso) self.Say("Tem certeza de que você tem mesos suficientes?");
                            else
                            {
                                var craneset = FieldSet.Instances["Crane_SS"];
                                ret = craneset.Enter(target, 0);
                                if (ret != 0) self.Say("Alguém mais está indo a Orbis neste exato momento. Fale comigo daqui a pouco.");
                                else
                                {
                                    ret = target.IncMoney(-meso, 1);
                                }
                            }
                        }
                        else
                        {
                            self.Say("Alguém mais está indo a Orbis neste exato momento. Fale comigo daqui a pouco.");
                        }
                    }
                    else
                    {
                        val2 = self.AskYesNo("Quer mudar para #b#m251000000##k agora? Se tiver #b" + meso2 + "mesos#k, te levarei agora mesmo.");
                        if (val2 == 0) self.Say("Ok. Caso mude de idéia, por favor, me avise.");
                        else
                        {
                            ret = target.IncMoney(-meso2, 1);
                            if (ret == 0) self.Say("Tem certeza de que você tem mesos suficientes?");
                            else target.ChangeMap(251000000, "");
                        }
                    }
                }
                //¿À¸£ºñ½º
            }
            else if (field.ID == 200000141)
            {
                var craneset = FieldSet.Instances["Crane_MR"];
                if (target.Job == 0)
                {
                    if (craneset.Started)
                    {
                        v0 = self.AskMenu("Olá! Eu sou a cegonha que voa de #bOrbis#k para #bMu Lung#k e volta. Vôo o tempo todo, então imaginei por que não fazer algum dinheiro levando viajantes como você por um preço baixo? É um bom negócio para mim. O que você acha? Quer voar para #bMu Lung#k agora mesmo? Para iniciantes, cobro só #b" + meso + "mesos#k.\r\n#b#L0# Está bem, leve-me a Mu Lung.#l");
                        if (v0 == 0)
                        {
                            if (target.Inventory.Mesos < meso) self.Say("Tem certeza de que você tem mesos suficientes?");
                            else
                            {
                                ret = craneset.Enter(target, 0);
                                if (ret != 0)
                                {
                                    self.Say("Alguém mais está indo a Mu Lung neste exato momento. Fale comigo daqui a pouco.");
                                }
                                else
                                {
                                    ret = target.IncMoney(-meso, 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        self.Say("Alguém mais está indo a Mu Lung neste exato momento. Fale comigo daqui a pouco.");
                    }
                }
                else
                {
                    if (craneset.Started)
                    {
                        v0 = self.AskMenu("Olá! Eu sou a cegonha que voa de #bOrbis#k para #bMu Lung#k e volta. Vôo o tempo todo, então imaginei por que não fazer algum dinheiro levando viajantes como você por um preço baixo? É um bom negócio para mim. O que você acha? \r\n#b#L0# Mu Lung(" + meso + " mesos)#l");
                        if (v0 == 0)
                        {
                            if (target.Inventory.Mesos < meso) self.Say("Tem certeza de que você tem mesos suficientes?");
                            else
                            {
                                ret = craneset.Enter(target, 0);
                                if (ret != 0) self.Say("Alguém mais está indo a Mu Lung neste exato momento. Fale comigo daqui a pouco.");
                                else
                                {
                                    ret = target.IncMoney(-meso, 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        self.Say("Alguém mais está indo a Mu Lung neste exato momento. Fale comigo daqui a pouco.");
                    }
                }
            }
            else if (field.ID == 251000000)
            {
                if (target.Job == 0) v0 = self.AskYesNo("Olá! Eu sou a cegonha que voa de #bOrbis#k para #bMu Lung#k e volta. Vôo o tempo todo, então imaginei por que não fazer algum dinheiro levando viajantes como você por um preço baixo? É um bom negócio para mim. O que você acha? Quer voar para #bMu Lung#k agora mesmo? Para iniciantes, cobro só #b" + meso2 + " mesos#k. ");
                else v0 = self.AskYesNo("Olá! Eu sou a cegonha que voa de de #bOrbis#k para #bMu Lung#k e volta. Vôo o tempo todo, então imaginei por que não fazer algum dinheiro levando viajantes como você por um preço baixo? É um bom negócio para mim. O que você acha? Quer voar para #bMu Lung#k agora mesmo? Cobro só #b" + meso2 + " mesos#k.");
                if (v0 == 0) self.Say("Ok. Caso mude de idéia, por favor, me avise.");
                else
                {
                    ret = target.IncMoney(-meso2, 1);
                    if (ret == 0) self.Say("Tem certeza de que você tem mesos suficientes?");
                    else target.ChangeMap(250000100, "");
                }
            }
        }
    }
    //½ÇÁ¦ ÀÌµ¿½Ã Ãæµ¹ ½ºÅ©¸³Æ®
    [Script("crane_MR")]
    class crane_MR : INpcScript
    {
        dynamic time, move;
        public void Run(INpcHost self, GameCharacter target)
        {
            move = FieldSet.Instances["Crane_MR"];
            time = move.GetQuestTime;
            if (time >= 58) target.ChangeMap(250000100, "");
        }
    }
    [Script("crane_SS")]
    class crane_SS : INpcScript
    {
        dynamic time, move;
        public void Run(INpcHost self, GameCharacter target)
        {
            move = FieldSet.Instances["Crane_SS"];
            time = move.GetQuestTime;
            if (time >= 58) target.ChangeMap(200000141, "");
        }
    }
}

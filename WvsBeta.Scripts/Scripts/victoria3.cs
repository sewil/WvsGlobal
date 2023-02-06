using System;
using WvsBeta.Common;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    // selling tickets at the subway station to the construction site : The higher the level, the longer the list of tickets available. The character can enter without having to receive a quest.
    [Script("subway_ticket")]
    class subway_ticket : INpcScript
    {
        dynamic v, ret, inventory, giveTicket, nRet;
        public void Run(INpcHost self, GameCharacter target)
        {
            if (target.Level < 20) self.Say("You can enter if you bought a ticket, but it doesn't look like you'll be able to. There are strange contraptions underground and it must be very difficult for you to deal with them, so train, prepare and come back, okay?");
            else
            {
                giveTicket = 0;

                if (target.Level > 19 && target.Level < 30)
                {
                    v = self.AskMenu("You need to buy a ticket to enter. When you've purchased, you can enter at #p1052007# on the right. What would you like to buy? \r\n#b#L0#Construction Site B1#l");
                    if (v == 0) giveTicket = 1;
                }
                if (target.Level > 29 && target.Level < 40)
                {
                    v = self.AskMenu("You need to buy a ticket to enter. When you've purchased, you can enter at #p1052007# on the right. What would you like to buy? \r\n#b#L0#Construction Site B1#l \r\n#b#L1#Construction Site B2#l");
                    if (v == 0) giveTicket = 1;
                    else if (v == 1) giveTicket = 2;
                }
                if (target.Level > 39)
                {
                    v = self.AskMenu("You need to buy a ticket to enter. When you've purchased, you can enter at #p1052007# on the right. What would you like to buy? \r\n#b#L0#Construction Site B1#l \r\n#b#L1#Construction Site B2#l \r\n#b#L2#Construction Site B3#l");
                    if (v == 0) giveTicket = 1;
                    else if (v == 1) giveTicket = 2;
                    else if (v == 2) giveTicket = 3;
                }

                inventory = target.Inventory;
                // construction site B1 : 500 mesos, construction site B2 : 1200 mesos, construction site B3 : 2000 mesos
                if (giveTicket == 1)
                {
                    nRet = self.AskYesNo("Are you going to buy the ticket for #bConstruction Site B1#k? It will cost 500 mesos. Before making the purchase, make sure you have an empty slot in your etc inventory.");
                    if (nRet == 1)
                    {
                        ret = inventory.Exchange(-500, 4031036, 1);
                        if (ret == 0) self.Say("Are you short of money? Confirm whether or not you have an empty slot in your etc inventory.");
                        else self.Say("You can insert the ticket at #p1052007#. I heard that in Area 1 there are some precious items available, but with so many traps everywhere, most of them come back soon. I wish you the best of luck.");
                    }
                    else self.Say("You can enter the site if you have purchased the ticket. I heard that there are strange contraptions everywhere there, but in the end precious and rare items await you. So let me know if you change your mind.");
                }
                else if (giveTicket == 2)
                {
                    nRet = self.AskYesNo("Are you going to buy the ticket for #bConstruction Site B2#k? It will cost 1,200 mesos. Before making the purchase, make sure you have an empty slot in your etc inventory.");
                    if (nRet == 1)
                    {
                        ret = inventory.Exchange(-1200, 4031037, 1);
                        if (ret == 0) self.Say("Are you short of money? Confirm whether or not you have an empty slot in your etc inventory.");
                        else self.Say("You can enter the ticket at #p1052007#. I heard that in Area 2 there are some precious items available, but with so many traps everywhere, most of them come back soon. Please take care.");
                    }
                    else self.Say("You can enter the site if you have purchased the ticket. I heard that there are strange contraptions everywhere there, but in the end precious and rare items await you. So let me know if you change your mind.");
                }
                else if (giveTicket == 3)
                {
                    nRet = self.AskYesNo("Are you going to buy the ticket for #bConstruction Site B3#k? It will cost 2,000 mesos. Before making the purchase, make sure you have an empty slot in your etc inventory.");
                    if (nRet == 1)
                    {
                        ret = inventory.Exchange(-2000, 4031038, 1);
                        if (ret == 0) self.Say("Are you short of money? Confirm whether or not you have an empty slot in your etc inventory.");
                        else self.Say("You can enter the ticket at #p1052007#. I hear that in Area 3 there are some precious items available, but with so many traps everywhere, most of them come back soon. I wish you the best of luck.");
                    }
                    else self.Say("You can enter the site if you have purchased the ticket. I heard that there are strange contraptions everywhere there, but in the end precious and rare items await you. So let me know if you change your mind.");
                }
            }
        }
    }
    // Take the ticket and let the character into the construction site
    [Script("_subway_in")]
    class _subway_in : INpcScript
    {
        dynamic nRet, inventory, ret;
        public void Run(INpcHost self, GameCharacter target)
        {
            inventory = target.Inventory;

            if (inventory.ItemCount(4031036) >= 1)
            {
                nRet = self.AskYesNo("This is the ticket reader. Are you going to use #b#t4031036##k? If you use the ticket, you will be brought inside immediately.");
                if (nRet == 1)
                {
                    ret = inventory.Exchange(0, 4031036, -1);
                    if (ret == 0) self.Say("Insert #b#t4031036##k into the ticket reader.");
                    else target.ChangeMap(103000900, "");
                }
            }
            else if (inventory.ItemCount(4031037) >= 1)
            {
                nRet = self.AskYesNo("This is the ticket reader. Are you going to use #b#t4031037##k? If you use the ticket, you will be #Gbrought:brought# inside immediately.");
                if (nRet == 1)
                {
                    ret = inventory.Exchange(0, 4031037, -1);
                    if (ret == 0) self.Say("Insert #b#t4031037##k into the ticket reader.");
                    else target.ChangeMap(103000903, "");
                }
            }
            else if (inventory.ItemCount(4031038) >= 1)
            {
                nRet = self.AskYesNo("This is the ticket reader. Are you going to use #b#t4031038##k? If you use the ticket, you will be #Gbrought:brought# inside immediately.");
                if (nRet == 1)
                {
                    ret = inventory.Exchange(0, 4031038, -1);
                    if (ret == 0) self.Say("Insert #b#t4031038##k into the ticket reader.");
                    else target.ChangeMap(103000906, "");
                }
            }
            else self.Say("This is the ticket reader. You will not be allowed entry without a ticket.");
        }
    }
    [Script("subway_in")]
    class subway_in : INpcScript
    {
        dynamic tMinute, v0, ret, list, nPeople, field, nRet, inven;
        DateTime cTime;
        public void Run(INpcHost self, GameCharacter target)
        {

            field = target.Field;
            cTime = MasterThread.CurrentDate;
            inven = target.Inventory;
            tMinute = 0;
            nPeople = 0;

            list = "";
            if (inven.ItemCount(4031036) > 0) list = list + "\r\b#b#L0# Construction Site B1#l#k";
            if (inven.ItemCount(4031037) > 0) list = list + "\r\b#b#L1# Construction Site B2#l#k";
            if (inven.ItemCount(4031038) > 0) list = list + "\r\b#b#L2# Construction Site B3#l#k";

            // NLC
            //if (inven.ItemCount(4031710) > 0) list = list + "\r\b#b#L3# Cidade Folha Nova (para aprendizes)#l#k";
            //if (inven.ItemCount(4031711) > 0) list = list + "\r\b#b#L4# Cidade Folha Nova (Normal)#l#k";

            if (list == "")
            {
                self.Say("This is the ticket reader. You will not be allowed entry without a ticket.");
            }
            else
            {
                v0 = self.AskMenu("This is the ticket reader. You will be brought inside immediately. Which ticket would you like to use?" + list);
                if (v0 == 0)
                {
                    ret = inven.Exchange(0, 4031036, -1);
                    if (ret == 0) self.Say("Insert #b#t4031036##k into the ticket reader.");
                    else target.ChangeMap(103000900, "");
                }
                else if (v0 == 1)
                {
                    ret = inven.Exchange(0, 4031037, -1);
                    if (ret == 0) self.Say("Insert #b#t4031037##k into the ticket reader.");
                    else target.ChangeMap(103000903, "");
                }
                else if (v0 == 2)
                {
                    ret = inven.Exchange(0, 4031038, -1);
                    if (ret == 0) self.Say("Insert #b#t4031038##k into the ticket reader.");
                    else target.ChangeMap(103000906, "");
                }
                else if (v0 == 3) // NLC
                {

                    tMinute = cTime.Minute;

                    if (tMinute == 0 || tMinute == 5) self.Say("Nós começaremos a embarcar 2 minutos antes da partida. Por favor, seja paciente e espere alguns minutinhos. Fique #Gatento:atenta#, porque o metrô partirá na hora certa, e nós paramos de receber bilhetes 1 minuto antes disso. Então, por favor, esteja aqui dentro do horário.");
                    else if (tMinute == 9 || tMinute == 4) self.Say("O metrô para CFN está se preparando para a partida. Desculpe, mas você terá que embarcar na próxima viagem. O horário das viagens está disponível com o porteiro, no guichê.");
                    else
                    {
                        nPeople = field.GetUserCount;
                        if (nPeople >= 50) self.Say("Desculpe, mas esta viagem está LOTADA. Não podemos aceitar mais passageiros. Por favor, embarque na próxima viagem.");
                        else
                        {
                            nRet = self.AskYesNo("Parece que há muitos lugares vagos nesta viagem. Por favor, esteja com seu bilhete em mãos e então posso deixar você entrar. O percurso será longo, mas finalmente você chegará ao seu destino. O que você acha? Quer embarcar nesta viagem?");
                            if (nRet == 0) self.Say("Você deve ter algum negócio para cuidar aqui, certo?");
                            else
                            {
                                ret = inven.Exchange(0, 4031710, -1);
                                if (ret == 0) self.Say("Insira #b#t4031710##k no leitor de bilhetes.");
                                else target.ChangeMap(600010004, "st00");
                            }
                        }
                    }
                }
                else if (v0 == 4) // NLC
                {

                    tMinute = cTime.Minute;

                    if (tMinute == 0 || tMinute == 5) self.Say("Nós começaremos a embarcar 2 minutos antes da partida. Por favor, seja paciente e espere alguns minutinhos. Fique #Gatento:atenta#, porque o metrô partirá na hora certa, e nós paramos de receber bilhetes 1 minuto antes disso. Então, por favor, esteja aqui dentro do horário.");
                    else if (tMinute == 9 || tMinute == 4) self.Say("O metrô para CFN está se preparando para a partida. Desculpe, mas você terá que embarcar na próxima viagem. O horário das viagens está disponível com o porteiro, no guichê.");
                    else
                    {
                        nPeople = field.GetUserCount;
                        if (nPeople >= 50) self.Say("Desculpe, mas esta viagem está LOTADA. Não podemos aceitar mais passageiros. Por favor, embarque na próxima viagem.");
                        else
                        {
                            nRet = self.AskYesNo("Parece que há muitos lugares vagos nesta viagem. Por favor, esteja com seu bilhete em mãos e então posso deixar você entrar. O percurso será longo, mas finalmente você chegará ao seu destino. O que você acha? Quer embarcar nesta viagem?");
                            if (nRet == 0) self.Say("Você deve ter algum negócio para cuidar aqui, certo?");
                            else
                            {
                                ret = inven.Exchange(0, 4031711, -1);
                                if (ret == 0) self.Say("Insira #b#t4031711##k no leitor de bilhetes.");
                                else target.ChangeMap(600010004, "st00");
                            }
                        }
                    }
                }
            }
        }
    }
    // The place where the character finds the coin at construction site B1: If the character came in without receiving the quest from Shumi, then a random ore of a mineral will be given at the end.
    [Script("subway_get1")]
    class subway_get1 : INpcScript
    {
        dynamic ret, qr, inventory, val, nNewItemID, nNewItemNum, rnum;
        public void Run(INpcHost self, GameCharacter target)
        {
            qr = target.QuestRecord;
            val = qr.GetState(2055);

            inventory = target.Inventory;

            if (inventory.ItemCount(4031039) < 1)
            {
                if (val == 1)
                {
                    ret = inventory.Exchange(0, 4031039, 1);
                    if (ret == 0) self.Say("Looking closely at #p1052008# it appears to contain a shiny object, only since your etc inventory is full, the item is inaccessible.");
                    else
                    {
                        self.Say("Looking closely at #p1052008#, it appears to contain a shiny object. You stretch out your hand and find a small coin.");
                        target.ChangeMap(103000100, "");
                    }
                }
                else
                {
                    // If the character walks in without a quest, then give him/her one raw ore of a mineral; if the character walked in after finishing up a quest, then give him/her 2 raw ores of a random mineral.
                    if (inventory.SlotCount(4) > inventory.HoldCount(4))
                    {
                        if (val == 2) nNewItemNum = 2;
                        else nNewItemNum = 1;

                        rnum = Rand32.NextBetween(0, 5);
                        nNewItemID = 0;
                        if (rnum == 0) nNewItemID = 4010000;
                        else if (rnum == 1) nNewItemID = 4010001;
                        else if (rnum == 2) nNewItemID = 4010002;
                        else if (rnum == 3) nNewItemID = 4010003;
                        else if (rnum == 4) nNewItemID = 4010004;
                        else if (rnum == 5) nNewItemID = 4010005;
                        ret = inventory.Exchange(0, nNewItemID, nNewItemNum);
                        if (ret == 0) self.Say("Looking closely at #t1052008#, it looks like it contains a shiny object, only since your etc inventory is full, the item is inaccessible. Free up some space in your etc inventory, then come back here.");
                        else target.ChangeMap(103000100, "");
                    }
                    else self.Say("You may not have the item you found because your etc inventory is full. Free up some space for the item and then go back");
                }
            }
            else
            {
                self.Say("You already have the coin. We looked further and found nothing.");
                target.ChangeMap(103000100, "");
            }
        }
    }
    // The place where the character finds the stack of money at construction site B2 : If the character came in without receiving the quest from Shumi, an ore of a random jewel will be given.
    [Script("subway_get2")]
    class subway_get2 : INpcScript
    {
        dynamic ret, qr, inventory, val, nNewItemID, nNewItemNum, rnum;
        public void Run(INpcHost self, GameCharacter target)
        {
            qr = target.QuestRecord;
            val = qr.GetState(2056);

            inventory = target.Inventory;

            if (inventory.ItemCount(4031040) < 1)
            {
                if (val == 1)
                {
                    ret = inventory.Exchange(0, 4031040, 1);
                    if (ret == 0) self.Say("Looking closely at #p1052008# it looks like it has a pile of papers, only since your etc inventory is full, the item is inaccessible.");
                    else
                    {
                        self.Say("Looking closely at #p1052008#, it looks like there's a pile of papers there. I held out my hand and... aha! A huge pile of cash.");
                        target.ChangeMap(103000100, "");
                    }
                }
                else
                {
                    // 1 random ore of a jewel if the character went in without receiving a quest, 2 if the character received the quest.
                    if (inventory.SlotCount(4) > inventory.HoldCount(4))
                    {
                        if (val == 2) nNewItemNum = 2;
                        else nNewItemNum = 1;

                        rnum = Rand32.NextBetween(0, 6);
                        nNewItemID = 0;
                        if (rnum == 0) nNewItemID = 4020000;
                        else if (rnum == 1) nNewItemID = 4020001;
                        else if (rnum == 2) nNewItemID = 4020002;
                        else if (rnum == 3) nNewItemID = 4020003;
                        else if (rnum == 4) nNewItemID = 4020004;
                        else if (rnum == 5) nNewItemID = 4020005;
                        else if (rnum == 6) nNewItemID = 4020006;
                        ret = inventory.Exchange(0, nNewItemID, nNewItemNum);
                        if (ret == 0) self.Say("Looking closely at #t1052008# it looks like there's something in there, but since your etc inventory is full, the item is inaccessible. Free up space for the item and then go back.");
                        else target.ChangeMap(103000100, "");
                    }
                    else self.Say("You may not have the item you found because your etc inventory is full. Free up space for the item and then go back.");
                }
            }
            else
            {
                self.Say("You already have the pile of cash. We looked further and found nothing.");
                target.ChangeMap(103000100, "");
            }
        }
    }
    // The place where the character finds the sack of money at construction site B3 : If the character came in without receiving the quest from Shumi, a random ore of a rare mineral or a jewel will be given.
    [Script("subway_get3")]
    class subway_get3 : INpcScript
    {
        dynamic ret, qr, inventory, val, nNewItemID, nNewItemNum, rnum;
        public void Run(INpcHost self, GameCharacter target)
        {
            qr = target.QuestRecord;
            val = qr.GetState(2057);

            inventory = target.Inventory;

            if (inventory.ItemCount(4031041) < 1)
            {
                if (val == 1)
                {
                    ret = inventory.Exchange(0, 4031041, 1);
                    if (ret == 0) self.Say("Looking closely at #p1052008#, it looks like there's a bag of something that contains a shiny object, only since your etc inventory is full, the item is inaccessible.");
                    else
                    {
                        self.Say("Looking closely at #p1052008#, it looks like there's a bag of something that contains a shiny object. You stretched out your hand and managed to reach a heavy bag of coins.");
                        target.ChangeMap(103000100, "");
                    }
                }
                else
                {
                    // 1 random ore of a mineral or a rare jewel if entering without the quest, 2 if with the quest.
                    if (inventory.SlotCount(4) > inventory.HoldCount(4))
                    {
                        if (val == 2) nNewItemNum = 2;
                        else nNewItemNum = 1;

                        rnum = Rand32.NextBetween(0, 2);
                        nNewItemID = 0;
                        if (rnum == 0) nNewItemID = 4010006;
                        else if (rnum == 1) nNewItemID = 4020007;
                        else if (rnum == 2) nNewItemID = 4020008;
                        ret = inventory.Exchange(0, nNewItemID, nNewItemNum);
                        if (ret == 0) self.Say("Looking closely at #t1052008# it looks like there's something in there, but since your etc inventory is full, the item is inaccessible. Free up space for the item and then go back.");
                        else target.ChangeMap(103000100, "");
                    }
                    else self.Say("You may not have the item you found because your etc inventory is full. Free up space for the item and then go back.");
                }
            }
            else
            {
                self.Say("You already have the bag of coins. We looked further and found nothing.");
                target.ChangeMap(103000100, "");
            }
        }
    }
    // The device for the character to leave if giving up midway through the quest
    [Script("subway_out")]
    class subway_out : INpcScript
    {
        dynamic nRet;
        public void Run(INpcHost self, GameCharacter target)
        {
            nRet = self.AskYesNo("This device is connected to the outside. Will you give up and leave this place? You'll have to start from where you started the next time you come here...");
            if (nRet == 1) target.ChangeMap(103000100, "");
        }
    }
    // A receptionist from sleepywood hotel
    [Script("hotel1")]
    class hotel1 : INpcScript
    {
        dynamic mon, v1, nRet;
        public void Run(INpcHost self, GameCharacter target)
        {
            self.Say("Welcome. We are Hotel #m105040300#. Our hotel works hard to serve you better at all times. If you're tired or feeling exhausted from hunting, how about a relaxing stay at our hotel?");
            v1 = self.AskMenu("We offer two types of room service. Please choose the one you prefer. \r\n#L0##bRegular sauna (499 mesos per use) #k#l\r\n#L1##bVIP sauna (999 mesos per use)#k#l");
            if (v1 == 0)
            {
                nRet = self.AskYesNo("You have chosen the regular sauna. Your HP and MP will quickly restore itself and you can even buy some items there. Are you sure you want to enter?");
                if (nRet == 0) self.Say("We also offer other types of services. Please think it over and then decide.");
                else if (nRet == 1)
                {
                    mon = target.IncMoney(-499, 1);
                    if (mon == 0) self.Say("I'm sorry. It looks like you don't have any money. Staying in our hotel will cost at least 499 mesos.");
                    else target.ChangeMap(105040401, "");
                }
            }
            else if (v1 == 1)
            {
                nRet = self.AskYesNo("You have chosen the VIP sauna. Your HP and MP will restore faster than in the regular sauna, and you might even find a special item there. Are you sure you want to sign in?");
                if (nRet == 0) self.Say("We also offer other types of services. Please think it over and then decide.");
                else if (nRet == 1)
                {
                    mon = target.IncMoney(-999, 1);
                    if (mon == 0) self.Say("I'm sorry. It looks like you don't have any money. Staying in our hotel will cost at least 999 mesos.");
                    else target.ChangeMap(105040402, "");
                }
            }
        }
    }
}

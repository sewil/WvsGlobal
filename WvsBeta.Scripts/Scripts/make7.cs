using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class make7
    {
        static INpcHost self;
        static GameCharacter target;
        public static void makeSton1(int index, string makeItem, string needItem)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRet = self.AskYesNo("In order to make #bfive " + makeItem + "s#k, you will need the following items. Most of them can be obtained by eliminating monsters, so it won't be too difficult to get them. What do you think? Do you want some?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("Not enough materials, huh? No problem. Come see me when you have the necessary items. There are several ways to acquire them. You can hunt monsters or buy them from other people, so don't get discouraged.");
            else
            {
                // The Magic Rock
                if (index == 0) ret = inventory.Exchange(-4000, 4000046, -20, 4000027, -20, 4021001, -1, 4006000, 5);
                else if (index == 1) ret = inventory.Exchange(-4000, 4000025, -20, 4000049, -20, 4021006, -1, 4006000, 5);
                else if (index == 2) ret = inventory.Exchange(-4000, 4000129, -15, 4000130, -15, 4021002, -1, 4006000, 5);
                else if (index == 3) ret = inventory.Exchange(-4000, 4000074, -15, 4000057, -15, 4021005, -1, 4006000, 5);
                else if (index == 4) ret = inventory.Exchange(-4000, 4000054, -7, 4000053, -7, 4021003, -1, 4006000, 5);
                else if (index == 5) ret = inventory.Exchange(-4000, 4000238, -15, 4000241, -15, 4021000, -1, 4006000, 5);

                // The Summoning Rock
                else if (index == 100) ret = inventory.Exchange(-4000, 4000028, -20, 4000027, -20, 4011001, -1, 4006001, 5);
                else if (index == 101) ret = inventory.Exchange(-4000, 4000014, -20, 4000056, -20, 4011003, -1, 4006001, 5);
                else if (index == 102) ret = inventory.Exchange(-4000, 4000132, -15, 4000128, -15, 4011005, -1, 4006001, 5);
                else if (index == 103) ret = inventory.Exchange(-4000, 4000074, -15, 4000069, -15, 4011002, -1, 4006001, 5);
                else if (index == 104) ret = inventory.Exchange(-4000, 4000080, -7, 4000079, -7, 4011004, -1, 4006001, 5);
                else if (index == 105) ret = inventory.Exchange(-4000, 4000226, -15, 4000237, -15, 4011001, -1, 4006001, 5);

                if (ret == 0) self.Say("Please check that you have all the items you need, and that you have some space available in your etc. inventory.");
                else self.Say("Here, take 5 pieces of #b" + makeItem + "#k. Even I have to admit this is a work of art. Alright, if you ever need my help again, just come back here and talk to me!");

            }
        }
        // The Traveling Alchemist : 2040050
        [Script("make_ston")]
        class make_ston : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                make7.self = self;
                make7.target = target;
                self.Say("Alright, mix the frog's tongue with the squirrel's tooth and... Ah! I almost forgot! I forgot to put the white glitter powder!! Jeeze, I almost caused a disaster... Woah!! How long have you been here? I think I got carried away just a little bit in my work... hehe.");
                var v1 = self.AskMenu("As you can see, I'm a traveling alchemist, but I can still come up with some things you might need. Do you want to check it out?",
                    "Make #t4006000#",
                    "Make #t4006001#"
                );
                // Make the Magic Rock (4006000)
                int itemID = v1 == 0 ? 4006000 : 4006001;

                string ask = "Haha... #b#t" + itemID + "##k is a mystical rock that only I can make. Many adventurers seem to need it to use their more powerful skills that demand more of their MP and HP. There are five ways to make #t" + itemID + "#. Which way do you want to use to make it?";

                if (v1 == 0)
                {
                    v1 = self.AskMenu(ask + "\r\n\r\n#b#L0# Make it using #t4000046# and #t4000027##l\r\n#b#L1# Make it using #t4000025# and #t4000049##l\r\n#b#L2# Make it using #t4000129# and #t4000130##l\r\n#b#L3# Make it using #t4000074# and #t4000057##l\r\n#b#L4# Make it using #t4000054# and #t4000053##l");
                    if (v1 == 0) makeSton1(0, "#t4006000#", "#v4000046# 20 #t4000046#s\r\n#v4000027# 20 #t4000027#s\r\n#v4021001# #t4021001#\r\n#v4031138# 4,000 mesos");
                    else if (v1 == 1) makeSton1(1, "#t4006000#", "#v4000025# 20 #t4000025#s\r\n#v4000049# 20 #t4000049#s\r\n#v4021006# #t4021006#\r\n#v4031138# 4,000 mesos");
                    else if (v1 == 2) makeSton1(2, "#t4006000#", "#v4000129# 15 #t4000129#s\r\n#v4000130# 15 #t4000130#s\r\n#v4021002# #t4021002#\r\n#v4031138# 4,000 mesos");
                    else if (v1 == 3) makeSton1(3, "#t4006000#", "#v4000074# 15 #t4000074#s\r\n#v4000057# 15 #t4000057#s\r\n#v4021005# #t4021005#\r\n#v4031138# 4,000 mesos");
                    else if (v1 == 4) makeSton1(4, "#t4006000#", "#v4000054# 7 #t4000054#s\r\n#v4000053# 7 #t4000053#s\r\n#v4021003# #t4021003#\r\n#v4031138# 4,000 mesos");
                    else if (v1 == 5) makeSton1(5, "#t4006000#", "#v4000238# 15 #t4000238#s \r\n#v4000241# 15 #t4000241#s\r\n#v4021000# #t4021000#\r\n#v4031138# 4,000 mesos");
                }
                // Make the Summoning Rock (4006001)
                else if (v1 == 1)
                {
                    v1 = self.AskMenu(ask + "\r\n\r\n#b#L0# Make it using #t4000028# and #t4000027##l\r\n#b#L1# Make it using #t4000014# and #t4000056##l\r\n#b#L2# Make it using #t4000132# and #t4000128##l\r\n#b#L3# Make it using #t4000074# and #t4000069##l\r\n#b#L4# Make it using #t4000080# and #t4000079##l");
                    if (v1 == 0) makeSton1(100, "#t4006001#", "#v4000028# 20 #t4000028#s\r\n#v4000027# 20 #t4000027#s\r\n#v4011001# #t4011001#\r\n#v4031138# 4,000 mesos");
                    else if (v1 == 1) makeSton1(101, "#t4006001#", "#v4000014# 20 #t4000014#s\r\n#v4000056# 20 #t4000056#s\r\n#v4011003# #t4011003#\r\n#v4031138# 4,000 mesos");
                    else if (v1 == 2) makeSton1(102, "#t4006001#", "#v4000132# 15 #t4000132#s\r\n#v4000128# 15 #t4000128#s\r\n#v4011005# #t4011005#\r\n#v4031138# 4,000 mesos");
                    else if (v1 == 3) makeSton1(103, "#t4006001#", "#v4000074# 15 #t4000074#s\r\n#v4000069# 15 #t4000069#s\r\n#v4011002# #t4011002#\r\n#v4031138# 4,000 mesos");
                    else if (v1 == 4) makeSton1(104, "#t4006001#", "#v4000080# 7 #t4000080#s\r\n#v4000079# 7 #t4000079#s\r\n#v4011004# #t4011004#\r\n#v4031138# 4,000 mesos");
                    else if (v1 == 5) makeSton1(105, "#t4006001#", "#v4000226# 15 #t4000226#s\r\n#v4000237# 15 #t4000237#s\r\n#v4011001# #t4011001#\r\n#v4031138# 4,000 mesos");
                }
            }
        }
    }
}

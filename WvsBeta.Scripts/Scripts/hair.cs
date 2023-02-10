using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class hair
    {
        static INpcHost self;
        static GameCharacter target;

        static void SayHairResult(int mHair, string success)
        {
            if (mHair == 1) self.Say(success);
            else if (mHair == -1) self.Say("Um... Looks like you don't have the right coupon... Too bad I can't cut your hair without it. I'm sorry.");
            else if (mHair == -3) self.Say("I'm sorry. A little problem here in the salon. I don't think I can cut your hair right now. Please come back later.");
            else if (mHair == 0 || mHair == -2) self.Say("I'm sorry. Looks like we have a little problem changing your hairstyle. Please come back in a little while.");
        }

        static int[] GetStyles(int[] mStyles, int[] fStyles)
        {
            var tHair = target.Hair % 10;
            if (target.Gender == 0)
            {
                return mStyles.Select(i => i + tHair).ToArray();
            }
            else
            {
                return fStyles.Select(i => i + tHair).ToArray();
            }
        }

        static int[] GetColors(int[] colors)
        {
            var cHair = target.Hair;
            var eHair = cHair - (cHair % 10);
            return colors.Select(i => eHair + i).ToArray();
        }

        // Henesys VIP hair salon 
        [Script("hair_henesys1")]
        class hair_henesys1 : INpcScript
        {
            int[] mStyles = new int[]
            {
                30030,
                30020,
                30000,
                30310,
                30330,
                30060,
                30150,
                30410,
                30210,
                30140,
                30120,
                30200,
            };
            int[] fStyles = new int[]
            {
                31050,
                31040,
                31000,
                31150,
                31310,
                31160,
                31100,
                31410,
                31030,
                31080,
                31070
            };
            int[] colors = new int[] { 0, 1, 2, 4, 6, 7 };
            dynamic mHair;
            public void Run(INpcHost self, GameCharacter target)
            {
                hair.self = self;
                hair.target = target;
                var selectHair = self.AskMenu("I'm Natalie, the head of this beauty salon. If you have a #b#t4050001##k or #b#t4051001##k, I can take care of your hairstyle. Select what you want to do.\r\n#b#L0# Change style (VIP coupon)#l\r\n#L1# Dye your hair (VIP Coupon)#l");
                if (selectHair == 0)
                {
                    var styles = GetStyles(mStyles, fStyles);

                    mHair = self.AskAvatar("I can change your hair style and make it look great. Why don't you change it up a bit? If it has #b#t5150001##k I will change it for you. Choose one to your liking.", 4050001, styles);
                }
                else if (selectHair == 1)
                {
                    var cs = GetColors(colors);

                    mHair = self.AskAvatar("I can change your hair style and make it look great. Why don't you change it up a bit? If it has #b#t4005100 I will change it for you. Choose one to your liking.", 4051001, cs);
                }

                hair.SayHairResult(mHair, "Check it out!! What do you think? Even I think it's a work of art! HAHAHA. Call me if you want a new haircut because I'll make you look great every time!");
            }
        }
        // Henesys EXP hair salon 
        [Script("hair_henesys2")]
        class hair_henesys2 : INpcScript
        {
            int[] mStyles = new int[]
            {
                30000,
                30020,
                30030,
                30060,
                30120,
                30140,
                30150,
                30200,
                30210,
                30310,
                30330,
                30410
            };
            int[] fStyles = new int[]
            {
                31000,
                31030,
                31040,
                31050,
                31080,
                31070,
                31100,
                31150,
                31160,
                31300,
                31310,
                31410
            };
            int[] colors = new int[] { 0, 1, 2, 4, 6, 7 };
            dynamic cHair, nRet1, tHair, mHair, selectHair;
            public void Run(INpcHost self, GameCharacter target)
            {
                hair.self = self;
                hair.target = target;
                selectHair = self.AskMenu("I'm Britney, the assistant. If you happen to have #b#t4050000##k or #b#t4051000##k, how about letting me change your hairstyle?\r\n#b#L0# Change Style (reg. coupon)#l\r\n #L2# Dye your hair (reg. coupon)#l");
                if (selectHair == 0)
                {
                    nRet1 = self.AskYesNo("If you use the regular coupon, your hair will RANDOMLY change with a chance to win a new style you didn't think was possible. Are you going to use #b#t4050000##k and really change your style?");
                    if (nRet1 == 0)
                    {
                        self.Say("I understand... Think about it and if you still want to change, come back to me.");
                        return;
                    }
                    else if (nRet1 == 1)
                    {
                        var styles = GetStyles(mStyles, fStyles);
                        mHair = self.MakeRandAvatar(4050000, styles);
                    }
                }
                else if (selectHair == 2)
                {
                    nRet1 = self.AskYesNo("If you use a normal coupon, your hair will RANDOMLY change. Still want to use #b#t4051000##k and change everything?");
                    if (nRet1 == 0)
                    {
                        self.Say("I understand... Think about it and if you still want to change, come back to me.");
                        return;
                    }
                    else if (nRet1 == 1)
                    {
                        var cs = GetColors(colors);
                        mHair = self.MakeRandAvatar(4051000, cs);
                    }
                }

                hair.SayHairResult(mHair, "Here's the mirror. What do you think of the new haircut? I knew it wasn't going to be the sexiest of them all, but didn't it look good? Come back when you need a new change!");
            }
        }
        // Kerning City VIP hair salon 
        [Script("hair_kerning1")]
        class hair_kerning1 : INpcScript
        {
            int[] mStyles = new int[]
            {
                30030,
                30020,
                30000,
                30130,
                30350,
                30190,
                30110,
                30180,
                30050,
                30040,
                30160
            };
            int[] fStyles = new int[]
            {
                31050,
                31040,
                31000,
                31060,
                31090,
                31330,
                31020,
                31130,
                31120,
                31140,
                31010
            };
            int[] colors = new int[] { 0, 2, 3, 7, 5 };
            dynamic cHair, tHair, mHair, selectHair, eHair;
            public void Run(INpcHost self, GameCharacter target)
            {
                hair.self = self;
                hair.target = target;
                selectHair = self.AskMenu("Hey! I'm Don Giovanni, the head of the beauty salon! If you have #b#t4050003##k, #b#t4051003##k, why don't you let me handle the rest? Decide what you want to do with your hair...\r\n#b#L0# Change Style (VIP coupon)#l\r\n#L1# Dye your hair (VIP coupon)#l");
                if (selectHair == 0)
                {
                    var styles = GetStyles(mStyles, fStyles);

                    mHair = self.AskAvatar("I can change your hair style to something totally new. Aren't you bored of your hair anymore? I can make a new cut with #b#t4050003##k. Choose the style according to your taste.", 4050003, styles);
                }
                else if (selectHair == 1)
                {
                    var cs = GetColors(colors);

                    mHair = self.AskAvatar("I can change your hair color to something totally new. Aren't you bored of your hair anymore? I can dye your hair if you have #b#t4051003##k. Choose the color you like!", 4051003, cs);
                }
                hair.SayHairResult(mHair, "Okay, look at your new haircut. What do you think? Even I admit this is a work of art! HAHAHA. Look for me when you want a new haircut. I take care of the rest!");
            }
        }
        // Kerning City EXP hair salon 
        [Script("hair_kerning2")]
        class hair_kerning2 : INpcScript
        {
            int[] mStyles = new int[]
            {
                30000,
                30020,
                30030,
                30040,
                30050,
                30110,
                30130,
                30160,
                30180,
                30190,
                30350,
                30400
            };
            int[] fStyles = new int[]
            {
                31000,
                31010,
                31020,
                31040,
                31050,
                31060,
                31090,
                31120,
                31130,
                31140,
                31330,
            };
            int[] colors = new int[] { 0, 2, 3, 7, 5 };
            dynamic cHair, tHair, mHair, selectHair, eHair, nRet1;
            public void Run(INpcHost self, GameCharacter target)
            {
                hair.self = self;
                hair.target = target;
                selectHair = self.AskMenu("I'm Andrew, Don's assistant. Everyone calls me Andre. If you have a #b#t4050002##k or #b#t4051002##k, let me change your hairstyle...\r\n#b#L0# Change Style (reg. coupon)#l\r\n#L2# Dye your hair (reg. coupon)#l");
                if (selectHair == 0)
                {
                    nRet1 = self.AskYesNo("If you use the regular coupon, your hair will RANDOMLY change with a chance to win a new style you didn't think was possible. Are you going to use #b#t4050002##k and really change your style?");
                    if (nRet1 == 0)
                    {
                        self.Say("I understand... Think a little more and, if you want, look for me.");
                        return;
                    }
                    else if (nRet1 == 1)
                    {
                        var styles = GetStyles(mStyles, fStyles);

                        mHair = self.MakeRandAvatar(4050002, styles);
                    }
                }
                else if (selectHair == 2)
                {
                    nRet1 = self.AskYesNo("If you use a regular coupon, your hair will change randomly. Still want to wear #b#t4051002##k and dye your hair?");
                    if (nRet1 == 0)
                    {
                        self.Say("I understand... Think a little more and, if you want, look for me.");
                        return;
                    }
                    else if (nRet1 == 1)
                    {
                        var cs = GetColors(colors);
                        mHair = self.MakeRandAvatar(4051002, cs);
                    }
                }

                hair.SayHairResult(mHair, "Here's the mirror. Your new cut! What do you think? I know it's not the coolest, but it looks really cool! Come back when you need a new change!");
            }
        }
        // Orbis VIP hair salon 
        [Script("hair_orbis1")]
        class hair_orbis1 : INpcScript
        {
            int[] mStyles = new int[]
            {
                30030,
                30020,
                30000,
                30230,
                30260,
                30280,
                30240,
                30290,
                30270,
                30340
            };
            int[] fStyles = new int[]
            {
                31040,
                31000,
                31250,
                31220,
                31260,
                31240,
                31110,
                31270,
                31030,
                31230
            };
            int[] colors = new int[] { 0, 1, 7, 3, 4, 5 };
            dynamic cHair, changeHair1, nRet1, tHair, mHair, selectHair, eHair;
            public void Run(INpcHost self, GameCharacter target)
            {
                hair.self = self;
                hair.target = target;
                selectHair = self.AskMenu("Hello, I'm #p2010001#. If you have #b#t4050005##k or #b#t4051005##k then let me take care of your hair. Decide what you want to do with it.\r\n#b#L0# Change Style (VIP coupon)#l\r\n#L1# Dye your hair (VIP coupon)#l\r\n#L2# Use #t4054000##l\r\n");
                if (selectHair == 0)
                {
                    var styles = GetStyles(mStyles, fStyles);
                    mHair = self.AskAvatar("I can change the look of your hair completely. Are you ready for a change? If it has #b#t4050005##k I will change it for you. Choose whatever you like!", 4050005, styles);
                }
                else if (selectHair == 1)
                {
                    var cs = GetColors(colors);
                    mHair = self.AskAvatar("I can change your hair color completely. Are you ready for a change? If you have #b#t4051005##k I will change it for you. Choose whatever you like!", 4051005, cs);
                }
                else if (selectHair == 2)
                {
                    cHair = target.Hair;
                    if (cHair == 31240 || cHair == 31241 || cHair == 31242 || cHair == 31243 || cHair == 31244 || cHair == 31245 || cHair == 31246 || cHair == 31247)
                    {
                        self.Say("Want to use #b#t4054000##k? Sorry, but it looks like you've already disheveled your hair. Try again another time.");
                        return;
                    }
                    else
                    {
                        nRet1 = self.AskYesNo("I think you have #b#t4054000##k on you. This coupon is specifically made for all women who prefer tousled hair. Wear it and I'll instantly change your look to shaggy hair. Are you sure you want to use #b#t4054000##k and change your hair?");
                        if (nRet1 == 0)
                        {
                            self.Say("I see... Well, think it over, and if you change your mind, come talk to me.");
                            return;
                        }
                        else
                        {
                            tHair = target.Hair % 10;
                            var changeHair = 31240 + tHair;
                            mHair = self.MakeRandAvatar(4054000, changeHair);
                        }
                    }
                }

                hair.SayHairResult(mHair, "Check it out!! What do you think? Even I think it's a work of art! HAHAHA. Call me if you want a new haircut because I'll make you look great every time!");
            }
        }
        // Orbis EXP hair salon 
        [Script("hair_orbis2")]
        class hair_orbis2 : INpcScript
        {
            int[] mStyles = new int[]
            {
                30000,
                30020,
                30030,
                30230,
                30240,
                30260,
                30270,
                30280,
                30290,
                30340,
                30440,
                30400
            };
            int[] fStyles = new int[]
            {
                31000,
                31030,
                31040,
                31110,
                31220,
                31230,
                31240,
                31250,
                31260,
                31270,
                31320,
            };
            int[] colors = new int[] { 0, 1, 7, 3, 4, 5 };
            dynamic cHair, tHair, mHair, selectHair, eHair, nRet1;
            public void Run(INpcHost self, GameCharacter target)
            {
                hair.self = self;
                hair.target = target;
                selectHair = self.AskMenu("I'm #p2012007#, the assistant. Do you have #b#t4050004##k or #b#t4051004##k with you? If so, how about letting me do your hair? What do you want to do with your hair?\r\n#b#L0# Change Style (reg. coupon)#l\r\n#L2# Dye your hair (reg. coupon)#l");
                if (selectHair == 0)
                {
                    nRet1 = self.AskYesNo("If you use the regular coupon, your hair will RANDOMLY change with a chance to win a new style you didn't think was possible. Still want to use #b#t4050004##k and change your hair?");
                    if (nRet1 == 0)
                    {
                        self.Say("I understand... Think about it and if you still want to change, come back to me.");
                        return;
                    }
                    else if (nRet1 == 1)
                    {
                        var styles = GetStyles(mStyles, fStyles);
                        mHair = self.MakeRandAvatar(4050004, styles);
                    }
                }
                else if (selectHair == 2)
                {
                    nRet1 = self.AskYesNo("If you use a normal coupon, your hair will change randomly. Still want to use #b#t4051004##k and change everything?");
                    if (nRet1 == 0)
                    {
                        self.Say("I understand... Think about it and if you still want to change, come back to me.");
                        return;
                    }
                    else if (nRet1 == 1)
                    {
                        var cs = GetColors(colors);

                        mHair = self.MakeRandAvatar(4051004, cs);
                    }
                }

                hair.SayHairResult(mHair, "Here's the mirror. What do you think of the new haircut? I knew it wasn't going to be the sexiest of them all, but didn't it look good? Stop by if you feel you need a change again.");
            }
        }
        // Ludibrium VIP Hair-Salon 
        [Script("hair_ludi1")]
        class hair_ludi1 : INpcScript
        {
            int[] mStyles = new int[]
            {
                30030,
                30020,
                30000,
                30250,
                30190,
                30150,
                30050,
                30280,
                30240,
                30300,
                30160
            };
            int[] fStyles = new int[]
            {
                31040,
                31000,
                31150,
                31280,
                31160,
                31120,
                31290,
                31270,
                31030,
                31230,
                31010
            };
            int[] colors = new int[] { 0, 2, 3, 4, 7, 5 };
            dynamic cHair, tHair, mHair, selectHair, eHair;
            public void Run(INpcHost self, GameCharacter target)
            {
                hair.self = self;
                hair.target = target;
                selectHair = self.AskMenu("Welcome to Ludibrium's hair salon! Do you happen to have #b#t4050007##k or #b#t4051007##k? If so, how about letting me take care of your hair? Decide what you want to do with your hair...\r\n#b#L0# Change Style (VIP coupon)#l\r\n#L1# Dye your hair (VIP coupon#l");
                if (selectHair == 0)
                {
                    var styles = GetStyles(mStyles, fStyles);
                    mHair = self.AskAvatar("I can change the look of your hair completely. Ready for a change? With #b#t4050007##k, I'll take care of the rest for you. Choose the style according to your taste...", 4050007, styles);
                }
                else if (selectHair == 1)
                {
                    var cs = GetColors(colors);

                    mHair = self.AskAvatar("I can change your hair color completely. Ready for a change? With #b#t4051007##k, I'll take care of the rest. Choose a color you like!", 4051007, cs);
                }

                hair.SayHairResult(mHair, "Check it out!! What do you think? Even I think it's a work of art! HAHAHA. Call me if you want a new haircut because I'll make you look great every time!");
            }
        }
        // Ludibrium EXP Hair-Salon 
        [Script("hair_ludi2")]
        class hair_ludi2 : INpcScript
        {
            int[] mStyles = new int[]
            {
                30250,
                30190,
                30150,
                30050,
                30280,
                30240,
                30300,
                30160
            };
            int[] fStyles = new int[]
            {
                31150,
                31280,
                31160,
                31120,
                31290,
                31270,
                31030,
                31230,
                31010
            };
            int[] colors = new int[] { 0, 2, 3, 4, 7, 5 };
            dynamic mHair, selectHair, nRet1;
            public void Run(INpcHost self, GameCharacter target)
            {
                hair.self = self;
                hair.target = target;
                var cStyle = 4050006;
                var cColor = 4051006;
                selectHair = self.AskMenu("Hey, I'm the assistant here. Don't worry, I'm great at this. If you happen to have #b#t" + cStyle + "##k or #b#t"+ cColor + "##k, why not let me take care of the rest?\r\n#b#L0# Change Style (reg. coupon)#l\r\n#L2# Dye your hair (reg. coupon)#l");
                if (selectHair == 0)
                {
                    nRet1 = self.AskYesNo("If you use a regular coupon, your hairstyle will randomly change to a new look, with a chance to win a new experimental style. Are you sure you want to use #b#t"+ cStyle + "##k and change everything?");
                    if (nRet1 == 0)
                    {
                        self.Say("I understand. Think about it closely, and if you still want it, feel free to talk to me.");
                        return;
                    }
                    else if (nRet1 == 1)
                    {
                        var styles = GetStyles(mStyles, fStyles);
                        mHair = self.MakeRandAvatar(cStyle, styles);
                    }
                }
                else if (selectHair == 2)
                {
                    nRet1 = self.AskYesNo("If you use the normal coupon, your hair color will change to a random color. Are you sure you want to use #b#t"+cColor+"##k and change everything?");
                    if (nRet1 == 0)
                    {
                        self.Say("I understand. Think about it closely, and if you still want it, feel free to talk to me.");
                        return;
                    }
                    else if (nRet1 == 1)
                    {
                        var cs = GetColors(colors);
                        mHair = self.MakeRandAvatar(cColor, cs);
                    }
                }
                hair.SayHairResult(mHair, "Here's the mirror. What do you think of the new haircut? I knew it wasn't going to be the sexiest of them all, but didn't it look good? Stop by if you feel you need a change again.");
            }
        }
    }
}

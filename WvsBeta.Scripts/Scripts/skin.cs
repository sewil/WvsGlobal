using System;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class skin
    {
        static void SaySkin(string text, int couponid)
        {
            self.Say(text);

            var mHair = self.AskAvatar("With our specialized machine, you can see in advance how it will look after treatment. What kind of skin treatment would you like to do? Choose your favorite style...", couponid, 0, 1, 2, 3, 4);

            if (mHair == 1) self.Say("Here's the mirror, you can look! What do you think? Doesn't your skin look beautiful and glowing like mine? Hehehehe~ I'm sure you're enjoying it. Come back again~");
            else if (mHair == -1) self.Say("Hmm... you don't have the skincare coupon you need to get the treatment. Sorry, but we can't do it.");
            else if (mHair == -3) self.Say("I'm sorry, but it looks like our skin care machine is not working at the moment. Please come back later.");
            else if (mHair == 0 || mHair == -2) self.Say("Sorry, there seems to be a problem with the procedure. Please come back later.");
        }
        static INpcHost self;
        static GameCharacter target;
        // Henesys Skin-care
        [Script("skin_henesys1")]
        class skin_henesys1 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                skin.self = self;
                skin.target = target;
                SaySkin("Hello there! Welcome to the Henesys Clinic! Would you like to have firm, healthy-looking skin like mine? With #b#t4053000##k, you can let us take care of everything and give you the skin you've always wanted~!", 4053000);
            }
        }
        // Orbis Skin-care
        [Script("skin_orbis1")]
        class skin_orbis1 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                skin.self = self;
                skin.target = target;
                SaySkin("Hello there! Welcome to Orbis Clinic~! Would you like to have firm, healthy-looking skin like mine? With #b#t4053001##k, you can let us take care of everything and give you the skin you've always wanted~!", 4053001);
            }
        }
        // Ludibrium Skin-care
        [Script("skin_ludi1")]
        class skin_ludi1 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                skin.self = self;
                skin.target = target;
                SaySkin("Oh hi! Welcome to the Ludibrium Clinic! Interested in getting tanned and looking sexy? How about beautiful, white skin? With #b#t5153002##k, you can let us take care of everything and give you the skin you've always dreamed of~!", 4053002);
            }
        }
    }
}

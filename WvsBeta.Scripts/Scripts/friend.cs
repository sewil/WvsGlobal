using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    static class friend
    {
        public static void Expand(INpcHost self, GameCharacter target, int fee)
        {
            var nRet1 = self.AskYesNo("Hope you get as much done as yesterday... Well, hello! Don't you wanna grow your buddy list? You seem like a person who has a lot of friends... Well, what do you think? With a little money, I can make it happen. Keep in mind, though, that this only applies to one character at a time, so it won't affect any other characters on your account. Sound good?");
            bool isLudi = self.Field.ID == MapIds.Ludibrium;
            if (nRet1 == 0) self.Say("I see... Guess you don't have as many friends as I thought. Haha, just kidding! Anyway, if you change your mind, feel free to come back and we'll do business. If you make a lot of friends, then you'll know where to go... hehe...");
            else
            {
                var nRet2 = self.AskYesNo($"Alright, good thinking! It really isn't that expensive. {(isLudi ? "With a special Ludibrium discount, it'll cost " : "")}#b{fee.Culture()} mesos, and I will add 5 more slots to your buddy list#k. And no, you don't have to buy each slot individually. Once you buy them, the slots will remain on your buddy list permanently. If you are one of those that needs more space, then I can recommend this for you. So what do you think? Does {fee.Culture()} mesos sound good?");
                if (nRet2 == 0) self.Say($"I see... Guess you don't have as many friends as I thought. Or maybe you don't have {fee.Culture()} mesos with you right now? Anyway, if you change your mind, feel free to come back and we'll do business. That is, of course, if you have the financial means... hehe...");
                else
                {
                    var fRet = target.IncFriendMax(5, -fee);
                    if (fRet == 0) self.Say($"Hey... Are you sure you have #b{fee.Culture()} mesos#k?? If so, please check that you haven't extended your buddy list to the maximum. Even with the money, the maximum amount of slots you can have on your buddy list is #b{Constants.MaxBuddySlots}#k.");
                    else self.Say("Alright! You buddy list has now been expanded with 5 extra slots. Check it out for yourself. And if you need more space on your buddy list, you know who to look for. Of course, it won't be free... Well, see you later...");
                }
            }
        }
    }
    // Mr. Goldstein : 1002003
    [Script("friend00")]
    class friend00 : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            friend.Expand(self, target, 250000);
        }
    }
    // Holly : 2040046
    [Script("friend01")]
    class friend01 : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            friend.Expand(self, target, 240000);
        }
    }
}

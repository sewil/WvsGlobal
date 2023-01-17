using System.Collections.Generic;

namespace WvsBeta.Game.Scripting
{
    public interface INpcScript : IGameScript
    {
        void Run(INpcHost self, GameCharacter target, byte state, byte answer, string stringAnswer, int integerAnswer);
    }

    public interface INpcHost
    {
        int mID { get; }
        void SendNext(string Message);
        void SendBackNext(string Message);
        void SendBackOK(string Message);
        void SendOK(string Message);
        void AskMenu(string Message);
        void AskYesNo(string Message);
        void AskText(string Message, string Default, short MinLength, short MaxLength);
        void AskInteger(string Message, int Default, int MinValue, int MaxValue);
        void AskStyle(string Message, List<int> Values);
        void Stop();

        object GetSessionValue(string pName);
        void SetSessionValue(string pName, object pValue);
    }
}
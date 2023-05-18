using System.Collections.Generic;
using System.Linq;
using WvsBeta.Game.GameObjects.DataLoading;
using static WvsBeta.MasterThread;
using WvsBeta.Common;
using WvsBeta.Game.GameObjects;
using WvsBeta.Game.Events.Packets;

namespace WvsBeta.Game.Events.GMEvents
{
    class MapleOXEvent : EventFieldSet
    {
        private RepeatingAction curQuestion;
        private readonly List<QuizData> questions = new List<QuizData>(); //page, index, answer. 0 = x, 1 = o for the answer

        public MapleOXEvent(ConfigReader.Node fsNode) : base(fsNode)
        {
        }

        public override EnterStatus Enter(GameCharacter chr, int mapIdx)
        {
            string portalName = chr.IsAdmin ? "t001" : "join00";
            return Enter(chr, mapIdx, portalName);
        }
        private void Reset()
        {
            curQuestion?.Stop();
            curQuestion = null;
            Lobby.ChatEnabled = true;
            questions.Clear();
        }

        public override void Enable()
        {
            base.Enable();
            Reset();

            var page = GameDataProvider.QuizQuestions[(byte)(1 + Rand32.Next() % 7)];
            while(questions.Count < 10)
            {
                var nextQuestion = page.RandomElement();
                if (!questions.Contains(nextQuestion))
                    questions.Add(nextQuestion);
            }

            Lobby.ChatEnabled = true;
        }

        public override void Start()
        {
            base.Start();
            Lobby.ChatEnabled = false;
            AskQuestion();
        }

        private void AskQuestion()
        {
            QuizData Question = questions.Last();
            Program.MainForm.LogDebug("Asking question.... Answer: " + Question.Answer);
            questions.RemoveAt(questions.Count - 1);
            Lobby.SendPacket(OXPackets.QuizQuestion(true, Question.QuestionPage, Question.QuestionIdx));
            curQuestion = RepeatingAction.Start("Quiz - " + (questions.Count - 1) + " - question", t => CheckAnswer(Question), 30*1000, 0);
        }

        private void CheckAnswer(QuizData question)
        {
            Lobby.SendPacket(OXPackets.QuizQuestion(false, question.QuestionPage, question.QuestionIdx));
            MapArea winnerArea = Lobby.Areas[question.Answer.ToString()];
            var characters = Characters.Where(c => !c.IsAdmin);
            var losers = characters.Where(chr => !Lobby.IsCharacterInArea(chr, winnerArea)).ToList();
            losers.ForEach(c => c.ChangeMap(LoseMapId));
            BroadcastMsg(BroadcastMessageType.Notice, $"{losers.Count} people have been eliminated from the Speed OX Quiz.");

            var winners = characters.Count(); // Resolves updated number after losers are teleported out of fieldset
            if (questions.Count == 0 || winners == 0)
            {
                End();
            }
            else
            {
                Program.MainForm.LogDebug("Asking next question...");
                curQuestion = RepeatingAction.Start("Quiz - " + (questions.Count - 1) + " - answer", t => AskQuestion(), 10 * 1000, 0);
            }
        }

        public override void End()
        {
            Reset();
            EnablePortals(false);
            ChatPacket.SendBroadcastMessageToMap(Lobby, "Congratulations to the winners! The portal has now opened. Press the up arrow key at the portal to enter.", BroadcastMessageType.Notice);
            ShowTimerAll(60, () => base.End());
        }
    }
}

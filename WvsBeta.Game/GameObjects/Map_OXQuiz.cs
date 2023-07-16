using System.Collections.Generic;
using System.Linq;
using WvsBeta.Game.GameObjects.DataLoading;
using static WvsBeta.MasterThread;
using WvsBeta.Common;
using WvsBeta.Game.GameObjects;
using WvsBeta.Game.Events.Packets;
using WvsBeta.Common.Extensions;

namespace WvsBeta.Game.Events.GMEvents
{
    class Map_OXQuiz : EventMap
    {
        private RepeatingAction curQuestion;
        private List<QuizData> questions = new List<QuizData>(); //page, index, answer. 0 = x, 1 = o for the answer

        public Map_OXQuiz(int id) : base(id)
        {
            OnTimerEnd += HandleTimerEnd;
        }

        ~Map_OXQuiz()
        {
            OnTimerEnd -= HandleTimerEnd;
        }

        public override void Reset()
        {
            base.Reset();
            curQuestion?.Stop();
            curQuestion = null;
            ChatEnabled = true;
            var page = GameDataProvider.QuizQuestions[(byte)(1 + Rand32.Next() % 7)];
            questions = page.Take(10).ToList().Shuffle().ToList(); // Double ToList to create a shallow copy before shuffle
        }

        public override void Start()
        {
            base.Start();
            ChatEnabled = false;
            AskQuestion();
        }

        private void AskQuestion()
        {
            QuizData Question = questions.Last();
            Program.MainForm.LogDebug("Asking question.... Answer: " + Question.Answer);
            questions.RemoveAt(questions.Count - 1);
            SendPacket(OXPackets.QuizQuestion(true, Question.QuestionPage, Question.QuestionIdx));
            curQuestion = RepeatingAction.Start("Quiz - " + (questions.Count - 1) + " - question", t => CheckAnswer(Question), 30*1000, 0);
        }

        private void CheckAnswer(QuizData question)
        {
            SendPacket(OXPackets.QuizQuestion(false, question.QuestionPage, question.QuestionIdx));
            MapArea winnerArea = Areas[question.Answer.ToString()];
            var characters = Characters.Where(c => !c.IsAdmin);
            var losers = characters.Where(chr => !IsCharacterInArea(chr, winnerArea)).ToList();
            losers.ForEach(c => c.ChangeMap(ReturnMap));
            ChatPacket.SendBroadcastMessageToMap(this, $"{losers.Count} people have been eliminated from the Speed OX Quiz.", BroadcastMessageType.Notice);

            var winners = characters.Count(); // Resolves updated number after losers are teleported out of fieldset
            if (questions.Count == 0 || winners == 0)
            {
                ChatPacket.SendBroadcastMessageToMap(this, "Congratulations to the winners! The portal has now opened. Press the up arrow key at the portal to enter.", BroadcastMessageType.Notice);
                StartTimer(60);
            }
            else
            {
                Program.MainForm.LogDebug("Asking next question...");
                curQuestion = RepeatingAction.Start("Quiz - " + (questions.Count - 1) + " - answer", t => AskQuestion(), 10 * 1000, 0);
            }
        }

        private void HandleTimerEnd(Map map)
        {
            End();
        }
    }
}

using System.Collections.Generic;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game.Events
{
    public enum EventTeam
    {
        Maple = 0,
        Story = 1
    }
    public abstract class EventMap : Map
    {
        public static Map WinMap => GameDataProvider.Maps[109050000];
        public bool Started { get; private set; }
        protected EventMap(int id) : base(id)
        {
        }

        public virtual void Start()
        {
            Started = true;
        }
        public virtual void End()
        {
            Started = false;
            Characters.ForEach(c => c.ChangeMap(ReturnMap));
            Reset();
        }

        public void SendEffectVictory(GameCharacter chr)
        {
            chr.SendPacket(FieldEffectPacket.EffectSound("Coconut/Victory"));
            chr.SendPacket(FieldEffectPacket.EffectScreen("event/coconut/victory"));
        }

        public void SendEffectLose(GameCharacter chr)
        {
            chr.SendPacket(FieldEffectPacket.EffectSound("Coconut/Failed"));
            chr.SendPacket(FieldEffectPacket.EffectScreen("event/coconut/lose"));
        }
    }
    public abstract class TeamEventMap : EventMap
    {
        public HashSet<GameCharacter> TeamMaple { get; }
        public HashSet<GameCharacter> TeamStory { get; }
        protected TeamEventMap(int id) : base(id)
        {
            TeamMaple = new HashSet<GameCharacter>();
            TeamStory = new HashSet<GameCharacter>();
        }
        public override void AddPlayer(GameCharacter chr)
        {
            base.AddPlayer(chr);
            if (TeamMaple.Count < TeamStory.Count)
            {
                TeamMaple.Add(chr);
                chr.Team = EventTeam.Maple;
            }
            else
            {
                TeamStory.Add(chr);
                chr.Team = EventTeam.Story;
            }
        }
        public override void RemovePlayer(GameCharacter chr, bool gmhide = false)
        {
            base.RemovePlayer(chr, gmhide);
            if (chr.Team == EventTeam.Maple) TeamMaple.Remove(chr);
            else TeamStory.Remove(chr);
            chr.Team = 0;
        }
        public override void Reset()
        {
            base.Reset();
            TeamMaple.Clear();
            TeamStory.Clear();
        }
    }
}

using reNX;
using reNX.NXProperties;
using System.Collections;
using System.Collections.Generic;
using WvsBeta.Common.Enums;

namespace WvsBeta.Common.Objects
{
    public class WZQuestData
    {
        public short QuestID { get; private set; }
        public IDictionary<QuestStage, WZQuestStage> Stages { get; private set; } = new Dictionary<QuestStage, WZQuestStage>();
        public WZQuestData(NXFile pFile, NXNode checkNode)
        {
            short qid = short.Parse(checkNode.Name);
            foreach (NXNode cStage in checkNode)
            {
                NXNode aStage = pFile.ResolvePath($"Quest/Act.img/{qid}/{cStage.Name}");
                WZQuestStage stage = new WZQuestStage(this, cStage, aStage);
                Stages.Add(stage.Stage, stage);
            }
            QuestID = qid;
        }
    }
    public class WZQuestTrigger
    {
        public short QuestID { get; private set; }
        public QuestState NewState { get; private set; }
        public WZQuestTrigger(NXNode node)
        {
            NewState = (QuestState)node["state"].ValueByte();
            QuestID = node["id"].ValueInt16();
        }
    }
    public class WZQuestAct
    {
        public WZQuestStage Stage { get; private set; }
        public short NextQuest { get; private set; }
        public int Exp { get; private set; }
        public int Mesos { get; private set; }
        public List<QuestItem> Items { get; private set; } = new List<QuestItem>();

        public WZQuestAct(WZQuestStage stage, NXNode node)
        {
            Stage = stage;
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "money":
                        Mesos = subNode.ValueInt32();
                        break;
                    case "exp":
                        Exp = subNode.ValueInt32();
                        break;
                    case "item":
                        foreach (var itemNode in subNode)
                        {
                            var item = new QuestItem();
                            item.Amount = itemNode["count"].ValueInt16();
                            item.ItemID = itemNode["id"].ValueInt32();
                            Items.Add(item);
                        }
                        break;
                    case "nextQuest":
                        NextQuest = subNode.ValueInt16();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public class WZQuestCheck
    {
        public WZQuestStage Stage { get; private set; }
        public List<WZQuestTrigger> Quests { get; private set; } = new List<WZQuestTrigger>();
        public List<QuestMob> Mobs { get; private set; } = new List<QuestMob>();
        public List<QuestItem> Items { get; private set; } = new List<QuestItem>();
        public int NpcID { get; private set; }
        public short Job { get; private set; }
        public int Mesos { get; private set; }
        public int LvMin { get; private set; }
        public int LvMax { get; private set; }
        public long End { get; private set; }
        public WZQuestCheck(WZQuestStage stage, NXNode node)
        {
            Stage = stage;
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "mob":
                        foreach (var mobNode in subNode)
                        {
                            QuestMob mob = new QuestMob();
                            mob.Count = mobNode["count"].ValueInt32();
                            mob.MobID = mobNode["id"].ValueInt32();
                            Mobs.Add(mob);
                        }
                        break;
                    case "quest":
                        foreach (var qtNode in subNode)
                        {
                            var trigger = new WZQuestTrigger(qtNode);
                            Quests.Add(trigger);
                        }
                        break;
                    case "item":
                        foreach (var itemNode in subNode)
                        {
                            var item = new QuestItem();
                            item.Amount = itemNode["count"].ValueInt16();
                            item.ItemID = itemNode["id"].ValueInt32();
                            Items.Add(item);
                        }
                        break;
                    case "job":
                        Job = subNode.ValueInt16();
                        break;
                    case "npc":
                        NpcID = subNode.ValueInt32();
                        break;
                    case "lvmin":
                        LvMin = subNode.ValueInt32();
                        break;
                    case "lvmax":
                        LvMax = subNode.ValueInt32();
                        break;
                    case "money":
                        Mesos = subNode.ValueInt32();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public enum QuestStage : byte
    {
        Start = 0,
        Complete = 1
    }
    public class WZQuestStage
    {
        public QuestStage Stage { get; private set; }
        public WZQuestData Quest { get; private set; }
        public WZQuestCheck Check { get; private set; }
        public WZQuestAct Act { get; private set; }
        public WZQuestStage(WZQuestData quest, NXNode checkNode, NXNode actNode)
        {
            Stage = (QuestStage)byte.Parse(checkNode.Name);
            Quest = quest;
            Check = new WZQuestCheck(this, checkNode);
            Act = new WZQuestAct(this, actNode);
        }
    }
}

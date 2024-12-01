using reNX;
using reNX.NXProperties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using WvsBeta.Common.Enums;

namespace WvsBeta.Common.Objects
{
    public class WZQuestData
    {
        public WZQuestInfo QuestInfo { get; private set; }
        public short QuestID { get; private set; }
        public IDictionary<QuestStage, WZQuestStage> Stages { get; private set; } = new Dictionary<QuestStage, WZQuestStage>();
        public WZQuestData(NXFile pFile, NXNode checkNode)
        {
            short qid = short.Parse(checkNode.Name);
            NXNode questInfoNode = pFile.ResolvePath($"Quest/QuestInfo.img/{qid}");
            QuestInfo = new WZQuestInfo(questInfoNode);
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
        public WZQuestStage Stage { get; }
        public short NextQuest { get; }
        public uint Exp { get; }
        public int Mesos { get; }
        public short Fame { get; }
        /// <summary>
        /// Important that it's a list rather than a dictionary. Needs to be indexed correctly for item selection.
        /// </summary>
        public List<QuestItem> Items { get; }

        public WZQuestAct(WZQuestStage stage, NXNode node)
        {
            Items = new List<QuestItem>();
            Stage = stage;
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "money":
                        Mesos = subNode.ValueInt32();
                        break;
                    case "exp":
                        Exp = subNode.ValueUInt32();
                        break;
                    case "item":
                        foreach (var itemNode in subNode)
                        {
                            var item = new QuestItem(itemNode);
                            Items.Add(item);
                        }
                        break;
                    case "pop":
                        Fame = subNode.ValueInt16();
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
        public IDictionary<int, QuestMob> Mobs { get; private set; } = new Dictionary<int, QuestMob>();
        public IDictionary<int, QuestItem> Items { get; private set; } = new Dictionary<int, QuestItem>();
        public int NpcID { get; private set; }
        public short Job { get; private set; }
        public int Mesos { get; private set; }
        public int LvMin { get; private set; }
        public int LvMax { get; private set; }
        public string End { get; private set; }
        public short Fame { get; private set; }
        public int IntervalMins { get; private set; }
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
                            QuestMob questMob = new QuestMob(mobNode);
                            Mobs.Add(questMob.MobID, questMob);
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
                            var item = new QuestItem(itemNode);
                            Items.Add(item.ItemID, item);
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
                    case "end":
                        End = subNode.ValueString();
                        break;
                    case "pop":
                        Fame = subNode.ValueInt16();
                        break;
                    case "interval":
                        IntervalMins = subNode.ValueInt32();
                        break;
                    default:
                        break;
                }
            }
        }
        public bool HasExpired()
        {
            if (End == null) return false;
            // Parse end
            if (!DateTime.TryParseExact(End, "yyyyMMddHH", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime edate)) return false;
            // Compare with now
            var cdate = MasterThread.CurrentDate;
            return edate <= cdate;
        }
    }
    public class WZQuestInfo
    {
        public IDictionary<byte, string> Stages { get; private set; } = new Dictionary<byte, string>();
        public byte Area { get; private set; }
        public string Name { get; private set; }
        public byte Order { get; private set; }
        public string Parent { get; private set; }
        public WZQuestInfo(NXNode node)
        {
            foreach (var subnode in node)
            {
                switch (subnode.Name)
                {
                    case "area":
                        Area = subnode.ValueByte();
                        break;
                    case "name":
                        Name = subnode.ValueString();
                        break;
                    case "order":
                        Order = subnode.ValueByte();
                        break;
                    case "parent":
                        Parent = subnode.ValueString();
                        break;
                    default:
                        if (byte.TryParse(subnode.Name, out byte stage))
                        {
                            Stages[stage] = subnode.ValueString();
                        }
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

using System.Drawing;
using System;
using WvsBeta.Common.Enums;
using reNX.NXProperties;

namespace WvsBeta.Common.Objects
{
    public class SkillLevelData
    {
        public byte Level { get; set; }
        public byte MobCount { get; set; }
        public byte HitCount { get; set; }

        public int BuffSeconds { get; set; }
        public short Damage { get; set; }
        public short AttackRange { get; set; }
        public byte Mastery { get; set; }

        public short HPProperty { get; set; }
        public short MPProperty { get; set; }
        public short Property { get; set; }

        public short HPUsage { get; set; }
        public short MPUsage { get; set; }
        public int ItemIDUsage { get; set; }
        public short ItemAmountUsage { get; set; }
        public short BulletUsage { get; set; }
        public short MesosUsage { get; set; }

        public short XValue { get; set; }
        public short YValue { get; set; }
        public short ZValue { get; set; }

        public short Speed { get; set; }
        public short Jump { get; set; }
        public short WeaponAttack { get; set; }
        public short MagicAttack { get; set; }
        public short WeaponDefense { get; set; }
        public short MagicDefense { get; set; }
        public short Accurancy { get; set; }
        public short Avoidability { get; set; }

        public SkillElement ElementFlags { get; set; }

        public short LTX { get; set; }
        public short LTY { get; set; }
        public short RBX { get; set; }
        public short RBY { get; set; }

        public SkillLevelData(int skillID, NXNode iNode)
        {
            Level = byte.Parse(iNode.Name);

            foreach (var nxNode in iNode)
            {
                switch (nxNode.Name)
                {
                    case "hs": // help string (refer to Strings.wz)
                    case "action": // Stance
                    case "ball":
                    case "hit":
                    case "bulletConsume": break; // Avenger uses like 3 stars

                    case "x":
                        XValue = nxNode.ValueInt16();
                        break;
                    case "y":
                        YValue = nxNode.ValueInt16();
                        break;
                    case "z":
                        ZValue = nxNode.ValueInt16();
                        break;
                    case "attackCount":
                        HitCount = nxNode.ValueByte();
                        break;
                    case "mobCount":
                        MobCount = nxNode.ValueByte();
                        break;
                    case "time":
                        BuffSeconds = nxNode.ValueInt32();
                        break;
                    case "damage":
                        Damage = nxNode.ValueInt16();
                        break;
                    case "range":
                        AttackRange = nxNode.ValueInt16();
                        break;
                    case "mastery":
                        Mastery = nxNode.ValueByte();
                        break;
                    case "hp":
                        HPProperty = nxNode.ValueInt16();
                        break;
                    case "mp":
                        MPProperty = nxNode.ValueInt16();
                        break;
                    case "prop":
                        Property = nxNode.ValueInt16();
                        break;
                    case "hpCon":
                        HPUsage = nxNode.ValueInt16();
                        break;
                    case "mpCon":
                        MPUsage = nxNode.ValueInt16();
                        break;
                    case "itemCon":
                        ItemIDUsage = nxNode.ValueInt32();
                        break;
                    case "itemConNo":
                        ItemAmountUsage = nxNode.ValueInt16();
                        break;
                    case "bulletCount":
                        BulletUsage = nxNode.ValueInt16();
                        break;
                    case "moneyCon":
                        MesosUsage = nxNode.ValueInt16();
                        break;
                    case "speed":
                        Speed = nxNode.ValueInt16();
                        break;
                    case "jump":
                        Jump = nxNode.ValueInt16();
                        break;
                    case "eva":
                        Avoidability = nxNode.ValueInt16();
                        break;
                    case "acc":
                        Accurancy = nxNode.ValueInt16();
                        break;
                    case "mad":
                        MagicAttack = nxNode.ValueInt16();
                        break;
                    case "mdd":
                        MagicDefense = nxNode.ValueInt16();
                        break;
                    case "pad":
                        WeaponAttack = nxNode.ValueInt16();
                        break;
                    case "pdd":
                        WeaponDefense = nxNode.ValueInt16();
                        break;
                    case "lt":
                        {
                            Point pPoint = nxNode.ValueOrDie<Point>();
                            LTX = (short)pPoint.X;
                            LTY = (short)pPoint.Y;
                            break;
                        }
                    case "rb":
                        {
                            Point pPoint = nxNode.ValueOrDie<Point>();
                            RBX = (short)pPoint.X;
                            RBY = (short)pPoint.Y;
                            break;
                        }
                    default:
                        Console.WriteLine($"Unhandled skill level node {nxNode.Name} for id {skillID} (level {Level})");
                        break;
                }
            }
        }
    }
}

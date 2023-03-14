using reNX.NXProperties;
using System.Collections.Generic;
using WvsBeta.Game;

public class MobSkillData
{
    public byte Level { get; }
    public byte SkillID { get; }
    public short EffectAfter { get; }
    public MobSkillData(int mobId, NXNode node, out bool error)
    {
        error = false;
        SkillID = node["skill"].ValueByte();
        Level = node["level"].ValueByte();
        if (!DataProvider.MobSkills.TryGetValue(SkillID, out Dictionary<byte, MobSkillLevelData> levelDataMap) || !levelDataMap.ContainsKey(Level))
        {
            Program.MainForm.LogAppend("Unknown mob skill {0} at level {1} for mob {2}. Skipping...", SkillID, Level, mobId);
            error = true;
        }
        if (node.ContainsChild("effectAfter"))
        {
            EffectAfter = node["effectAfter"].ValueInt16();
        }
    }
}

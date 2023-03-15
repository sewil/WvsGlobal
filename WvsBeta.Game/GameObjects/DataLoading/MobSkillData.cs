using reNX.NXProperties;

public class MobSkillData
{
    public byte Level { get; }
    public byte SkillID { get; }
    public short EffectAfter { get; }
    public MobSkillData(NXNode node)
    {
        SkillID = node["skill"].ValueByte();
        Level = node["level"].ValueByte();
        if (node.ContainsChild("effectAfter"))
        {
            EffectAfter = node["effectAfter"].ValueInt16();
        }
    }
}

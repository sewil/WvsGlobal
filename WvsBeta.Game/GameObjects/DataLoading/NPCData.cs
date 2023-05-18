using System.Collections.Generic;

public class NPCData
{
    public int ID { get; set; }
    public string Quest { get; set; }
    public int Trunk { get; set; }
    public short Speed { get; set; }
    public byte SpeakLineCount { get; set; }
    public List<ShopItemData> Shop { get; set; }
    /// <summary>
    /// Whether this NPC exists in a map.
    /// </summary>
    public bool IsAccessible { get; set; }
}
namespace WvsBeta.Game
{
    public class NpcLife : LifeWrapper, IFieldObj
    {
        public Map Field { get; }
        public uint SpawnID { get; set; }

        public NpcLife(Map field, Life life) : base(life)
        {
            Field = field;
        }

        public bool IsShownTo(IFieldObj Object) => true;
    }
}
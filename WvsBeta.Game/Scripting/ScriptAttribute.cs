using System;

namespace WvsBeta.Game.Scripting
{
    public class ScriptAttribute : Attribute
    {
        public string DisplayName { get; private set; }
        public ScriptAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}

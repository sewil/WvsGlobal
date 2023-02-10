using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WvsBeta.Game.Scripting
{
    public interface IStandardScript : IGameScript
    {
        IDictionary<string, string> ScriptNameMap { get; set; }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace WvsBeta.Common
{
    public class ConfigReader
    {
        public static string DataSvrPath = Path.Combine(Environment.CurrentDirectory, "..", "..", "DataSvr");
        public class Node : IEnumerable<Node>
        {
            public List<Node> SubNodes { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public List<string> Values { get; set; } = new List<string>();

            private static IFormatProvider NumberFormat = new CultureInfo("en-US");

            public int GetInt() => int.Parse(Value, NumberFormat);
            public uint GetUInt() => uint.Parse(Value, NumberFormat);
            public short GetShort() => short.Parse(Value, NumberFormat);
            public ushort GetUShort() => ushort.Parse(Value, NumberFormat);
            public byte GetByte() => byte.Parse(Value, NumberFormat);
            public bool GetBool() => GetByte() != 0;
            public string GetString() => Value;
            public double GetDouble() => double.Parse(Value, NumberFormat);
            public T GetEnum<T>() => (T) Enum.Parse(typeof(T), Value);

            public Node this[string name]
            {
                get { return SubNodes?.Find(x => x.Name == name); }
            }

            public IEnumerator<Node> GetEnumerator()
            {
                return SubNodes.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable) SubNodes).GetEnumerator();
            }
        }

        public string Filename { get; }
        public Node RootNode { get; }

        public ConfigReader(string path)
        {
            using (var sr = new StreamReader(File.OpenRead(path)))
            {
                int row = 0;
                RootNode = ReadInnerNode("RootNode", sr, ref row, 0);
            }
        }

        public Node this[string name] => name == "" ? RootNode : RootNode[name];
        
        // Parser

        private static readonly Regex lineRegex = new Regex(@"^\s*([^ =]+)\s*=\s*([^\r\n$]*)\s*$");
        private static readonly Regex arrayLineRegex = new Regex(@"\s*([^ ,\[\]]+)\s*,?\s*");
        
        private Node ReadInnerNode(string nodeName, StreamReader sr, ref int currentLineNumber, int depth, bool isArray = false)
        {
            depth++;
            string line = "";

            var node = new Node
            {
                Name = nodeName,
                SubNodes = new List<Node>(),
                Value = null,
            };

            while (!sr.EndOfStream)
            {
                currentLineNumber++;
                line = sr.ReadLine().Trim();
                if (line == "" || line.StartsWith("#") || line.StartsWith("/")) continue;

                if (line == "}" || line == "]")
                {
                    // End of block
                    break;
                }

                if (line.Contains(" # "))
                {
                    line = line.Substring(0, line.IndexOf(" # "));
                }

                if (isArray)
                {
                    var matches = arrayLineRegex.Matches(line);
                    if (matches.Count > 0)
                    {
                        var values = matches.Cast<Match>().Select(i => i.Groups[1].Value);
                        node.Values.AddRange(values);
                    }
                }
                else
                {
                    var matches = lineRegex.Match(line);
                    if (!matches.Success) throw new Exception("Error on line " + currentLineNumber + " in node " + node.Name);
                    var name = matches.Groups[1].Captures[0].Value;
                    var value = matches.Groups[2].Captures[0].Value;

                    if (value == "{" || value == "[")
                    {
                        if (isArray && value == "[") throw new Exception("Can't open new array inside array statement!");
                        var subNode = ReadInnerNode(name, sr, ref currentLineNumber, depth, value == "[");
                        node.SubNodes.Add(subNode);
                    }
                    else
                    {
                        node.SubNodes.Add(new Node
                        {
                            Name = name,
                            SubNodes = null,
                            Value = value,
                        });
                    }
                }
            }

            if (depth > 1 && line != "}" && line != "]")
            {
                throw new Exception("Missing ending brace.");
            }
            return node;
        }
    }
}

using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace WvsBeta.Common
{
    public class StringPool
    {
        private readonly Dictionary<int, string> values = new Dictionary<int, string>();
        public string this[int key] => values[key];
        public static readonly StringPool Instance = new StringPool();
        private StringPool()
        {
            using (TextFieldParser parser = new TextFieldParser("StringPool.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();
                    int id = int.Parse(fields[0]);
                    string text = fields[1];
                    values.Add(id, text);
                }
            }
        }
    }
}

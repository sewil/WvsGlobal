using reNX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using reNX.NXProperties;

namespace WvsBeta.DropDataWriter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NXFile nx = new NXFile(@"..\DataSvr\DropData.nx");
            XmlWriterSettings settings = new XmlWriterSettings();
            foreach (var imgnode in nx.BaseNode)
            {
                using (XmlWriter writer = XmlWriter.Create(@"DropData\" + imgnode.Name + ".xml"))
                {
                    writer.WriteStartElement("imgdir");
                    writer.WriteAttributeString("name", imgnode.Name);
                    foreach (var subnode in imgnode)
                    {
                        writer.WriteStartElement("imgdir");
                        writer.WriteAttributeString("name", subnode.Name);
                        foreach (var subsubnode in subnode)
                        {
                            writer.WriteStartElement("imgdir");
                            writer.WriteAttributeString("name", subsubnode.Name);
                            foreach (var subsubsubnode in subsubnode)
                            {
                                string value = subsubsubnode.ValueString();
                                if (int.TryParse(value, out int _))
                                {
                                    writer.WriteStartElement("int");
                                }
                                else
                                {
                                    writer.WriteStartElement("string");
                                }
                                writer.WriteAttributeString("name", subsubsubnode.Name);
                                writer.WriteAttributeString("value", value);
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
            }
        }
    }
}

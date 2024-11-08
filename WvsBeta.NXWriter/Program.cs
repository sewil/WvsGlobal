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
using System.Runtime.InteropServices;
using System.Drawing;
using System.Globalization;

namespace WvsBeta.NXWriter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) throw new ArgumentException("Missing file name argument!");
            string fileName = args[0];
            string basePath = $@"..\ClientBin\exports\{fileName}";
            NXFile nx = new NXFile($@"..\ClientBin\exports\{fileName}.nx");
            XmlWriterSettings settings = new XmlWriterSettings();
            Directory.CreateDirectory(basePath);
            foreach (var node in nx.BaseNode)
            {
                WriteNode(basePath, node);
            }
        }
        static void WriteNode(string path, NXNode node)
        {
            if (node.Name.EndsWith(".img"))
            {
                WriteImg(path, node);
            }
            else
            {
                string nPath = Path.Combine(path, node.Name);
                Directory.CreateDirectory(nPath);
                foreach (var childNode in node)
                {
                    WriteNode(nPath, childNode);
                }
            }
        }
        class XMlutf8Encoding : UTF8Encoding
        {
            public override string WebName => "UTF-8";
        }
        static void WriteImg(string path, NXNode node)
        {
            var encoding = new XMlutf8Encoding();
            var settings = new XmlWriterSettings() { Indent = true, Encoding = encoding, IndentChars = "" };
            using (var fileStream = new FileStream($@"{path}\{node.Name}.xml", FileMode.Create))
            using (var memoryStream = new MemoryStream())
            using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
            {
                writer.WriteStartDocument(true);
                WriteImgNode(writer, node);
                writer.WriteEndDocument();
                writer.Flush();
                string stringXml = Encoding.UTF8.GetString(memoryStream.ToArray())
                    .Replace(" />", "/>")
                    .Replace("'", "&apos;") + "\r\n";
                byte[] byteArray = Encoding.UTF8.GetBytes(stringXml);
                MemoryStream formattedStream = new MemoryStream(byteArray);
                formattedStream.CopyTo(fileStream);
                fileStream.Flush();
                formattedStream.Flush();
            }
        }
        static void WriteImgNode(XmlWriter writer, NXNode node)
        {
            if (node is NXValuedNode<String>)
            {
                writer.WriteStartElement("string");
                writer.WriteAttributeString("name", node.Name);
                writer.WriteAttributeString("value", node.ValueString());
            }
            else if (node is NXValuedNode<Double>)
            {
                writer.WriteStartElement("float");
                writer.WriteAttributeString("name", node.Name);
                writer.WriteAttributeString("value", node.ValueDouble().ToString("N1", CultureInfo.InvariantCulture));
            }
            else if (node is NXValuedNode<Int64>)
            {
                writer.WriteStartElement("int");
                writer.WriteAttributeString("name", node.Name);
                writer.WriteAttributeString("value", node.ValueString());
            }
            else if (node is NXLazyValuedNode<byte[]>) // Audio
            {
                writer.WriteStartElement("sound");
                writer.WriteAttributeString("name", node.Name);
            }
            else if (node is NXValuedNode<System.Drawing.Point> p)
            {
                writer.WriteStartElement("vector");
                writer.WriteAttributeString("name", node.Name);
                writer.WriteAttributeString("x", p.Value.X.ToString());
                writer.WriteAttributeString("y", p.Value.Y.ToString());
            }
            else if (node.GetType() == typeof(NXBitmapNode))
            {
                var b = (NXBitmapNode)node;
                writer.WriteStartElement("canvas");
                writer.WriteAttributeString("name", node.Name);
                if (b.Value != null)
                {
                    writer.WriteAttributeString("width", b.Value.Width.ToString());
                    writer.WriteAttributeString("height", b.Value.Height.ToString());
                }
            }
            else
            {
                writer.WriteStartElement("imgdir");
                writer.WriteAttributeString("name", node.Name);
            }
            foreach (var childNode in node)
            {
                WriteImgNode(writer, childNode);
            }
            writer.WriteEndElement();
        }
    }
}

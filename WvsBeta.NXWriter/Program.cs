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
using System.Reflection;

namespace WvsBeta.NXWriter
{
    internal class Program
    {
        static bool orderNodes;
        static void Main(string[] args)
        {
            if (args.Length < 2) throw new ArgumentException("Missing file name argument!");
            string filein = Path.GetFullPath(args[0]);
            if (args.Length >= 3)
            {
                orderNodes = args[2] == "true";
            }
            string fileName = new FileInfo(filein).Name;
            string dirout = Path.GetFullPath(Path.Combine(args[1], fileName));
            Console.WriteLine($"Reading from {filein}. Writing to {dirout}. Node ordering {orderNodes}.");
            NXFile nx = new NXFile(filein);
            XmlWriterSettings settings = new XmlWriterSettings();
            Directory.CreateDirectory(dirout);
            foreach (var node in nx.BaseNode)
            {
                WriteNode(dirout, node);
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
        static void WriteImg(string path, NXNode node)
        {
            var settings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
            var xmlPath = $@"{path}\{node.Name}.xml";
            Console.WriteLine(xmlPath);
            using (XmlWriter writer = XmlWriter.Create(xmlPath, settings))
            {
                writer.WriteStartDocument(true);
                WriteImgNode(writer, node);
                writer.WriteEndDocument();
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
            var childNodes = orderNodes ? node
                .OrderBy(i => i.Name)
                .OrderBy(i => int.TryParse(i.Name, out int n) ? n : 0)
                .ToList() : node.ToList();
            foreach (var childNode in childNodes)
            {
                WriteImgNode(writer, childNode);
            }
            writer.WriteEndElement();
        }
    }
}

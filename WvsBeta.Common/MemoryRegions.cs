using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

namespace WvsBeta.Common
{
    public class MemoryRegion
    {
        public uint Address { get; }
        public byte[] Data { get; }

        public int Length => Data.Length;

        public MemoryRegion(uint address, byte[] data)
        {
            Address = address;
            Data = data;
        }
    }

    public class FileRegion
    {
        public string Name { get; }
        public uint StartAddress { get; }
        public uint FileOffset { get; }
        public FileRegion(string name, uint startAddress, uint fileOffset)
        {
            Name = name;
            StartAddress = startAddress;
            FileOffset = fileOffset;
        }
    }

    public class MemoryRegions
    {
        private static ILog log = LogManager.GetLogger("MemoryRegions");

        private static MemoryRegions _instance;

        public static MemoryRegions Instance => _instance ?? Init();

        public static MemoryRegions Init()
        {
            if (_instance != null) return _instance;
            _instance = new MemoryRegions();
            _instance.LoadRegions();
            return _instance;
        }

        public List<FileRegion> FileRegions = new List<FileRegion>();
        public List<MemoryRegion> Regions = new List<MemoryRegion>();

        public int MaxRandomMemoryOffset { get; private set; } = 0;

        public void LoadRegions()
        {
            try
            {
                string binPath = Path.Combine("..", "ClientBin", Config.Instance.ClientBinaryName);
                if (!File.Exists(binPath))
                {
                    throw new Exception("Not loading any regions; cannot find " + Config.Instance.ClientBinaryName);
                }

                if (!File.Exists("MemoryRegions.tsv"))
                {
                    throw new Exception("Unable to load MemoryRegions.tsv");
                }

                using (var mapleFileStream = File.Open(binPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var mapleReader = new BinaryReader(mapleFileStream))
                using (var configReader = new StreamReader(File.OpenRead("MemoryRegions.tsv"), Encoding.ASCII))
                {
                    // Skip to the file regions
                    FileRegions.Clear();
                    Regions.Clear();
                    while (true)
                    {
                        string line = ReadNextLine(configReader);
                        if (line == null) break;
                        if (line == "START_FILE_REGIONS")
                        {
                            FileRegions = ParseFileRegions(configReader);
                        }
                        else if (line == "START_MEMORY_REGIONS")
                        {
                            Regions = ParseMemoryRegions(configReader, mapleReader, mapleFileStream);
                        }
                    }

                    var regionSizes = Regions.Select(x => x.Length).ToList();
                    int minimumSize = regionSizes.Count > 0 ? regionSizes.Min() : 0;
                    MaxRandomMemoryOffset = Math.Max(0, minimumSize - 1); // Make sure we do not go negative
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception while loading MemoryRegions:", ex);
            }
        }

        private byte[] GetMemoryRegionData(uint address, int length, BinaryReader mapleReader, FileStream mapleFileStream)
        {
            uint lastStartAddress = 0;
            uint lastOffset = 0;
            foreach (var region in FileRegions)
            {
                // Find the region this address is in.
                if (region.StartAddress < address && region.StartAddress > lastStartAddress)
                {
                    lastStartAddress = region.StartAddress;
                    lastOffset = region.FileOffset;
                }
            }

            mapleFileStream.Seek((address - lastStartAddress) + lastOffset, SeekOrigin.Begin);
            return mapleReader.ReadBytes(length);
        }

        private List<FileRegion> ParseFileRegions(StreamReader reader)
        {
            var fileRegions = new List<FileRegion>();
            while (true)
            {
                string line = ReadNextLine(reader);
                if (line == "END_FILE_REGIONS" || line == null) break;
                var parsedLine = ParseFileRegionLine(line);
                if (parsedLine != null)
                    fileRegions.Add(parsedLine);
            }
            return fileRegions;
        }

        private List<MemoryRegion> ParseMemoryRegions(StreamReader reader, BinaryReader mapleReader, FileStream mapleFileStream)
        {
            var memoryRegions = new List<MemoryRegion>();
            while(true)
            {
                string line = ReadNextLine(reader);
                if (line == "END_MEMORY_REGIONS" || line == null) break;
                var memoryRegion = ParseMemoryRegionLine(line, mapleReader, mapleFileStream);
                if (memoryRegion != null)
                    memoryRegions.Add(memoryRegion);
            }
            return memoryRegions;
        }

        private FileRegion ParseFileRegionLine(string line)
        {
            if (line == null) return null;

            var elements = line.Split('\t');
            if (elements.Length < 3) return null;

            var name = elements[0];
            var startAddress = uint.Parse(elements[1], NumberStyles.HexNumber);
            var fileOffset = uint.Parse(elements[2], NumberStyles.HexNumber);
            return new FileRegion(name, startAddress, fileOffset);
        }

        private MemoryRegion ParseMemoryRegionLine(string line, BinaryReader mapleReader, FileStream mapleFileStream)
        {
            if (line == null) return null;

            var elements = line.Split('\t');
            if (elements.Length < 2) return null;

            var address = uint.Parse(elements[0], NumberStyles.HexNumber);
            var length = int.Parse(elements[1], NumberStyles.Integer);
            var data = GetMemoryRegionData(address, length, mapleReader, mapleFileStream);
            return new MemoryRegion(address, data);
        }

        private string ReadNextLine(StreamReader reader)
        {
            if (reader.EndOfStream) return null;
            string line = null;
            while (!reader.EndOfStream && line == null)
            {
                var tmp = reader.ReadLine();
                if (tmp.Length == 0 || tmp[0] == '#')
                {
                    continue;
                }

                line = tmp;
            }

            return line;
        }
    }
}

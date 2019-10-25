using System.IO;
using System.Linq;

namespace SabfExtract
{
    //Maybe some day this will be a proper implementation...
    public class Uasset
    {
        public uint Magic { get; set; }
        public int Version { get; set; }
        public byte[] Unk1 { get; set; }
        public uint HeaderSize { get; set; }
        public string none { get; set; }
        public byte[] unk2 { get; set; }

        private byte[] data;

        //public uint num_strings { get; set; }
        //public uint offset_strings { get; set; }
        //public uint num_assets { get; set; }
        //public uint offset_assets { get; set; }
        //public uint num_refs { get; set; }
        //public uint offset_res { get; set; }


        public Uasset(Stream stream)
        {
            using BinaryReader reader = new BinaryReader(stream);
            Magic = reader.ReadUInt32();
            Version = reader.ReadInt32();
            Unk1 = reader.ReadBytes(16);
            HeaderSize = reader.ReadUInt32();
            //reader.BaseStream.Position += 4;
            //unk2 = reader.ReadBytes(4);

            reader.BaseStream.Position = 0;
            
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            data = ms.ToArray();

            //num_strings = reader.ReadUInt32();
            //offset_strings = reader.ReadUInt32();
            //num_assets = reader.ReadUInt32();
            //offset_assets = reader.ReadUInt32();
            //num_refs = reader.ReadUInt32();
            //offset_res = reader.ReadUInt32();
        }

        public byte[] GetAsset()
        {
            //Remove weird uasset bytes at the end of the file
            return data.Skip((int)HeaderSize).Take((int)(data.Length - HeaderSize - 4)).ToArray();
        }
    }
}

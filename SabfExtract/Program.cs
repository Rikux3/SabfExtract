using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SabfExtract
{
    class Program
    {
        private static readonly byte[] SABFMAGIC = { 0x73, 0x61, 0x62, 0x66 };
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var arg1 = args[0];
                if (File.GetAttributes(arg1).HasFlag(FileAttributes.Directory))
                {
                    foreach (var file in Directory.GetFiles(arg1))
                        Extract(file);
                }
                else
                {
                    Extract(arg1);
                }
            }
            else
                Console.WriteLine("No argument specified. Exiting...");
        }

        private static void Extract(string fileName)
        {
            try
            {
                Console.WriteLine($"Opening file {fileName}");
                var assetData = new Uasset(File.OpenRead(fileName)).GetAsset();
                var sabfInstances = FindPattern(assetData, SABFMAGIC).ToList();
                if (sabfInstances.Count > 1)
                {
                    Console.WriteLine("More than one instance of sabf found! Aborting...");
                    return;
                }
                var outName = $"{Path.GetFileName(fileName)}.sab";
                Console.WriteLine($"Saving new file {outName}");
                File.WriteAllBytes(outName, assetData.Skip(sabfInstances[0]).ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static IEnumerable<int> FindPattern(byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    yield return i;
                }
            }
        }
    }
}

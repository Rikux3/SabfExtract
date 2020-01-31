using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SabfExtract
{
    class Program
    {
        private static readonly byte[] SABFMAGIC = { 0x73, 0x61, 0x62, 0x66 };
        private static readonly byte[] MABFMAGIC = { 0x6d, 0x61, 0x62, 0x66 };
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
                var fileIn = File.ReadAllBytes(fileName);
                var assetData = fileIn.Take(fileIn.Length - 4).ToArray();

                Console.WriteLine("Scanning for sabf header...");

                var sabfInstances = HasPattern(assetData, SABFMAGIC);
                if (sabfInstances.Item1)
                {
                    if (sabfInstances.Item2.Count > 1)
                    {
                        Console.WriteLine("More than one instance of sabf found!");
                    }
                    else
                    {
                        var outName = $"{Path.GetFileName(fileName)}.sab";
                        Console.WriteLine($"Saving new file {outName}");
                        File.WriteAllBytes(outName, assetData.Skip(sabfInstances.Item2[0]).ToArray());
                    }
                }
                else
                {
                    Console.WriteLine("No sabf header found!");
                }

                Console.WriteLine("Scanning for mabf header...");

                var mabfInstances = HasPattern(assetData, MABFMAGIC);
                if (mabfInstances.Item1)
                {
                    if (mabfInstances.Item2.Count > 1)
                    {
                        Console.WriteLine("More than one instance of mabf found!");
                    }
                    else
                    {
                        var outName = $"{Path.GetFileName(fileName)}.mab";
                        Console.WriteLine($"Saving new file {outName}");
                        File.WriteAllBytes(outName, assetData.Skip(mabfInstances.Item2[0]).ToArray());
                    }
                }
                else
                {
                    Console.WriteLine("No mabf header found!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static (bool, List<int>) HasPattern(byte[] source, byte[] pattern)
        {
            var entries = FindPattern(source, pattern).ToList();
            return (entries.Count > 0) ? (true, entries) : (false, entries);
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
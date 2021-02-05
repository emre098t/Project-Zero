using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Helpers;
using System.IO;

namespace Digital_World.Database
{
    /// <summary>
    /// Parses MonsterList.bin
    /// </summary>
    public class MapMonsterList
    {
        public static Dictionary<uint, MDBMonsters> Monsters = new Dictionary<uint, MDBMonsters>();

        public static void Load(string fileName)
        {
            if (Monsters.Count > 0) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {

                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        MDBMonsters entry = new MDBMonsters();
                        entry.i2 = read.ReadInt();
                        entry.Id = read.ReadUInt();

                        entry.Name = read.ReadZString(Encoding.Unicode, 128);
                        entry.Desc = read.ReadZString(Encoding.Unicode, 1024);
                        int counter = read.ReadInt();
                        entry.uInts = new int[counter];
                        for (int j = 0; j < counter; j++)
                            entry.uInts[j] = read.ReadInt();
                        Monsters.Add(entry.Id, entry);
                    }
                }
            }
            
            Console.WriteLine("[MapMonsterList] Loaded {0} Monsters.", Monsters.Count);
        }
    }
    /// <summary>
    /// Monster Map data loaded from MonsterList.bin
    /// </summary>
    public class MDBMonsters
    {
        public int i2;
        /// <summary>
        /// Entry Id
        /// </summary>
        public uint Id;
        /// <summary>
        /// Entry name
        /// </summary>
        public string Name;
        /// <summary>
        /// Description of the entry
        /// </summary>
        public string Desc;
        /// <summary>
        /// An array of ints. Purpose unknown.
        /// </summary>
        public int[] uInts;
    }
}

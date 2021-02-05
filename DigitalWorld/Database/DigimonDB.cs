using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Helpers;
using System.IO;
using Digital_World.Entities;

namespace Digital_World.Database
{
    public class DigimonDB
    {
        public static Dictionary<int, DigimonData> Digimon = new Dictionary<int, DigimonData>();
        public static void Load(string fileName)
        {
            if (Digimon.Count > 0) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    //436 - > FOR UPDATED DMO
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        read.Seek(4 + i * 572);

                        DigimonData digiData = new DigimonData();
                        digiData.Species = read.ReadInt();
                        digiData.Model = read.ReadInt();
                        //digiData.DisplayName = read.ReadZString(Encoding.Unicode);
                        //Console.WriteLine(digiData.DisplayName);
                        read.Seek(4 + 136 + i * 572);
                        digiData.Name = read.ReadZString(Encoding.ASCII);
                        //Console.WriteLine(digiData.Name);
                        read.Seek(4 + 372 + i * 572); //240 + 4 = 244 -> NEW BIN
                        digiData.HP = read.ReadShort();
                        //Console.WriteLine(digiData.HP);
                        digiData.DS = read.ReadShort();
                        //Console.WriteLine(digiData.DS);
                        digiData.DE = read.ReadShort();
                        //Console.WriteLine(digiData.DE);
                        digiData.EV = read.ReadShort();
                        //Console.WriteLine(digiData.EV);
                        digiData.MS = read.ReadShort();
                        //Console.WriteLine(digiData.MS);
                        digiData.CR = read.ReadShort();
                        //Console.WriteLine(digiData.CR);
                        digiData.AT = read.ReadShort();
                        //Console.WriteLine(digiData.AT);
                        digiData.AS = read.ReadShort();
                        //Console.WriteLine(digiData.AS);
                        digiData.AR = read.ReadShort();
                        //Console.WriteLine(digiData.uStat);
                        digiData.HT = read.ReadShort();
                        //Console.WriteLine(digiData.HT);
                        digiData.uShort1 = read.ReadShort();
                       // Console.WriteLine(digiData.uShort1);
                        digiData.Skill1 = read.ReadShort();
                        //Console.WriteLine(digiData.Skill1);
                        digiData.Skill2 = read.ReadShort();
                        //Console.WriteLine(digiData.Skill2);
                        digiData.Skill3 = read.ReadShort();
                        digiData.BL = 0;
                            //Console.WriteLine(digiData.Skill3);

                        //Console.WriteLine("---------------------------");

                        Digimon.Add(digiData.Species, digiData);

                        using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(@"D:\DMODB\GDMODigiDB.txt", true))
                        {
                            file.WriteLine(digiData.Species + " | " + digiData.Model + " | " + digiData.DisplayName + " | " + digiData.Name + " | " + digiData.HP + " | " + digiData.DS + " | " + digiData.DE + " | " + digiData.AS + " | " + digiData.MS + " | " + digiData.AT + " | " + digiData.CR + " | " + digiData.EV + " | " + digiData.AR + " | ");
                        }


                    }
                }
            }
            Console.WriteLine("[DigimonDB] Loaded {0} digimon.", Digimon.Count);

            
        }

        public static DigimonData GetDigimon(int Species)
        {
            if (Digimon.ContainsKey(Species))
                return Digimon[Species];
            else
                return null;
        }

        public static List<int> GetSpecies(string Name)
        {
            List<int> species = new List<int>();
            foreach (KeyValuePair<int, DigimonData> kvp in Digimon)
            {
                DigimonData dData = kvp.Value;
                if (dData.DisplayName.Contains(Name) || dData.Name.Contains(Name))
                    species.Add(dData.Species);
            }
            return species;
        }
    }

    public class DigimonData
    {
        public int Species, Model;
        public string Name;
        public string DisplayName;
        public short HP, DS, DE, AS, MS, CR, AT, EV, uStat, HT, AR, uShort1, BL;
        public short Skill1, Skill2, Skill3, Skill4;

        public DigimonData() { }

        public DigimonStats Stats(short Size)
        {
            //TODO: Get Stats
            return null;
        }

        public DigimonStats Default(Character Tamer, int Sync, int Size)
        {
            DigimonStats Stats = new DigimonStats();

            Stats.MaxHP = (short)(Math.Min(Math.Floor((decimal)HP * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.HP * (Sync / 100)), short.MaxValue));
            Stats.HP = (short)(Math.Min(Math.Floor((decimal)HP * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.HP * (Sync / 100)), short.MaxValue));
            Stats.MaxDS = (short)(Math.Min(Math.Floor((decimal)DS * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.DS * (Sync / 100)), short.MaxValue));
            Stats.DS = (short)(Math.Max(Math.Floor((decimal)DS * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.DS * (Sync / 100)), short.MaxValue));

            Stats.DE = (short)(Math.Min(Math.Floor((decimal)DE * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.DE * (Sync / 100)), short.MaxValue));
            Stats.MS = (short)(Math.Min(Math.Floor((decimal)MS * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.MS * (Sync / 100)), short.MaxValue));
            Stats.CR = (short)(Math.Min(Math.Floor((decimal)CR * ((ushort)Size / 10000)), short.MaxValue));
            Stats.AT = (short)(Math.Min(Math.Floor((decimal)AT * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.AT * (Sync / 100)), short.MaxValue));
            Stats.EV = EV;
            Stats.uStat = uStat;
            Stats.HT = HT;

            Stats.Intimacy = (short)Sync;
            return Stats;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", DisplayName, Species);
        }
    }
}

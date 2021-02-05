using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Helpers;
using System.IO;
using Digital_World.Entities;

namespace Digital_World.Database
{
    public class CharCreateDB
    {
        public static Dictionary<int, CharCreate> Chars = new Dictionary<int, CharCreate>();
        public static Dictionary<int, CharCreateDigimons> Digimons = new Dictionary<int, CharCreateDigimons>();

        public static void Load(string filename)
        {

            if (Chars.Count > 0)
            {
                return;
            }

            using (Stream s = File.OpenRead(filename))
            {
                using (BitReader read = new BitReader(s))
                {

                    //PREMIUM_SILK

                    int count = read.ReadInt();

                    for (int i = 0; i < count; i++)
                    {

                        //Console.WriteLine(i);
                        CharCreate chars = new CharCreate();

                        chars.TamerModel = read.ReadInt();
                        chars.EnabledInClient = read.ReadByte();
                        chars.EnabledToCreate = read.ReadByte();
                        chars.unk1 = read.ReadInt();
                        chars.unk2 = read.ReadInt();
                        chars.unk3 = read.ReadInt();
                        chars.CountDressingClothes = read.ReadInt();
                        for (int j = 0; j < chars.CountDressingClothes; j++)
                        {
                            DressingClothes cloth = new DressingClothes();
                            cloth.DressingCloth = read.ReadInt();
                        }
                        Chars.Add(chars.TamerModel, chars);
                    }
                    
                    

                    /*
                     * ADD SPECIAL FOR LOOP FOR PACKAGES - > PREMIUM_SILK
                     * 
                     * 
                     */


                    ////-> NORMAL SILK
                    /*
                    int counter = count * 2;

                    for (int i = count; i < counter + 1; i++)
                    {

                        //Console.WriteLine(i);
                        CharCreateDigimons dms = new CharCreateDigimons();

                        read.Seek(12 + i * 2412);

                        Digimons.Add(dms.unique_id, dms);
                    }

                    /*
                    * ADD SPECIAL FOR LOOP FOR PACKAGES - > SILK
                    * 
                    * 
                    */
                    
                    

                }

            }

            Console.WriteLine("[CharCreateDB] Loaded {0} , {1} entries.", Chars.Count, Digimons.Count);
        }


        public static CharCreate getID(int TamerModel)
        {
            if (Chars.ContainsKey(TamerModel))
                return Chars[TamerModel];
            else
                return null;


        }

        public static CharCreateDigimons getID2(int unique_id)
        {
            if (Digimons.ContainsKey(unique_id))
                return Digimons[unique_id];
            else
                return null;


        }

        public class DressingClothes
        {
            public int DressingCloth;
        }


    }

    public class CharCreate
    {
        public int TamerModel, EnabledInClient, EnabledToCreate, unk1, unk2, unk3, CountDressingClothes = 0;

        public CharCreate() { }

    }

    public class CharCreateDigimons
    {
        public int amount, id, unique_id, id_2, id_3 = 0;

        public CharCreateDigimons() { }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Helpers;
using System.IO;
using Digital_World.Entities;

namespace Digital_World.Database
{
    public class CashShopDB
    {
        public static Dictionary<int, CASHINFO> CashShop = new Dictionary<int, CASHINFO>();

        public static void Load(string filename)
        {

            using (Stream s = File.OpenRead(filename))
            {
                using(BitReader read = new BitReader(s)){

                    //PREMIUM_SILK

                    int count = 313;

                    for (int i = 0; i < count; i++)
                    {

                        //Console.WriteLine(i);
                        CASHINFO m = new CASHINFO();

                        read.Seek(8 + i * 2412);
                        m.unique_id = read.ReadInt(); // GET UNIQUE ID
                        Console.WriteLine(m.unique_id); 
                        read.Seek(6 + 170 + i * 2412);
                        m.id = read.ReadInt(); //GET ITEMID
                        Console.WriteLine(m.id);
                        m.amount = read.ReadInt(); //GET AMOUNT
                        Console.WriteLine(m.amount);

                        Console.WriteLine("-----------------");

                        CashShop.Add(m.unique_id, m);
                    }

                    /*
                     * ADD SPECIAL FOR LOOP FOR PACKAGES - > PREMIUM_SILK
                     * 
                     * 
                     */


                    ////-> NORMAL SILK

                    int counter = count * 2;

                    for (int i = count; i < counter+1; i++)
                    {

                        //Console.WriteLine(i);
                        CASHINFO m = new CASHINFO();

                        read.Seek(12 + i * 2412);
                        m.unique_id = read.ReadInt(); // GET UNIQUE ID
                        Console.WriteLine(m.unique_id);
                        read.Seek(10 + 170 + i * 2412);
                        m.id = read.ReadInt(); //GET ITEMID
                        Console.WriteLine(m.id);
                        m.amount = read.ReadInt(); //GET AMOUNT
                        Console.WriteLine(m.amount);

                        Console.WriteLine("-----------------");

                        CashShop.Add(m.unique_id,m);
                    }

                    /*
                    * ADD SPECIAL FOR LOOP FOR PACKAGES - > SILK
                    * 
                    * 
                    */


                }

            }

            Console.WriteLine("[CashShopDB] Loaded {0} entries.", CashShop.Count);
        }


        public static CASHINFO getID(int unique_id)
        {
            if (CashShop.ContainsKey(unique_id))
                return CashShop[unique_id];
            else
                return null;

            
        }

    }



    public class CASHINFO
    {
        public int amount, id, unique_id,id_2,id_3 = 0;

        public CASHINFO() { }

    }

}

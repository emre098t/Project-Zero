using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Helpers;
using System.IO;

namespace Digital_World.Database
{
    public class ItemDB
    {
        public static Dictionary<int, ItemData> Items = new Dictionary<int, ItemData>();
        
        public static void Load(string fileName)
        {

            if (Items.Count > 0) return;
            using (BitReader read = new BitReader(File.OpenRead(fileName)))
            {

         
                int count = read.ReadInt();
                //int count = 2;
                for (int i = 0; i < count; i++)
                {
                    read.Seek(4 + i * 1596);

                    ItemData iData = new ItemData();
                    iData.ItemId = read.ReadInt(); // ->
                    //Console.WriteLine(iData.itemId);
                    iData.Name = read.ReadZString(Encoding.Unicode); // -> 
                    //Console.WriteLine(iData.Name);
                    read.Seek(4 + 132 + i * 1596);
                    iData.uInt1 = read.ReadInt(); // - >
                    //Console.WriteLine(iData.uInt1);
                    iData.Desc = read.ReadZString(Encoding.Unicode); // ->
                    //Console.WriteLine(iData.Desc);
                    read.Seek(4 + 1160 + i * 1596);

                    iData.Icon = read.ReadZString(Encoding.ASCII);
                    //Console.WriteLine(iData.Icon);
                    read.Seek(4 + 1224 + i * 1596); //520 584
                    iData.ItemType = read.ReadShort();
                    //Console.WriteLine(iData.ItemType);
                    iData.Kind = read.ReadZString(Encoding.Unicode);
                    //Console.WriteLine(iData.Kind);

                    //-> Trying to see if this works

                    read.Seek(4 + 1354 + i * 1596);

                    for (int j = 0; j < iData.uShorts1.Length; j++)
                    {
                        iData.uShorts1[j] = read.ReadShort();
                        //Console.WriteLine(iData.uShorts1[j]);
                    }
                    iData.Stack = read.ReadShort();
                    //Console.WriteLine("Stack: " + iData.Stack);
                    for (int j = 0; j < iData.uShorts2.Length; j++)
                    {
                        iData.uShorts2[j] = read.ReadShort();
                        //Console.WriteLine(iData.uShorts2[j]);
                    }
                    iData.Buy = read.ReadInt();
                    //Console.WriteLine("Buy :" + iData.Buy);
                    iData.Sell = read.ReadInt();
                    //Console.WriteLine("Sell :" + iData.Sell);
                    Items.Add(iData.ItemId, iData);
                    
                    /*using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"D:\DMODB\GDMOITEMDB.txt", true))
                    {
                        file.WriteLine("{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} | {8} | {9}", iData.ItemId, iData.Name, iData.uInt1, iData.Desc, iData.Icon, iData.ItemType, iData.Kind, iData.Stack, iData.Buy, iData.Sell);
                    }*/


                    //full_id            //name             //short_id           //type
                    //Console.WriteLine(iData.ItemId + " | " + iData.Name + " | " + iData.Icon + " | " + iData.Desc + " | " + iData.itemId);
                    //Console.WriteLine(" ");
                    //Console.WriteLine("----------------------------------------");
                    //Console.WriteLine(" ");

                    //SqlDB.InsertItems(iData.ItemId,iData.Name,iData.itemId,iData.Kind);

                }
            }
            Console.WriteLine("[ItemDB] Loaded {0} items.", Items.Count);
        }

        public static ItemData GetItem(int fullId)
        {
            ItemData iData = null;
            foreach (KeyValuePair<int, ItemData> kvp in Items)
            {
                if (kvp.Value.ItemId == fullId)
                {
                    iData = kvp.Value;
                    break;
                }
            }
            return iData;
        }

        public static ItemData GetItem(ushort shortId)
        {
            ItemData iData = null;
            foreach (KeyValuePair<int, ItemData> kvp in Items)
            {
                if (kvp.Value.itemId == shortId)
                {
                    iData = kvp.Value;
                    break;
                }
            }
            return iData;
        }
    }

    public class ItemData
    {
        
        public ushort itemId;
        public ushort Mod;
        public string Name;
        public int uInt1;
        public string Desc;
        public string Icon;
        public short ItemType;
        public string Kind;
        public short Stack;
        public short[] uShorts1; //8
        public short[] uShorts2; //12
        public int Buy, Sell;

        public ItemData()
        {
            //TOTAL 8 + 12 + STACK = 23 - > VER155
            uShorts1 = new short[8];
            uShorts2 = new short[14];
        }

        public int ItemId
        {
            get
            {
                return (Mod << 16) + itemId;
            }
            set
            {
                Mod = (ushort)(value >> 16);
                itemId = (ushort)(value & 0xffff);
            }
        }

    }
}

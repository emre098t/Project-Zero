using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Helpers;
using System.IO;

namespace Digital_World.Database
{
    public class QuestDB
    {
        public static Dictionary<int, QuestData> Quests = new Dictionary<int, QuestData>();
        
        public static void Load(string fileName)
        {

            if (Quests.Count > 0) return;
            using (BitReader read = new BitReader(File.OpenRead(fileName)))
            {

         
                int count = read.ReadInt();
                //int count = 2;
                for (int i = 0; i < count; i++)
                {
                    QuestData qdata = new QuestData();
                    //Quests.Add(qdata.QuestID, qdata);
                    
                    /*using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"F:\DMODB\DMWITEMDB.txt", true))
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
            Console.WriteLine("[QuestsDB] Loaded {0} quests.", Quests.Count);
        }
    }

    public class QuestData
    {
        
        public ushort QuestID;



    }
}

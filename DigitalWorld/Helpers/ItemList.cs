﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Entities;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Digital_World.Helpers
{
    [Serializable]
    public class ItemList
    {
        private Item[] items;

        public ItemList(int max)
        {
            items = new Item[max];
            for (int i = 0; i < items.Length; i++)
                items[i] = new Item(0);
        }

        public int Count
        {
            get
            {
                int i = 0;
                for (int j = 0; j < items.Length; j++)
                {
                    if (items[j].ItemId != 0)
                        i++;
                }
                return i;
            }
        }

        public int FindSlot(int itemId)
        {
            for (int i = 0; i < items.Length; i++)
                if (items[i].ItemId == itemId)
                    return i;
            return -1;
        }

        public int FindSlot_v2(int ID, int slot)
        {
            int temp = 0;

            for (int i = 0; i < items.Length; i++)
            {

                if (i != slot && items[i].ID == ID)
                {
                    temp = i;
                }
                    
                    
            }

            return temp;
        }

        public Item Find(short itemId)
        {
            for (int i = 0; i < items.Length; i++)
                if (items[i].ItemId == itemId)
                    return items[i];
            return null;
        }

        public int EquipSlot(short slotId)
        {
            int slot = 0;
            switch (slotId)
            {
                case 5000:
                    {
                        slot= 21;
                        break;
                    }
                case 1000:
                case 1001:
                case 1002:
                case 1003:
                case 1004:
                case 1005:
                case 1006:
                    slot = slotId - 1000;
                    break;
                case 4000:
                    slot = 9;
                    break;
                default:
                    {
                        break;
                    }
            }
            return slot;
        }

        /// <summary>
        /// Gets or sets the item at idx
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Item this[int idx]
        {
            get
            {
                return items[idx];
            }
            set
            {
                items[idx] = value;
            }
        }

        /// <summary>
        /// Adds item i to an open slot.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int Add(Item i)
        {
            int slot = GetOpenSlot();
            if (slot == -1)
                return -1;
            else
            {
                items[slot] = i;
                return slot;
            }
        }

        public bool Remove(Item i)
        {
            int slot = FindSlot(i.ItemId);
            if (slot != -1)
            {
                items[slot] = new Item();
                return true;
            }
            else
                return false;
        }

        public bool Remove(int Slot)
        {
            if (Slot != -1)
            {
                items[Slot] = new Item();
                return true;
            }
            else
                return false;
        }

        public bool Contains(int itemId)
        {
            if (FindSlot(itemId) == -1)
                return false;
            return true;
        }

        public int Count_times(int ID)
        {
            int temp = 0;

            for (int i = 0; i < items.Length; i++)
            {
                if(items[i].ID==ID)
                    temp++;

            }
            return temp;
        }

        public bool Contains(Item i)
        {
            return Contains(i.ItemId);
        }

        private int GetOpenSlot()
        {
            for (int i = 0; i < items.Length; i++)
                if (items[i].ItemId == 0)
                    return i;
            return -1;
        }

        public byte[] Serialize()
        {
            byte[] b = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(m, this);
                b = m.ToArray();
            }
            return b;
        }

        public static ItemList Deserialize(byte[] buffer)
        {
            ItemList itemList = null;
            using (MemoryStream m = new MemoryStream(buffer))
            {
                BinaryFormatter f = new BinaryFormatter();
                itemList = (ItemList)f.Deserialize(m);
            }
            return itemList;
        }

        public byte[] LoadCashWH()
        {
            byte[] buffer = null;

            using (MemoryStream m = new MemoryStream())
            {
                for (int i = 0; i < items.Length; i++)
                {
                    m.Write(items[i].loadCashWH(),0,56);
                }

                buffer = m.ToArray();
            }

            return buffer;
        }
        public byte[] ToArrayEquipment()
        {
            byte[] buffer = null;
            using (MemoryStream m = new MemoryStream())
            {
                for (int i = 0; i < items.Length; i++)
                {
                    m.Write(items[i].ToArray(), 0, 20); //28
                }

                //Console.WriteLine("ItemsLength {0}", items.Length);
                buffer = m.ToArray();
                //Console.WriteLine("BUFFER {0}", buffer);
            }
            return buffer;
        }
        public byte[] ToArrayChipSet()
        {
            byte[] buffer = null;
            using (MemoryStream m = new MemoryStream())
            {
                for (int i = 0; i < items.Length; i++)
                {
                    m.Write(items[i].ToArray(), 0, 28); //28
                }

                Console.WriteLine("ItemsLength {0}", items.Length);
                buffer = m.ToArray();
                Console.WriteLine("BUFFER {0}", buffer);
            }
            return buffer;
        }
        public byte[] ToArray()
        {
            byte[] buffer = null;
            using (MemoryStream m = new MemoryStream())
            {
                for (int i = 0; i < items.Length; i++)
                {
                    m.Write(items[i].ToArray(), 0, 68);
                }
                
                //Console.WriteLine("ItemsLength {0}", items.Length);
                buffer = m.ToArray();
                //Console.WriteLine("BUFFER {0}", buffer);
            }
            return buffer;
        }
    }
}

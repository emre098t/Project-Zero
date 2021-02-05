﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Helpers;

namespace Digital_World.Entities
{
    public enum CharacterModel : int
    {
        NULL = -1,
        Masaru  = 80001,
        Tohma,
        Yoshino,
        Ikuto,
    }

    /// <summary>
    /// Character Class
    /// </summary>
    public class Character
    {
        public uint AccountId = 0;
        public uint CharacterId = 0;
        public int CharacterPos = 0;
        public string CharTime;
        public int Starter = 0;
        public uint intHandle = 0;
        public ItemList Equipment = new ItemList(13);
        public int Level = 1;
        public CharacterModel Model = CharacterModel.NULL;
        public int CharModel;
        public string Name = string.Empty;
        public int lastChar = 0;
        public int Money = 0;

        public int MaxHP = 0;
        public int MaxDS = 0;
        public int HP = 0;
        public int DS = 0;
        public int AT = 0;
        public int DE = 0;
        public int EXP = 0;
        public int MS = 0;
        public int Fatigue = 0;

        public int InventorySize = 30;
        public int WarehouseSize = 21;
        public int ArchiveSize = 1;
        public ItemList Inventory = new ItemList(150);
        public ItemList Warehouse = new ItemList(245);
        public Position Location = new Position();
        public CharPosition CharPosition = new CharPosition();
        public ItemList TempCashVault = new ItemList(7);
        public ItemList ChipSets = new ItemList(12);
        public ItemList Digivice = new ItemList(1);



        /// <summary>
        /// The current egg in the incubator
        /// </summary>
        public int Incubator = 0;
        /// <summary>
        /// The level of the egg in the incubator
        /// </summary>
        public int IncubatorLevel = 0;

        /// <summary>
        /// A list of digiIds in the Archive
        /// </summary>
        public uint[] ArchivedDigimon = new uint[40];
        public Digimon[] DigimonList = new Digimon[5];
        public QuestList Quests;

        public bool Riding = false;
        public int RidingInt = 0;

        /// <summary>
        /// The current active Digimon
        /// </summary>
        public Digimon Partner
        {
            get
            {
                return DigimonList[0];
            }
            set
            {
                DigimonList[0] = value;
            }
        }

        public Character() {
            Equipment = new ItemList(13); //28
            Inventory = new ItemList(150);
            Warehouse = new ItemList(245);
            Quests = new QuestList();
            ArchivedDigimon = new uint[40];
            DigimonList = new Digimon[5];
            TempCashVault = new ItemList(7);
        }

        public Character(uint AcctId, string charName, int charModel)
        {
            AccountId = AcctId;
            Name = charName;
            Model = (CharacterModel)charModel;
            Equipment = new ItemList(13);
            Inventory = new ItemList(150);
            Warehouse = new ItemList(245);
            Quests = new QuestList();
            ArchivedDigimon = new uint[40];
            DigimonList = new Digimon[5];
            TempCashVault = new ItemList(7);
        }

        public override string ToString()
        {
            return string.Format("Tamer: Lv {1} {0}", Name, Level);
        }

        public override bool Equals(object obj)
        {
            if (typeof(Character) != obj.GetType())
            {
                return base.Equals(obj);
            }
            else
            {
                return (obj as Character).AccountId == this.AccountId;
            }
        }

        public uint ProperModel
        {
            get
            {
                uint iModel = 0x9C40A0;
                iModel += (((uint)Model - 80001) * 128);
                return (iModel << 8);
            }
        }

        /// <summary>
        /// Handle to the Tamer's entity
        /// </summary>
        public short TamerHandle
        {
            get
            {
                byte[] b = new byte[] { (byte)((intHandle >> 32) & 0xFF), 0x20 };
                return BitConverter.ToInt16(b, 0);
            }
        }


        public short BTamerHandle
        {

            get
            {

                byte[] b = new byte[] { (byte)((intHandle >> 32) & 0xFF),0x00 };
                return BitConverter.ToInt16(b, 0);
            }
        }



        /// <summary>
        /// Handle to the Digimon's entity
        /// </summary>
        public short DigimonHandle = 0;
    }
}

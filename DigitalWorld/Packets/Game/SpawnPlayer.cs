﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Entities;

namespace Digital_World.Packets.Game.Interface
{
    public class SpawnPlayer : Packet
    {
        public SpawnPlayer(Character Tamer, Digimon Partner)
        {
            packet.Type(1006);
            packet.WriteShort(513);
            packet.WriteByte(0);
            packet.WriteInt(Partner.Location.PosX);
            packet.WriteInt(Partner.Location.PosY);
            
            //Start Tamer Structure
            packet.WriteUInt(Partner.Model);
            packet.WriteInt(Partner.Location.PosX);
            packet.WriteInt(Partner.Location.PosY);
            packet.WriteString(Partner.Name);
            packet.WriteShort(Partner.Size);
            packet.WriteByte((byte)Partner.Level);
            packet.WriteUInt(Partner.Model);    //Related to riding mode
            packet.WriteShort(Partner.Stats.MS);

            packet.WriteShort(Tamer.TamerHandle);
            packet.WriteByte(0xff);
            packet.WriteInt(0);
            packet.WriteInt(Tamer.Location.PosX);
            packet.WriteInt(Tamer.Location.PosY);
            packet.WriteUInt(Tamer.intHandle);
            packet.WriteInt(Tamer.Location.PosX);
            packet.WriteInt(Tamer.Location.PosY);
            packet.WriteString(Tamer.Name);
            packet.WriteByte((byte)Tamer.Level);
            packet.WriteUInt(Tamer.intHandle);
            packet.WriteShort((short)Tamer.MS);

            packet.WriteByte(0xff);
            for (int i = 0; i < 13; i++)
                packet.WriteBytes(Tamer.Equipment[i].ToArray());

            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteShort(Tamer.DigimonHandle);
            packet.WriteByte(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteByte(0);
            //packet.WriteShort(Tamer.DigimonHandle);
            //packet.WriteInt(Partner.Location.PosX);
            //packet.WriteInt(Partner.Location.PosY);
            //packet.WriteByte(0);
        }

        /// <summary>
        /// Spawns a Tamer
        /// </summary>
        /// <param name="Tamer"></param>
        public SpawnPlayer(Character Tamer)
        {
            packet.Type(1006);
            packet.WriteShort(259);
            packet.WriteByte(0);

            packet.WriteInt(Tamer.Location.PosX);
            packet.WriteInt(Tamer.Location.PosY);
            packet.WriteUInt(Tamer.intHandle);
            packet.WriteInt(Tamer.Location.PosX);
            packet.WriteInt(Tamer.Location.PosY);

            packet.WriteString(Tamer.Name);
            packet.WriteByte((byte)Tamer.Level);
            packet.WriteUInt(0);
            packet.WriteShort((short)Tamer.MS);
            packet.WriteByte(0xff);
            for (int i = 0; i < 13; i++)
                packet.WriteBytes(Tamer.Equipment.ToArray());

            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteShort(Tamer.DigimonHandle);
            packet.WriteInt(0);
            packet.WriteInt(0);
        }

        /// <summary>
        /// Spawns a Partner Digimon
        /// </summary>
        /// <param name="Partner"></param>
        /// <param name="hTamer"></param>
        public SpawnPlayer(Digimon Partner, short hTamer)
        {
            packet.Type(1006);
            packet.WriteShort(259);
            packet.WriteByte(0);

            packet.WriteInt(Partner.Location.PosX);
            packet.WriteInt(Partner.Location.PosY);
            packet.WriteUInt(Partner.Model);
            packet.WriteInt(Partner.Location.PosX);
            packet.WriteInt(Partner.Location.PosY);

            packet.WriteString(Partner.Name);
            packet.WriteShort(Partner.Size);
            packet.WriteByte((byte)Partner.Level);
            packet.WriteUInt(0);
            packet.WriteShort((short)Partner.Stats.MS);
            packet.WriteShort(hTamer);
            packet.WriteByte(0xff);

            packet.WriteInt(0);
            packet.WriteShort(0);
        }
    }
}

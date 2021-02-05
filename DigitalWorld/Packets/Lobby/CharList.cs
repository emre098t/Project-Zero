using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Entities;

namespace Digital_World.Packets.Lobby
{
    public class CharList : Packet, IPacket
    {
        public CharList(List<Character> listTamers)
        {
            packet.Type(0x0515);

            //Console.WriteLine(packet.ToString());

            byte iChar = 0;
            int pos = 4;
            foreach (Character Tamer in listTamers)
            {
                packet.WriteByte((byte)Tamer.CharacterPos); //packet.WriteByte(iChar++);
                packet.WriteShort((short)Tamer.Location.Map);
                packet.WriteInt((int)Tamer.Model);
                packet.WriteByte((byte)Tamer.Level);
                packet.WriteString(Tamer.Name);
                for (int i = 0; i < 13; i++)
                {
                    Item item = Tamer.Equipment[i];
                    packet.WriteBytes(item.ToArray());
                }
                packet.WriteInt(Tamer.Partner.Species);
                packet.WriteByte((byte)Tamer.Partner.Level);
                packet.WriteString(Tamer.Partner.Name);
                if (Tamer.Name == "Lazarevic")
                {
                    packet.WriteByte(5);
                    packet.WriteByte(5);
                    packet.WriteByte(5);
                    packet.WriteByte(5);
                    packet.WriteByte(5);
                    packet.WriteByte(5);
                    packet.WriteByte(5);
                    packet.WriteByte(5);
                }
                else
                {
                    packet.WriteByte(1);
                    packet.WriteByte(0);
                    packet.WriteByte(0);
                    packet.WriteByte(0);
                    packet.WriteByte(0);
                    packet.WriteByte(0);
                    packet.WriteByte(0);
                    packet.WriteByte(0);
                }
                //Console.WriteLine("{0}|{1}|{2}", Tamer.Name, Tamer.CharacterId, Tamer.CharacterPos);
            }
            packet.WriteByte(0x63);
        }
    }
}

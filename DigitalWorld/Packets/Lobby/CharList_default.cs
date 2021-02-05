using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Entities;

namespace Digital_World.Packets.Lobby
{
    public class CharList_default : Packet, IPacket
    {
        public CharList_default(List<Character> listTamers)
        {
            packet.Type(0x0515);

            //Console.WriteLine(packet.ToString());

            byte iChar = 0;
            int pos = 4;
            foreach (Character Tamer in listTamers)
            {
                packet.WriteByte(iChar++);
                packet.WriteShort((short)Tamer.Location.Map);
                packet.WriteInt((int)Tamer.Model);
                packet.WriteByte((byte)Tamer.Level);
                packet.WriteString(Tamer.Name);
                for (int i = 0; i < 14; i++)
                {
                    Item item = Tamer.Equipment[i];
                    packet.WriteBytes(item.ToArray());
                }
                packet.WriteInt(Tamer.Partner.Species);
                packet.WriteByte((byte)Tamer.Partner.Level);
                packet.WriteString(Tamer.Partner.Name);
                packet.WriteByte(0);
                packet.WriteByte(1);
                packet.WriteShort(0);
                packet.WriteShort(0);
                packet.WriteShort(0);
                packet.WriteShort(0);
                packet.WriteByte(0x03);
                packet.WriteByte(0);
            }
            packet.WriteByte(0x63);
        }
    }
}

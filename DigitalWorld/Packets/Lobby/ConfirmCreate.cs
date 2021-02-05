using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digital_World.Packets.Lobby
{
    public class ConfirmCreate:Packet
    {
        public ConfirmCreate(Client client, int position, int model, int digimodel, string name, string diginame)
        {
            packet.Type(0x051A);
            packet.WriteByte((byte)client.CreateTamerHandshake);
            packet.WriteByte(0x2F);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteByte((byte)client.CreateDigimonHandshake);
            packet.WriteByte(0x55);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteByte((byte)position);
            packet.WriteByte(0x3);
            packet.WriteByte(0);
            packet.WriteInt(model);
            packet.WriteByte(0x1);
            packet.WriteString(name);
            for (int i = 0; i < 884; i++)
            {
                packet.WriteByte(0);
            }
            packet.WriteInt(digimodel);
            packet.WriteByte(0x1);
            packet.WriteString(diginame);
            packet.WriteByte(1);
            packet.WriteByte(0);
            packet.WriteByte(0);
            packet.WriteByte(0);
            packet.WriteByte(0);
            packet.WriteByte(0);
            packet.WriteByte(0);
            packet.WriteByte(0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digital_World.Packets.Auth
{
    public class ServerIP:Packet,IPacket
    {
        public ServerIP(string IP, uint Port, uint AccountID, uint UniqueID)
        {
            packet.Type(901);
            packet.WriteUInt(AccountID);
            packet.WriteUInt(UniqueID);
            packet.WriteString(IP);
            packet.WriteUInt(Port);
            
        }
    }

    public class ServerIPP : Packet, IPacket
    {
        public ServerIPP()
        {
            packet.WriteShort(0x06A6);
            packet.WriteInt(0);
            packet.WriteInt(2);
            /*
            packet.Type(901);
            packet.WriteUInt(AccountID);
            packet.WriteUInt(UniqueID);
            packet.WriteString(IP);
            packet.WriteUInt(Port);*/
        }
    }
}

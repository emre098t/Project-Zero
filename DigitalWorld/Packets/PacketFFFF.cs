using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digital_World.Packets
{
    public class PacketFFFF:Packet
    {
        public PacketFFFF(short time)
        {

            /*
             *    HANDSHAKE FFFF
             *    
             *    // 0x8e, 0xd8
             * 
             */


            packet.Type(0xFFFF);
            packet.WriteShort((short)time);
        }
    }
    public class PacketFFFF_Auth : Packet
    {
        public PacketFFFF_Auth(short time)
        {

            /*
             *    HANDSHAKE FFFF
             *    
             *    // 0x8e, 0xd8
             * 
             */


            packet.Type(65535); //65535
            packet.WriteShort((short)time);
            packet.WriteShort(0);
            packet.WriteShort(0x02); //0x02
            packet.WriteShort(0);
        }
    }

    public class PacketFFFF_Lobby : Packet
    {
        public PacketFFFF_Lobby()
        {

            /*
             *    HANDSHAKE FFFF
             *    
             *    // 0x8e, 0xd8
             * 
             */


            packet.Type(0xFFFF);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
        }
    }
}

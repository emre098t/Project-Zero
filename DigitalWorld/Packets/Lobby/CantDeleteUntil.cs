using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digital_World.Packets.Lobby
{
    public class CantDeleteUntil:Packet
    {
        public CantDeleteUntil(int result, string datetime)
        {
            packet.Type(1304);
            packet.WriteInt(result);
            packet.WriteString(datetime + " GMT +9");
        }
    }
}

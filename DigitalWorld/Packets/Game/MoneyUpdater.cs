using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Entities;

namespace Digital_World.Packets.Game
{
    public class MoneyUpdater : Packet
    {
        public MoneyUpdater(short handle, int amount)
        {
            packet.Type(3911);
            packet.WriteShort(handle);//Tamer handle 
            packet.WriteInt(amount);//amount to give
        }

    }
}
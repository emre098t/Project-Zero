using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Digital_World.Packets.Auth
{
    public class ServerList:Packet, IPacket
    {
        public ServerList(Dictionary<int, string> servers, string user, int characters)
        {
            packet.Type(0x06A5);
            packet.WriteByte((byte)servers.Count);
            foreach(KeyValuePair<int, string> server in servers)
            {
                packet.WriteInt(server.Key); // - > PORT
                packet.WriteString(server.Value);// - > IP
                packet.WriteByte((byte)SqlDB.ServerMaintenance(server.Value)); // ->  // Maintenience 1 = yes | 0 = no
                packet.WriteByte((byte)SqlDB.ServerLoad(server.Value)); // ->  Server Load 0 = low | 1 = mid | 2 = full
                packet.WriteByte((byte)characters); //Characters - > Count
                packet.WriteByte((byte)SqlDB.ServerNewOrNo(server.Value));
            }
        }
    }
}

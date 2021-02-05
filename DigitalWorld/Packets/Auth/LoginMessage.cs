using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digital_World.Packets.Auth
{
    /// <summary>
    /// The login message shown by Joymax
    /// </summary>
    public class LoginMessage : Packet,IPacket
    {
        public LoginMessage(int message)
        {
            packet.Type(0x0CE5);
            packet.WriteByte((byte)message);
            packet.WriteByte(0x27);
            packet.WriteByte(0);
            packet.WriteByte(0);
            packet.WriteByte(0);
        }
    }

    public class LoginMessage2 : Packet, IPacket
    {
        public LoginMessage2(string message)
        {
            packet.Type(0x0CE5);
            packet.WriteString(message);
        }
    }
    public class LoginTEST : Packet, IPacket
    {
        public LoginTEST()
        {
            packet.Type(0x06B1);
            packet.WriteShort(0x0);
            packet.WriteShort(0x07);
            packet.WriteShort(0x05);
            packet.WriteShort(0x02);
            packet.WriteShort(0x03);
            packet.WriteShort(0x04);
            packet.WriteInt(0x01);
            packet.WriteShort(0x09);
            packet.WriteShort(0x06);

        }
    }

    public class LoginTo : Packet, IPacket
    {
        public LoginTo(byte wheretogo)
        {
            packet.Type((0x0CE5));
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteByte(wheretogo);

        }
    }
    public class LoginTo2 : Packet, IPacket
    {
        public LoginTo2(byte wheretogo)
        {
            packet.Type((0x0CE5));
            packet.WriteByte(0x48);
            packet.WriteByte(0x27);
            packet.WriteByte(0x0);
            packet.WriteByte(0x0);
            packet.WriteByte(0x0);
            packet.WriteByte(0x0);
            packet.WriteByte(wheretogo);

        }
    }

    public class SecondPassTest : Packet, IPacket
    {
        public SecondPassTest(int test)
        {
            packet.Type(9801);
            packet.WriteInt(test);

        }
    }
}

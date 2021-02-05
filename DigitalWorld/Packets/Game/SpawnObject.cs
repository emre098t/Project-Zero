using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digital_World.Packets.Game
{
    public class SpawnObject :Packet
    {
        public SpawnObject(short hObject, int oX, int oY, int X1, int Y1,uint Model, int X2, int Y2, short hDigimon, int dX, int dY)
        {
            packet.Type(1006);

            //04 01
            packet.WriteShort(260);
            packet.WriteByte(0);

            packet.WriteShort(hObject);
            packet.WriteInt(oX);
            packet.WriteInt(oY);

            //03 01
            packet.WriteShort(259);
            packet.WriteByte(0);
            packet.WriteInt(X1);
            packet.WriteInt(Y1);
            packet.WriteUInt(Model);
            packet.WriteInt(X2);
            packet.WriteInt(Y2);
            packet.WriteInt(2815);
            packet.WriteShort(0);
            //05 01
            packet.WriteShort(261);
            packet.WriteByte(0);
            packet.WriteShort(hDigimon);
            packet.WriteInt(dX);
            packet.WriteInt(dY);

            packet.WriteByte(0);
        }

        public SpawnObject(uint Model, int X, int Y)
        {
            packet.Type(1006);

            packet.WriteShort(259);
            packet.WriteByte(0);
            packet.WriteInt(X);
            packet.WriteInt(Y);
            packet.WriteUInt(Model);
            packet.WriteInt(X);
            packet.WriteInt(Y);
            packet.WriteByte(0xff);
            packet.WriteShort(1); // 99 level
            packet.WriteShort(0);
            packet.WriteShort(0);
            //packet.WriteByte(0);

            
        }

        //TESTING
        public SpawnObject(short digimon)
        {

            packet.Type(1006);
            packet.WriteShort(269);
            packet.WriteByte(0);
            packet.WriteShort(digimon);
            packet.WriteShort(0);
            packet.WriteByte(0);
            packet.WriteShort(144);
        }

        public SpawnObject(short digi, int X, int Y)
        {
            packet.Type(1006);
            packet.WriteShort(261);
            packet.WriteByte(0);
            packet.WriteInt(X);
            packet.WriteInt(Y);
            packet.WriteShort(0);
            packet.WriteShort(28622); //unknown
            packet.WriteShort(0);
            packet.WriteShort(269);
            packet.WriteByte(0);
            packet.WriteShort(digi);
            packet.WriteShort(0);
            packet.WriteByte(0);
            packet.WriteShort(144);
        }

        public SpawnObject()
        {
            packet.Type(1006);

            /*byte[] rawdata = { 0x03,0x01,0x00,0x3D,0x70,0x00,0x00,0x45,0x90,0x00,0x00,0x2B,
            0x10,0x90,0x3C,0x3D,0x70,0x00,0x00,0x45,0x90,0x00,0x00,0x08,0x45,0x78,0x63,0x61,
            0x64,0x6F,0x72,0x69,0x00,0x25,0x2E,0x51,0xFA,0x4B,0x10,0xC0,0x09,0x03,0x14,0x20,
            0xFF,0x00,0x00,0x00,0x00,0x16,0x00,0x0C,0x00,0x00,0x00,0x09,0x00,0x00,0x00,0x00, 
            0x00,0x00,0x00,0x00 };
            */

            byte[] rawdata = {0x0D,0x02,0x00,0x3B,0x10,0x00,0x00,0x00,0x90,0xBF,0x40,0x00,
            0x00,0x00,0x90,0x00};

            packet.WriteBytes(rawdata);

        }
    }
}

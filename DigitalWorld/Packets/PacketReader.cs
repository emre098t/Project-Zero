﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Digital_World.Packets
{
    public class PacketReader : IDisposable
    {
        private MemoryStream packet;
        private int length = 0;
        private int type = 0;
        private int longtype = 0;
        private int checksum = 0;

        public PacketReader(byte[] buffer)
        {
            packet = new MemoryStream(buffer);
           
            length = ReadShort();
            type = ReadShort();
            packet.Seek(length - 2,SeekOrigin.Begin);
            checksum = ReadShort();

            //Console.WriteLine("{0} | {1} | {2}", length, type, longtype);

            //Console.WriteLine(checksum);

            if (checksum != (length ^ 6716))
            {
                throw new Exception("Invalid packet checksum");
            }
            else if (buffer.Length > length)
            {
                Console.WriteLine(Packet.Visualize(buffer));
                Console.WriteLine("WARNING: This packet may contain other data.");
            }
            packet.Seek(4, SeekOrigin.Begin);
        }

        /// <summary>
        /// Jump to absolute position
        /// </summary>
        /// <param name="position"></param>
        public void Seek(long position)
        {
            packet.Seek(position,SeekOrigin.Begin);
        }

        /// <summary>
        /// Skip bytes bytes
        /// </summary>
        /// <param name="bytes">Number of bytes to skip</param>
        public void Skip(long bytes)
        {
            packet.Seek(bytes, SeekOrigin.Current);
        }

        public int ReadInt()
        {
            byte[] buffer = new byte[4];
            packet.Read(buffer, 0, 4);
            return BitConverter.ToInt32(buffer, 0);
        }

        public int ReadScan()
        {
            byte[] buffer = new byte[15];
            packet.Read(buffer, 0, 15);
            for(int i=0; i < 15; i++)
            Console.WriteLine(buffer[i]);
           
            return BitConverter.ToInt32(buffer, 0);

        }

        public byte ReadByte()
        {
            byte[] buffer = new byte[1];
            packet.Read(buffer, 0, 1);
            return buffer[0];
        }

        public short ReadShort()
        {
            byte[] buffer = new byte[2];
            packet.Read(buffer, 0, 2);
            return BitConverter.ToInt16(buffer, 0);
        }

        public float ReadFloat()
        {
            byte[] buffer = new byte[4];
            packet.Read(buffer, 0, 4);
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Reads a string with preceeding length
        /// </summary>
        /// <returns>String read from the packet</returns>
        public string ReadString()
        {
            int len = packet.ReadByte();
            byte[] buffer = new byte[len];
            packet.Read(buffer, 0, len);
            return Encoding.ASCII.GetString(buffer).Trim();
        }

        public string ReadStringByLength(int len)
        {
            byte[] buffer = new byte[len];
            packet.Read(buffer, 0, len);
            return Encoding.ASCII.GetString(buffer).Trim();
        }

        /// <summary>
        /// Reads a null-terminated string
        /// </summary>
        /// <returns>String read from the packet</returns>
        public string ReadZString()
        {
            StringBuilder sb = new StringBuilder();
            while (packet.CanRead)
            {
                int data = packet.ReadByte();
                if (data == 0)
                    break;
                sb.Append((char)data);
            }
            return sb.ToString();
        }

        public byte[] ReadBytes(int len)
        {
            byte[] buffer = new byte[len];
            packet.Read(buffer, 0, len);
            return buffer;
        }

        public uint ReadUInt()
        {
            byte[] buffer = new byte[4];
            packet.Read(buffer, 0, 4);
            return BitConverter.ToUInt32(buffer, 0);
        } 

        public int Length
        {
            get
            {
                return length;
            }
        }

        public int LongType
        { get { return longtype; } }

        public int Type
        { get { return type; } }

        public int Checksum
        {
            get
            {
                return checksum;
            }
        }

        /// <summary>
        /// Visualizes the packet for human reading
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Packet.Visualize(ToArray());
        }

        public byte[] ToArray()
        {
            return packet.ToArray();
        }

        public void Dispose()
        {
            packet.Close();
            packet.Dispose();
        }
    }
}

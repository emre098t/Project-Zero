using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Digital_World.Packets;
using Digital_World.Packets.Game;
using System.Windows.Documents;

namespace Digital_World
{
    public class PacketWriter : IDisposable
    {
        MemoryStream m_stream;
        byte[] m_buffer;
        public PacketWriter()
        {
            m_stream = new MemoryStream();
            m_stream.Write(new byte[] { 0, 0 }, 0, 2);
        }

        public void WriteInt(int value)
        {
            m_stream.Write(BitConverter.GetBytes(value), 0, 4);
            
        }
        public void WriteInt(int value, int pos)
        {
            m_stream.Seek(pos, SeekOrigin.Begin);
            m_stream.Write(BitConverter.GetBytes(value), 0, 4);

        }

        public void WriteByte(byte value)
        {
            m_stream.Write(BitConverter.GetBytes(value), 0, 1);
        }

        public void WriteShort(short value)
        {
            m_stream.Write(BitConverter.GetBytes(value), 0, 2);
        }

        public void WriteShort(short value, int pos)
        {
            m_stream.Seek(pos, SeekOrigin.Begin);
            m_stream.Write(BitConverter.GetBytes(value), 0, 2);
        }
        public void WriteUShort(ushort value)
        {
            m_stream.Write(BitConverter.GetBytes(value), 0, 2);
        }

        /// <summary>
        /// Write a null terminated string to the buffer
        /// </summary>
        /// <param name="value">String to write</param>
        public void WriteString(string value)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(value);
            m_stream.WriteByte((byte)buffer.Length);
            m_stream.Write(buffer, 0, buffer.Length);
            m_stream.WriteByte(0);
        }

        public void WriteString(string value, int pos)
        {
            m_stream.Seek(pos,SeekOrigin.Begin);
            byte[] buffer = Encoding.ASCII.GetBytes(value);
            m_stream.WriteByte((byte)value.Length);
            m_stream.Write(buffer,0,buffer.Length);
            m_stream.WriteByte(0);
        }

        public void WriteBytes(byte[] buffer)
        {
            m_stream.Write(buffer, 0, buffer.Length);
        }

        public void Type(int type)
        {
            m_stream.Write(BitConverter.GetBytes(type), 0, 2);
        }

        public void Checksum(short checksum)
        {
            m_stream.Write(BitConverter.GetBytes(checksum), 0, 2);
        }

        public void WriteFloat(float value)
        {
            m_stream.Write(BitConverter.GetBytes(value), 0, 2);
        }

        public void WriteUInt(uint value)
        {
            m_stream.Write(BitConverter.GetBytes(value), 0, 4);
        }
        public void WriteUInt(uint value, int pos)
        {
            m_stream.Seek(pos,SeekOrigin.Begin);
            m_stream.Write(BitConverter.GetBytes(value), 0, 4);
        }
        public void WriteByte(uint value, int pos)
        {
            m_stream.Seek(pos, SeekOrigin.Begin);
            m_stream.Write(BitConverter.GetBytes(value), 0, 1);
        }

        public byte[] Finalize()
        {
            if (m_buffer == null)
            {
                this.WriteShort(0);

                byte[] buffer = m_stream.ToArray();
                //Console.WriteLine("TEST {0}", buffer);

                byte b1 = buffer[2];
                byte b2 = buffer[3];

                int packetid = b1 + b2;

                Console.WriteLine(packetid);

                byte[] len;
                byte[] checksum;

                len = BitConverter.GetBytes((short)buffer.Length);
                checksum = BitConverter.GetBytes((short)(buffer.Length ^ 6716));
                len.CopyTo(buffer, 0);
                checksum.CopyTo(buffer, buffer.Length - 2);


                m_stream.Close();
                m_buffer = buffer;
            }
            return m_buffer;
        }


        public byte[] FinalizeCharInfo()
        {
            if (m_buffer == null)
            {
                this.WriteShort(0);

                byte[] buffer = m_stream.ToArray();
                //Console.WriteLine("TEST {0}", buffer);
                byte[] id = BitConverter.GetBytes((short)2);
                byte[] len = BitConverter.GetBytes((short)buffer.Length);
                byte[] checksum = BitConverter.GetBytes((short)(buffer.Length - 45 ^ 6716));
                //Console.WriteLine("FinalizeCharInfo: {} | {} | {}", id, len, checksum);
                len.CopyTo(buffer, 0);
                checksum.CopyTo(buffer, buffer.Length - 2);

                m_stream.Close();
                m_buffer = buffer;
            }
            return m_buffer;
        }


        public int Length
        {
            get
            {
                return (int)m_stream.Length;
            }
        }

        public void Dispose()
        {
            m_stream.Dispose();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Digital_World.Packets;
using Digital_World.Entities;
using Digital_World.Helpers;
using System.Timers;

namespace Digital_World
{
    public class Client : IDisposable
    {
        public Socket m_socket = null;
        public const int BUFFER_SIZE = 4096;
        public byte[] buffer = new byte[BUFFER_SIZE];
        public byte[] oldBuffer;

        private Timer myTimer = new Timer(10000);

        public string Username;
        public uint AccountID = 0;
        public string SecurityCode = null;
        public int UniqueID = 0;
        public int AccessLevel = 0;
        public int Crowns = 0;
        public int SecondPasswordTesting = 0;
        public short handshake = 0;
        public byte Characters = 0;
        public byte CreateTamerHandshake = 0;
        public byte CreateDigimonHandshake = 0;
        public DateTime DateTime = DateTime.UtcNow;
        //public string CurrentDateTime = DateTime.UtcNow.ToString("yyyy/MM/dd/ THH:mm:ssZ");


        public ItemList CashVault = new ItemList(49);

        public Character Tamer = null;

        public uint time_t = 0;

        public Client()
        {
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.AutoReset = false;
        }

        /// <summary>
        /// Send a raw packet to the client
        /// </summary>
        /// <param name="buffer"></param>
        public void Send(byte[] buffer)
        {
            BeginSend(buffer);
        }

        public void SendMultiple(byte[] buffer, byte[] buffer2)
        {
            BeginSend(buffer);
            BeginSend(buffer2);
        }

        /// <summary>
        /// Send a formed packet to the cleint
        /// </summary>
        /// <param name="packet"></param>
        public void Send(IPacket packet)
        {
            BeginSend(packet.ToArray());
        }

        public void SendCharInfo(IPacketCharInfo packet)
        {
            BeginSend(packet.ToArrayCharInfo());
        }


        public void SendMultiple(byte[] buffer)
        {

        }
        private void BeginSend(byte[] buffer)
        {
            try
            {
                myTimer.Stop();
                m_socket.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(EndSend), this);
            }
            catch (Exception)
            {
                Console.WriteLine("Error: BeginSend()");
                Close();

            }
        }

        public void Close()
        {
            m_socket.Close();
        }

        private void EndSend(IAsyncResult ar)
        {
            try
            {
                m_socket.EndSend(ar);
            }
            catch (ObjectDisposedException) { }
            catch (Exception e)
            {
                Console.WriteLine("Error: EndSend()\n{0}", e);
            }
        }

        public void Handshake_Auth()
        {
            int time_t = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            handshake = (short)(time_t & 0xFF);
            Packet packet = new Packets.PacketFFFF_Auth(handshake);
            Send(packet);

            myTimer.Start();
        }

        public void Handshake_Lobby()
        {
            Packet packet = new Packets.PacketFFFF_Lobby();
            Send(packet);

            myTimer.Start();
        }


        public void Handshake_World()
        {
            Packet packet = new Packets.PacketFFFF_Lobby();
            Send(packet);

            myTimer.Start();
        }

        void myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Handshake timeout encountered");
            m_socket.Close();
        }

        public override string ToString()
        {
            if (Tamer == null)
                return string.Format("{0}", m_socket.RemoteEndPoint);
            else
                return string.Format("{0} - {1}", m_socket.RemoteEndPoint, Tamer);
        }

        /// <summary>
        /// Sends the contents of PacketReader to the client.
        /// </summary>
        /// <param name="packet">PacketReader object</param>
        public void Send(PacketReader packet)
        {
            Send(packet.ToArray());
        }

        public void Send_V2(PacketReader packet)
        {
            Send(packet.ToArray());
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Client))
            {
                if (this.Tamer != null && (obj as Client).Tamer != null)
                    return (obj as Client).Tamer.CharacterId == this.Tamer.CharacterId;
                else
                    return (obj as Client).AccountID == this.AccountID;
            }
            return base.Equals(obj);
        }

        public static bool operator ==(Client c1, Client c2)
        {
            try
            {
                if (c1.GetType() == typeof(Client) && c2.GetType() == typeof(Client))
                {
                    if (c2.Tamer != null && c1.Tamer != null)
                        return c1.Tamer.CharacterId == c2.Tamer.CharacterId;
                    else
                        return c1.AccountID == c2.AccountID;
                }
                else
                {
                    return false;
                }
            }
            catch (NullReferenceException)
            {
                
            }
            return false;
        }

        public static bool operator !=(Client c1, Client c2)
        {
            if (c2.Tamer != null && c1.Tamer != null)
                return c1.Tamer.CharacterId != c2.Tamer.CharacterId;
            else
                return c1.AccountID != c2.AccountID;
        }

        public void Dispose()
        {
            if (m_socket.Connected)
                Close();
            myTimer.Dispose();
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(AccountID);
        }
    }
}

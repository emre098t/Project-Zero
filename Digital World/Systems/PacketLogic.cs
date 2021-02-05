using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Packets;
using System.IO;
using Digital_World.Entities;
using Digital_World.Database;
using Digital_World.Helpers;
using Digital_World.Packets.Game;

namespace Digital_World.Systems
{
    public partial class Yggdrasil
    {
        /// <summary>
        /// For Handles
        /// </summary>
        static Random Rand = new Random();

        /// <summary>
        /// Server-wide Send to all connected clients.
        /// </summary>
        /// <param name="Packet">Packet to Send</param>
        public void Send(IPacket Packet)
        {
            List<Client> remove = new List<Client>();
            Client[] temp = Clients.ToArray();
            for (int i = 0; i < temp.Length;i++ )
            {
                Client Client = temp[i];
                try
                {
                    Client.Send(Packet);
                }
                catch
                {
                    remove.Add(Client);
                }
            }

            lock (Clients)
            {
                foreach (Client Client in remove)
                {
                    Client.Close();
                    Clients.Remove(Client);
                }
            }
        }

        /// <summary>
        /// Process incoming packets
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        public void Process(Client client, PacketReader packet)
        {
            Character Tamer = client.Tamer;
            Digimon ActiveMon = null;
            GameMap ActiveMap = null;
            if (Tamer != null && Tamer.Partner != null)
            {
                ActiveMon = Tamer.Partner;
                ActiveMap = Maps[client.Tamer.Location.Map];
            }

            Console.WriteLine("Digital-World");
            Console.WriteLine(packet.Type);

            switch (packet.Type)
            {
                case -1:
                    
                        client.time_t = (uint)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                        client.Send(new Packets.PacketFFFE((int)client.time_t, (short)(client.handshake ^ 0x7e41)));
                        //client.Send(new Packets.PacketFFFE((int)client.time_t, 0x7e41));
                    break;
                case 1706:
                    {
                        uint u = packet.ReadUInt();
                        uint acctId = packet.ReadUInt();
                        int accessCode = packet.ReadInt();

                        SqlDB.LoadUser(client, acctId, accessCode);
                        SqlDB.LoadTamer(client);

                        if (client.Tamer == null)
                            client.m_socket.Close();

                        lock (Clients) { Clients.Add(client); }

                        MakeHandles(client.Tamer, (uint)client.time_t);

                        Packet p = new CharInfo(client.Tamer);
                        client.Send(p);

                        Maps[client.Tamer.Location.Map].Enter(client); //Enter GameMap
                        break;





                    }




                /*
Server->Client
7D 00 17 04 01 74 57 65 6C 63 6F 6D 65 20 74 6F     }....tWelcome.to
20 44 69 67 69 74 61 6C 20 4D 61 73 74 65 72 73     .Digital.Masters
20 57 6F 72 6C 64 20 4C 61 7A 61 72 37 21 20 49     .World.Lazar7!.I
66 20 79 6F 75 20 6E 65 65 64 20 68 65 6C 70 20     f.you.need.help.
70 6C 65 61 73 65 20 63 6F 6E 74 61 63 74 20 75     please.contact.u
73 20 76 69 61 20 6D 6F 64 6D 61 69 6C 20 61 74     s.via.modmail.at
20 64 69 73 63 6F 72 64 21 20 45 6E 6A 6F 79 20     .discord!.Enjoy.
79 6F 75 72 20 74 69 6D 65 21 00 41 1A              your.time!.A.    

 */

                case 1713:
                    {
                        client.Send(new Packets.Game.CharInfo2(client.Tamer));
                        break;
                    }

                case -3:
                    { break; }

                case 1703:
                    {
                        //Appears at the end of map switching
                        client.Send(packet);
                        break;
                    }
                default:
                    {
                        //Unknown Packets!
                        Console.WriteLine("Unknown Packet ID: {0}", packet.Type);
                        Console.WriteLine(Packet.Visualize(packet.ToArray()));

                        break;
                    }
            }
        }



        public static string WelcomeMessage(Client client)
        {
            string message = "";

            message.Replace("Welcome to Digimon Masters {0}", client.Tamer.Name);
            return message;
        }
        public static uint GetModel(uint Model)
        {
            uint hEntity = Model;
            return (uint)(hEntity + Rand.Next(1, 255));
        }

        public int size_obtained(int x, int y, int z)
        {
            int max = 0;

            if (x > y && x > z)
            {
                max = 3;
            }
            else if (y > z && y > x)
            {
                max = 4;
            }
            else if(z > x && z > y) 
            {
                max = 5;

            } 
            else if(x == y && y == x){

                max = fix_size_tie(3,4);
            }
            else if (y == z && z == y)
            {
                max = fix_size_tie(4, 5);

            }
            else if (z == x && x == z)
            {
                max = fix_size_tie(3, 5);
            }
            return max;
        }

        public int fix_size_tie(int x, int y)
        {
            int result = 0;

            int min = 0;

            int max = 2;

            Random number = new Random();

            int[] array = new int[2];
            array[0] = x;
            array[1] = y;

            int rand = number.Next(min, max);

            result = array[rand];

            return result;
        }

        public static uint GetModel(uint Model, byte Id)
        {
            uint hEntity = Model;
            return (uint)(hEntity + Id);
        }

        public static short GetHandle(uint Model, byte type)
        {
            byte[] b = new byte[] { (byte)((Model >> 32) & 0xFF), type };
            return BitConverter.ToInt16(b, 0);
        }

        public static void MakeHandles(Character Tamer, uint time_t)
        {
            Tamer.intHandle = (uint)(Tamer.ProperModel + Rand.Next(1,255));

            for (int i = 0; i < Tamer.DigimonList.Length; i++)
            {
                if (Tamer.DigimonList[i] == null) continue;
                Digimon mon = Tamer.DigimonList[i];
                mon.Model = GetModel(mon.ProperModel());
            }

            Tamer.DigimonHandle = Tamer.Partner.Handle;

            //Tamer.DigimonHandle = 0x3410; //-> 4118
        }

    }
}

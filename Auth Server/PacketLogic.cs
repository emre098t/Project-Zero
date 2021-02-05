using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Digital_World.Packets;
using System.IO;
using Digital_World.Database;
using System.Configuration;

namespace Digital_World
{
    public class PacketLogic
    {
        public static void Process(Client client, byte[] buffer)
        {
            PacketReader packet = null;
            try
            {
                packet = new PacketReader(buffer);
            }
            catch
            {
                return;
            }
            switch (packet.Type)
            {

                //0xFFFF - > HANDSHAKE
                case -1:
                    {
                        //TIME BASICALLY! 
                        client.Send(new Packets.PacketFFFE((short)(client.handshake ^ 0x7e41)));
                        
                        break;
                    }
                //LOGIN    
                case 0xCE5:
                    {
                        packet.Seek(4);

                        int test1 = packet.ReadInt();
                        Console.WriteLine(test1);

                        packet.Seek(8);

                        int GDMOVer = packet.ReadByte();

                        switch (GDMOVer)
                        {
                            case 5:
                                {
                                    packet.Seek(15);
                                    int st_size = packet.ReadByte();
                                    byte[] steamid_get = new byte[st_size];
                                    for (int i = 0; i < st_size; i++)
                                    {
                                        steamid_get[i] = packet.ReadByte();
                                        //Console.WriteLine(username_get[i]);
                                    }
                                    string steamid = Encoding.ASCII.GetString(steamid_get).Trim();
                                    int steamvalidatecheck = SqlDB.SteamValidate(client, steamid);

                                    switch (steamvalidatecheck)
                                    {
                                        case -3:
                                            {
                                                SqlDB.CreateSteamUser(steamid);
                                                SqlDB.SteamValidate(client, steamid);
                                                client.Send(new Packets.Auth.LoginTo(1));
                                                break;
                                            }
                                        default:
                                            {
                                                //Console.WriteLine("SC: {0}", client.SecurityCode);
                                                //When u send loginmessage after login
                                                //31
                                                //30 Character is already accessed. Please access again later
                                                //55 It will be opened in a short while.
                                                //56

                                                int status = SqlDB.ServerStatus();
                                                switch (status)
                                                {
                                                    case 2:
                                                        {
                                                            client.Send(new Packets.Auth.LoginMessage(31));
                                                            break;
                                                        }
                                                    default:
                                                        {

                                                            client.Send(new Packets.Auth.LoginTo(1));
                                                            break;
                                                        }
                                                }
                                                /*
                                                if (client.SecurityCode == null)
                                                {
                                                    client.Send(new Packets.Auth.LoginTo(3));
                                                }
                                                if (client.SecurityCode != null)
                                                {
                                                    client.Send(new Packets.Auth.LoginTo(2));
                                                }
                                                if (client.SecurityCode == "skip")
                                                {
                                                    client.Send(new Packets.Auth.LoginTo(1));
                                                }*/
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case 0:
                                {
                                    packet.Seek(9);
                                    int u_size = packet.ReadByte();
                                    byte[] username_get = new byte[u_size];

                                    for (int i = 0; i < u_size; i++)
                                    {
                                        username_get[i] = packet.ReadByte();
                                        //Console.WriteLine(username_get[i]);
                                    }

                                    string username = Encoding.ASCII.GetString(username_get).Trim();

                                    //Console.WriteLine(username);

                                    packet.Seek(9 + username.Length + 2);

                                    int p_size = packet.ReadByte();

                                    byte[] password_get = new byte[p_size];


                                    for (int i = 0; i < p_size; i++)
                                    {
                                        password_get[i] = packet.ReadByte();
                                        //Console.WriteLine(username_get[i]);
                                    }

                                    string password = Encoding.ASCII.GetString(password_get).Trim();

                                    //Console.WriteLine(password);
                                    Console.WriteLine("USER LOGIN-IN: {0}", username);

                                    int check = SqlDB.Validate(client, username, password);

                                    switch (check)
                                    {

                                        case -1:

                                            //client.Send(new Packets.Auth.LoginMessage("This user has been banned."));
                                            client.Send(new Packets.Auth.LoginMessage(0x44));

                                            break;

                                        //incorrect pass
                                        case -2:


                                            //client.Send(new Packets.Auth.LoginMessage("Incorrect Password!"));
                                            client.Send(new Packets.Auth.LoginMessage(0x49));

                                            break;

                                        //Username doesn't exist
                                        case -3:
                                            //client.Send(new Packets.Auth.LoginMessage(0x12));
                                            client.Send(new Packets.Auth.LoginMessage(0x12));

                                            break;

                                        //Normal Login
                                        default:
                                            Console.WriteLine("User Login Succesful: {0}|{1}", username, client.SecurityCode);
                                            int status = SqlDB.ServerStatus();
                                            switch (status)
                                            {
                                                case 2:
                                                    {
                                                        client.Send(new Packets.Auth.LoginMessage(31));
                                                        break;
                                                    }
                                                default:
                                                    {                                                       
                                                        client.Send(new Packets.Auth.LoginTo(1));

                                                        break;
                                                    }
                                            }
                                            break;

                                    }
                                }

                                break;
                        }
                        break;
                    }

                case 0x06A6:
                    {

                        /*GET SERVER NUMBER = SERVER ID - EXAMPLE
                         * 
                         * 0A 00
                         * A6 06
                         * 01 00 00 00 -> IDENTIFICATION = SERVER NUMBER -> ID
                         * 36 1A 
                         * 
                         */
                        int serverID = BitConverter.ToInt32(buffer, 4);


                        //FROM HERE YOU WILL GET PORT & IP
                        KeyValuePair<int, string> server = SqlDB.GetServer(serverID);

                        //LOAD THE USER - > LOADS ALL INFO OF CONNECTING CLIENT INTO THE CLIENT CLASS
                        SqlDB.LoadUser(client);
                        client.Send(new Packets.Auth.ServerIP(server.Value, (uint)server.Key, (uint)client.AccountID, (uint)client.UniqueID));
                        break;
                    }
                case 0x6A5:
                    {
                        //NOT REALLY WORRIED ABOUT THIS ONE - > Refresh ServerList

                        client.Send(new Packets.Auth.ServerList(SqlDB.GetServers(), client.Username, client.Characters));

                        break;
                    }

                case -3:
                    {


                        break;
                    }
                default:
                    {
                        //Unknown Packets!
                        Console.WriteLine("Unknown Packet ID: {0}", packet.Type);
                        Console.WriteLine(Packet.Visualize(buffer));

                        break;
                    }
            }

        }

    }

}



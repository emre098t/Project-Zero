using System;
using System.Collections.Generic;
using Digital_World.Entities;
using Digital_World.Packets;
using Digital_World.Helpers;
using System.Windows;
using Digital_World.Packets.Game;
using System.Collections.ObjectModel;
using Digital_World.Database;
using System.IO;

namespace Digital_World
{
    public class PacketLogic
    {
        private static ReadOnlyCollection<TimeZoneInfo> _timeZones = TimeZoneInfo.GetSystemTimeZones();
        public static void Process(Client client, byte[] buffer, Settings Opt)
        {
            PacketReader packet = new PacketReader(buffer);

            //Console.WriteLine("Lobby-");
            //Console.WriteLine("{0}|{1}", packet.Type, packet.Checksum);

            switch (packet.Type)
            {

                case -1 :
                    {

                        //Console.WriteLine("Accepted connection: {0}", client.m_socket.RemoteEndPoint);

                        client.time_t = (uint)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                        //TIME BASICALLY! 
                        //client.Send(new Packets.PacketFFFE(0x7e41));
                        client.Send(new Packets.PacketFFFE((short)(client.handshake ^ 0x7e41)));
                    }

                break;

                case -3:
                    break; 

                case 1706 : //LIST CHAR
                    {
                        packet.Seek(8);

                        //ACCOUNT ID
                        uint AcctID = packet.ReadUInt();
                        Console.WriteLine(AcctID);
                        //UNIQUE ID
                        int uniID = packet.ReadInt();

                        SqlDB.LoadUser(client, AcctID, uniID);
                        List<Character> list_of_tamers = SqlDB.GetCharacters(AcctID);

                        //byte[] buffer2 = File.ReadAllBytes("C:\\Users\\Lazarevic\\Desktop\\CharList.packet");
                        //client.Send(buffer2);
                        if (list_of_tamers.Count > 0)
                            client.Send(new Packets.Lobby.CharList(list_of_tamers));
                        else if (list_of_tamers.Count == 0)
                            client.Send(new Packets.Lobby.CharList_default(list_of_tamers));

                    }
                    break;

                case 0x516: //CHECK NAME
                    {
                        string check_name = packet.ReadString();

                        if (SqlDB.NameAvail(check_name))
                        {
                            client.Send(new Packets.Lobby.NameCheck(1)); client.CreateTamerHandshake = (byte)((int)client.time_t);
                        }
                        else
                        {
                            client.Send(new Packets.Lobby.NameCheck(0));
                        }
                        break; 

                    }
                case 0x6A7:
                    {
                        client.Send(packet.ToArray());
                        break;
                    }
                case 0x518: //CHAR DELETE SYSTEM
                    {
                        int slot = packet.ReadInt();
                        string code = packet.ReadString();
                        bool canDelete = SqlDB.VerifyCode(client.AccountID, code);

                        if (canDelete)
                        {
                            if (SqlDB.DeleteTamer(client.AccountID, slot))
                                client.Send(new Packets.Lobby.CharDelete(1));
                            else
                                //client.Send(new Packets.Lobby.CharDelete(0));
                                client.Send(new Packets.Lobby.CharDelete(0));
                        }
                        else
                        {
                            client.Send(new Packets.Lobby.CharDelete(2));
                        }
                        break;
                    }
                  

                case 0x517:  //CREATE SYSTEM
                    {
                        client.CreateDigimonHandshake = (byte)((int)client.time_t);
                        int position = packet.ReadByte();
                        int model = packet.ReadInt();
                        string CharName = packet.ReadZString();

                        packet.Seek(42); //42

                        int digi_model = packet.ReadInt();
                        string DigiName = packet.ReadZString();
                        Console.WriteLine("{0} | {1} | {2} | {3}", model, CharName, digi_model, DigiName);


                        int charID = SqlDB.CreateCharacter(client.AccountID,position,model,CharName,digi_model);
                        int digiID = (int)SqlDB.CreateDigimon((uint)charID,DigiName,digi_model);
                        SqlDB.SetPartner(charID, digiID);
                        SqlDB.SetTamer(charID, digiID);
                        client.Send(new Packets.Lobby.ConfirmCreate(client, position, model, digi_model, CharName, DigiName));  // -> CHECK!!!

                        break;
                    }

                case 1305: //REQUEST MAP SERVER
                    {
                        //Request Map Server
                        int g_slot = packet.ReadInt();
                        Position pLoc = null;
                        try
                        {
                            SqlDB.SetLastChar(client.AccountID, g_slot);
                            pLoc = SqlDB.GetTamerPosition(client.AccountID, g_slot);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        client.Send(new Packets.Lobby.ServerIP(Opt.GameServer.IP.ToString(), Opt.GameServer.Port,
                        pLoc.Map, pLoc.MapName));

                    }
                    break;
                
                    
                
            }

        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static void IsTamerDeleteable()
        {
            
        }
    }
}

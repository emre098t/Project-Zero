﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Digital_World.Network;
using System.Collections.ObjectModel;
using Digital_World.Database;
using Digital_World.Helpers;
using System.IO;

namespace Digital_World
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LobbyMainWin : Window
    {
        private SocketWrapper server = null;
        private ObservableCollection<Client> clients = new ObservableCollection<Client>();
        private Settings Opt = null;

        public LobbyMainWin()
        {
            InitializeComponent();
            
            server = new SocketWrapper();
            server.OnAccept += new SocketWrapper.dlgAccept(server_OnAccept);
            server.OnClose += new SocketWrapper.dlgClose(server_OnClose);
            server.OnRead += new SocketWrapper.dlgRead(PacketProcessor);

            Logger l = new Logger(tLog);
            //listClients.ItemsSource = clients;

            MapDB.Load("Data\\Map_List.bin");
            DigimonDB.Load("Data\\DigimonList.bin");



            Opt = Settings.Deserialize();

            Console.WriteLine("This server is configured to route players to the game server at {0}:{1}",
                Opt.GameServer.Host, Opt.GameServer.Port);
            if (Opt.LobbyServer.AutoStart)
            {
                server.Listen(new ServerInfo(Opt.LobbyServer.Port, Opt.LobbyServer.IP, Opt.LobbyServer.MaxPlayers));
                Title = "Starting Lobby Server...";
            }
        }

        void PacketProcessor(Client client, byte[] buffer, int length)
        {
            int type = BitConverter.ToInt16(buffer, 2);

            PacketLogic.Process(client, buffer, Opt);
        }

        void server_OnClose(Client client)
        {
            try
            {
                clients.Remove(client);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: OnClose\n{0}", e);
            }
        }

        void server_OnAccept(Client client)
        {
            client.Handshake_Lobby();
            //listClients.Dispatcher.BeginInvoke(new Action(() => { clients.Add(client); }));
        }

        private void mi_Options_Click(object sender, RoutedEventArgs e)
        {
            Options winOpt = new Options();
            if (winOpt.ShowDialog().Value)
                Opt = Settings.Deserialize();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (server.Running) return;
            server.Listen(new ServerInfo(Opt.LobbyServer.Port, Opt.LobbyServer.IP, Opt.LobbyServer.MaxPlayers));
            Title = "Starting Lobby Server...";
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            server.Stop();
            foreach (Client client in clients)
            {
                //client.Send(new Packets.Auth.LoginMessage("Server is shutting down."));
                client.Send(new Packets.Auth.LoginMessage(0x12));
                client.m_socket.Close();
            }

            sbInfo1.Content = "Stopping Lobby Server...";
        }

        private void mi_Debug_Click(object sender, RoutedEventArgs e)
        {
            foreach (Client client in clients)
            {
                //-15325
                client.Send(new Packets.Auth.LoginMessage(0x31));
            }
        }
    }
}

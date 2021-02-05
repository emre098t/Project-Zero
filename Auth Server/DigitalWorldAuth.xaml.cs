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
using System.Diagnostics;
using Digital_World.Network;
using Digital_World.Packets;
using Digital_World.Helpers;
using MahApps.Metro.Controls;

namespace Digital_World
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthMainWin : Window //Window
    {
        SocketWrapper server;
        List<Client> clients = new List<Client>();
        Settings Opt;

        public AuthMainWin()
        {
            InitializeComponent();

            server = new SocketWrapper();
            server.OnAccept += new SocketWrapper.dlgAccept(m_auth_OnAccept);
            server.OnRead += new SocketWrapper.dlgRead(m_auth_OnRead);
            server.OnClose += new SocketWrapper.dlgClose(server_OnClose);

            Logger _writer = new Logger(tLog);

            Opt = Settings.Deserialize();
            if (Opt.AuthServer.AutoStart)
            {
                ServerInfo info = new ServerInfo(Opt.AuthServer.Port, Opt.AuthServer.IP, Opt.AuthServer.MaxPlayers);
                server.Listen(info);
                Title = "Running";
            }
            //LabelCount.Content = clients.Count();
        }

        void m_auth_OnRead(Client client, byte[] buffer, int length)
        {
            //TODO: Packet Response Logic
            PacketLogic.Process(client, buffer);
        }

        void m_auth_OnAccept(Client state)
        {
            state.Handshake_Auth();
            clients.Add(state);
        }

        void server_OnClose(Client client)
        {
            try
            {
                clients.Remove(client);
            }
            catch { }
        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (server.Running) return;
            ServerInfo info = new ServerInfo(Opt.AuthServer.Port,
                 Opt.AuthServer.IP, Opt.AuthServer.MaxPlayers);
            server.Listen(info);
            Title = "Running";
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            server.Stop();
            
            foreach(Client client in clients)
            {
                //client.Send(new Packets.Auth.LoginMessage("Server is shutting down."));
                client.Send(new Packets.Auth.LoginMessage(0x12));
                client.m_socket.Close();
            }
            Title = "Stopped";
        }

        private void mi_opt_Click(object sender, RoutedEventArgs e)
        {
            Options winOpt = new Options();
            if (winOpt.ShowDialog().Value)
                Opt = Settings.Deserialize();
        }
    }
}

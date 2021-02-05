﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Digital_World.Helpers;
using MySql.Data.MySqlClient;

namespace Digital_World
{
    /// <summary>
    /// MySQL Database Wrapper for the Digital World
    /// </summary>
    public partial class SqlDB
    {
        private static string db_host = "";
        private static string db_user = "";
        private static string db_pass = "";
        private static string db_schema = "";

        private static MySqlConnection m_con;
        private static MySqlConnection Connection
        {
            get
            {
                if (m_con == null || m_con.State != ConnectionState.Open || m_con.Database == "")
                    m_con = Connect();
                return m_con;
            }
            set { m_con = value; }
        }

        public static void SetInfo(string host, string user, string pass, string table)
        {
            db_host = host;
            db_user = user;
            db_pass = pass;
            db_schema = table;
        }

        private static Random RNG = new Random();

        static SqlDB()
        {
            Connection = Connect();
        }

        /// <summary>
        /// Connect to the mysql db
        /// </summary>
        /// <returns>A connection to the database</returns>
        public static MySqlConnection Connect()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(string.Format("server={0};uid={1};pwd={2};database={3};", db_host, db_user, db_pass, db_schema));
                conn.Open();
                return conn;
            }
            catch (MySqlException)
            {
                return null;
            }
        }

        public static int Validate(Client client, string user, string pass)
        {
            int level = 0;
            try
            {
                using (MySqlConnection Connection = Connect())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `username` = @user", Connection))
                {
                    cmd.Parameters.AddWithValue("@user", user);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            if (read.Read())
                            {
                                if (read["username"].ToString() == user && read["password"].ToString() == SHA2(pass))
                                {
                                    level = (int)read["level"];
                                    client.AccessLevel = (int)read["level"];
                                    client.Username = user;
                                    client.AccountID = Convert.ToUInt32((int)read["accountId"]);
                                    client.Crowns = (int)read["crowns"];
                                    client.SecondPasswordTesting = (int)read["secondpasswordtest"];
                                    client.SecurityCode = read["securitycode"].ToString();

                                    if (read["cashvault"] == null)
                                    {
                                        try
                                        {
                                            client.CashVault = ItemList.Deserialize((byte[])read["cashvault"]);
                                        }
                                        catch
                                        {
                                            client.CashVault = new ItemList(49);
                                        }
                                    }
                                }
                                else
                                {
                                    //Wrong Pass
                                    level = -2;
                                }
                            }
                        }
                        else

                        { level = -3; }
                    }

                }
                using (MySqlConnection Connection = Connect())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `chars` WHERE `accountId` = @id", Connect()))
                {
                    cmd.Parameters.AddWithValue("@id", client.AccountID);
                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                client.Characters++;
                            }
                        }
                        else client.Characters = 0;
                    }
                }


            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: Validate\n{0}", e);
                level = -1;
            }
            return level;
        }
        public static int SteamValidate(Client client, string steamid)
        {
            int level = 0;
            try
            {
                using (MySqlConnection Connection = Connect())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `username` = @user", Connection))
                {
                    cmd.Parameters.AddWithValue("@user", steamid);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            if (read.Read())
                            {
                                if (read["username"].ToString() == steamid)
                                {
                                    level = (int)read["level"];
                                    client.AccessLevel = (int)read["level"];
                                    client.Username = steamid;
                                    client.AccountID = Convert.ToUInt32((int)read["accountId"]);
                                    client.Crowns = (int)read["crowns"];
                                    client.SecondPasswordTesting = (int)read["secondpasswordtest"];
                                    client.SecurityCode = read["securitycode"].ToString();

                                    if (read["cashvault"] == null)
                                    {
                                        try
                                        {
                                            client.CashVault = ItemList.Deserialize((byte[])read["cashvault"]);
                                        }
                                        catch
                                        {
                                            client.CashVault = new ItemList(49);
                                        }
                                    }
                                }
                                else
                                {
                                    //Wrong Pass
                                    level = -2;
                                }
                            }
                        }
                        else

                        { level = -3; }
                    }

                }
                using (MySqlConnection Connection = Connect())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `chars` WHERE `accountId` = @id", Connect()))
                {
                    cmd.Parameters.AddWithValue("@id", client.AccountID);
                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                client.Characters++;
                            }
                        }
                        else client.Characters = 0;
                    }
                }


            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: Validate\n{0}", e);
                level = -1;
            }
            return level;
        }



        public static bool CreateUser(string user, string pass)
        {
            bool success = false;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO `acct` (`username`, `password`)  VALUES (@user, @pass)", Connection))
                {
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@pass", SHA2(pass));

                    int r = cmd.ExecuteNonQuery();
                    if (r == 1)
                    {
                        success =  true;
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: CreateUser\n{0}", e);
                return false;
            }
            return success;
        }

        public static bool CreateSteamUser(string steamid)
        {
            bool success = false;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO `acct` (`username`, `password`, `email`)  VALUES (@user, @pass, @Delete)", Connection))
                {
                    cmd.Parameters.AddWithValue("@user", steamid);
                    cmd.Parameters.AddWithValue("@pass", SHA2(steamid));
                    cmd.Parameters.AddWithValue("@Delete", "Delete");

                    int r = cmd.ExecuteNonQuery();
                    if (r == 1)
                    {
                        success = true;
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: CreateSteamUser\n{0}", e);
                return false;
            }
            return success;
        }
        /****** InsertItems to mysql *****/

        public static void InsertItems(int fullID, string name, ushort Itemid, string type_kind)
        {
            using (MySqlConnection connect = Connect())
            using (MySqlCommand cmd = new MySqlCommand("INSERT INTO items (full_id,short_id,name,type) VALUES (@full,@short,@name,@type_k)", connect))
            {
                cmd.Parameters.AddWithValue("@full", fullID);
                cmd.Parameters.AddWithValue("@short", Itemid);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@type_k",type_kind);

                cmd.ExecuteNonQuery();
            }


        }

        /***** Get Digi Name *******/

        public static string getDigiName(int fullId)
        {

            string name = "";

            using(MySqlConnection Connection = Connect())
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM items WHERE full_id = @id",Connection))
            {
                cmd.Parameters.AddWithValue("@id",fullId);

                using (MySqlDataReader read = cmd.ExecuteReader())
                {
                    if(read.HasRows){

                        while(read.Read()){

                            name = read.GetString(4);
                        }
                    }

                }
                   
            }

            return name;

        }
        public static int getDigiID(string name)
        {

            int idx = 0;

            using (MySqlConnection Connection = Connect())
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM items WHERE name = @name_", Connection))
            {
                cmd.Parameters.AddWithValue("@name_", name);

                using (MySqlDataReader read = cmd.ExecuteReader())
                {
                    if (read.HasRows)
                    {

                        while(read.Read())
                        {       
                                
                                idx = read.GetInt32(1);
                            
                            
                        }
                    }

                }

            }

            return idx;

        }
        /// <summary>
        /// Loads all user information into the Client class
        /// </summary>
        /// <param name="client">Client</param>
        public static void LoadUser(Client client)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `username` = @user", Connection))
                {
                    cmd.Parameters.AddWithValue("@user", client.Username);

                    using (MySqlDataReader read = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (read.HasRows)
                        {
                            if (read.Read())
                            {
                                client.AccessLevel = (int)read["level"];
                                client.AccountID = Convert.ToUInt32((int)read["accountId"]);

                                int uniId = RNG.Next(1, int.MaxValue);
                                using (MySqlCommand updateUniId =
                                    new MySqlCommand("UPDATE `acct` SET `uniId` = @uniId WHERE `accountId` = @id", Connect()))
                                {
                                    updateUniId.Parameters.AddWithValue("@uniId", uniId);
                                    updateUniId.Parameters.AddWithValue("@id", read["accountId"]);
                                    updateUniId.ExecuteNonQuery();

                                    client.UniqueID = uniId;
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: Validate\n{0}", e);
            }
        }

        /// <summary>
        /// Loads all user data into the Client class. Used by the Lobby Server
        /// </summary>
        /// <param name="client">client</param>
        /// <param name="AccountID">AccountID to find</param>
        /// <param name="UniId">Unique ID</param>
        public static void LoadUser(Client client, uint AccountID, int UniId)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `accountId` = @id", Connection))
                {
                    cmd.Parameters.AddWithValue("@id", AccountID);

                    using (MySqlDataReader read = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (read.HasRows)
                        {
                            if (read.Read() && (int)read["uniId"] == UniId)
                            {
                                client.AccessLevel = (int)read["level"];
                                client.AccountID = AccountID;

                                client.UniqueID = UniId;
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: Validate\n{0}", e);
            }
        }

        public static Dictionary<int, string> GetServers()
        {
            Dictionary<int, string> servers = new Dictionary<int, string>();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `serverid`, `name` FROM servers", Connection))
                {
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                servers.Add((int)data["serverid"], data["name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServers\n{0}", e);
            }
            return servers;
        }
        public static int LoginMessage2()
        {
            int message = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `maintenance` FROM servers", Connection))
                {
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                message = (int)data["maintenance"];
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServersInfo\n{0}", e);
            }
            return message;
        }
        public static int SecurityCode(Client client)
        {
            int securitymessage = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `securitycode` FROM `acct` WHERE `accoundId`= @acct", Connection))
                {
                    cmd.Parameters.AddWithValue("@acct", client.AccountID);
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                if(data["securitycode"] == null)
                                {
                                    securitymessage = 0;
                                }
                                if(data["securitycode"] != null)
                                {
                                    securitymessage = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServersInfo\n{0}", e);
            }
            return securitymessage;
        }

        public static int ServerStatus()
        {
            int message = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `maintenance` FROM `servers` WHERE `serverId`= 0", Connection))
                {
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                message = (int)data["maintenance"];
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServersInfo\n{0}", e);
            }
            return message;
        }



        public static int ServerMaintenance(string name)
        {
            int message = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `maintenance` FROM `servers` WHERE `name`= @name", Connection))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                message = (int)data["maintenance"];
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServersInfo\n{0}", e);
            }
            return message;
        }



        public static int ServerLoad(string name)
        {
            int message = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `serverload` FROM `servers` WHERE `name`= @name", Connection))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                message = (int)data["serverload"];
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServersInfo\n{0}", e);
            }
            return message;
        }

        public static int ServerNewOrNo(string name)
        {
            int message = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `isNew?` FROM `servers` WHERE `name`= @name", Connection))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                message = (int)data["isNew?"];
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServersInfo\n{0}", e);
            }
            return message;
        }



        public static KeyValuePair<int, string> GetServer(int ID)
        {
            KeyValuePair<int, string> k = new KeyValuePair<int,string>(6999,"127.0.0.1");
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT port, INET_NTOA(ip) ip FROM servers WHERE `serverId` = @id", Connection))
                {
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                k = new KeyValuePair<int, string>((int)data["port"], data["ip"].ToString());
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServer\n{0}", e);
            }
            return k;
        }


        /// <summary>
        /// Get the number of characters for a specific account id
        /// </summary>
        /// <param name="AcctId">Account Id to match</param>
        /// <returns>The number of characters tied to AcctId</returns>
        public static int GetNumChars(uint AcctId)
        {
            int characters = 0;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM `chars` WHERE `accountId` = @id"
                    , Connection))
                {
                    cmd.Parameters.AddWithValue("@id", AcctId);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                characters++;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetCharacters\n{0}", e);
            }
            return characters;
        }

        public static string SHA2(string value)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 shaM = new SHA256Managed())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(value);
                buffer = shaM.ComputeHash(buffer);

                for (int i = 0; i < buffer.Length; i++)
                    sb.Append(buffer[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string CreateSecurityCode(Client client, string code)
        {
            try
            {
                using (MySqlConnection con = Connect())
                {
                    using (MySqlCommand cmd = new MySqlCommand(
                    "UPDATE `acct` SET `securitycode` = @scode WHERE `accountId` = @acct",
                    con))
                    {
                        cmd.Parameters.AddWithValue("@acct", client.AccountID);
                        cmd.Parameters.AddWithValue("@scode", code);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: VerifyCode()\n{0}", e);
            }
            return code;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace Template.Services
{
    /// <summary>
    /// MySQL Database Wrapper for the Digital World
    /// </summary>
    public partial class SqlDB
    {
        private static string db_host = "localhost";
        private static string db_user = "root";
        private static string db_pass = "Barsa123!";
        private static string db_schema = "gdmo";

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
        public static string GetUsername(ulong discordid)
        {
            string username = "";
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT * from `acct` WHERE `discordid` = @discordid"
                    , Connection))
                {
                    cmd.Parameters.AddWithValue("@discordid", discordid);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                username = (string)read["username"];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetCharacters\n{0}", e);
            }
            return username;
        }

        public static string DeleteDiscordAccount(ulong discordid)
        {
            string username = "";
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "DELETE from `acct` WHERE `discordid` = @discordid"
                    , Connection))
                {
                    cmd.Parameters.AddWithValue("@discordid", discordid);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                username = (string)read["username"];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetCharacters\n{0}", e);
            }
            return username;
        }
        /*
        public static List<Character> GetCharacters(uint AcctId)
        {
            List<Character> chars = new List<Character>();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM `chars` WHERE `accountId` = @id"
                    , Connect()))
                {
                    cmd.Parameters.AddWithValue("@id", AcctId);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Character Tamer = new Character();
                                Tamer.AccountId = AcctId;
                                Tamer.CharacterId = Convert.ToUInt32((int)dr["characterId"]); ;
                                Tamer.CharacterPos = ((int)dr["characterPos"]); ;
                                Tamer.Model = (CharacterModel)(int)dr["charModel"];
                                Tamer.Name = (string)dr["charName"];
                                Tamer.Level = (int)dr["charLv"];
                                Tamer.Location = new Helpers.Position((short)(int)dr["map"], (int)dr["x"], (int)dr["y"]);

                                Tamer.Partner = GetDigimon((uint)(int)dr["partner"]);
                                if (dr["mercenary1"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary1"];
                                    Digimon merc = GetDigimon((uint)mercId);
                                    Tamer.DigimonList[1] = merc;
                                }
                                if (dr["mercenary2"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary2"];
                                    Digimon merc = GetDigimon((uint)mercId);
                                    Tamer.DigimonList[2] = merc;
                                }

                                try
                                {
                                    BinaryFormatter f = new BinaryFormatter();
                                    using (MemoryStream m = new MemoryStream((byte[])dr["archive"]))
                                    {
                                        Tamer.ArchivedDigimon = (uint[])f.Deserialize(m);
                                    }
                                    for (int i = 0; i < Tamer.ArchivedDigimon.Length; i++)
                                    {
                                        if (Tamer.ArchivedDigimon[i] != 0)
                                        {
                                            Digimon digi = GetDigimon(Tamer.ArchivedDigimon[i]);
                                            ResetModel(digi.DigiId, digi.Species);
                                        }
                                    }

                                }
                                catch { Tamer.ArchivedDigimon = new uint[40]; }

                                try
                                {
                                    Tamer.Equipment = ItemList.Deserialize((byte[])dr["equipment"]);
                                }
                                catch
                                {
                                    Tamer.Equipment = new ItemList(39);
                                }

                                chars.Add(Tamer);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetCharacters\n{0}", e);
            }

            return chars;
        }
        */
        //DELETE FROM `gdmo`.`acct` WHERE  `accountId`=12;
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
        public static bool DiscordIDAvail(ulong discordid)
        {
            if (Connection == null) Connection = Connect();
            bool avail = false;
            try
            {
                using (MySqlConnection con = Connect())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `discordid` = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", discordid);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows) avail = false;
                        else avail = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: DiscordIDAvail({1})\n{0}", e, discordid);
            }
            return avail;
        }

        public static int GetAccountID(ulong discordid)
        {
            int accountId = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT * from `acct` WHERE `discordid` = @discordid"
                    , Connection))
                {
                    cmd.Parameters.AddWithValue("@discordid", discordid);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                accountId = (int)read["accountId"];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetAccountId\n{0}", e);
            }
            return accountId;
        }

        public static int GetAccountIDwUser(string user)
        {
            int accountId = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT * from `acct` WHERE `username` = @username"
                    , Connection))
                {
                    cmd.Parameters.AddWithValue("@username", user);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                accountId = (int)read["accountId"];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetAccountId\n{0}", e);
            }
            return accountId;
        }
        public static bool isCreated(ulong discordid)
        {
            bool allow = false;
            try
            {
                using (MySqlConnection con = Connect())
                {
                    using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `discordid` from `acct` WHERE `discordid` = @discordid",
                    con))
                    {
                        cmd.Parameters.AddWithValue("@discordid", discordid);
                        using (MySqlDataReader read = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                        {
                            if (read.HasRows && read.Read())
                            {
                                if (discordid.Equals(read["discordid"]))
                                    allow = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: isCreated()\n{0}", e);
            }
            return allow;
        }

        public static bool isCreatedUser(string username)
        {
            bool allow = false;
            try
            {
                using (MySqlConnection con = Connect())
                {
                    using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `username` from `acct` WHERE `username` = @user",
                    con))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        using (MySqlDataReader read = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                        {
                            if (read.HasRows && read.Read())
                            {
                                if (username.Equals(read["username"]))
                                    allow = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: isCreated()\n{0}", e);
            }
            return allow;
        }


        public static void SetNewPassword(ulong discordid, string password)
        {
            try
            {
                using (MySqlConnection Connection = Connect())
                using (MySqlCommand cmd = new MySqlCommand("UPDATE `acct` SET `password` = @password WHERE `discordid` = @discordid",
                    Connection))
                {
                    cmd.Parameters.AddWithValue("@discordid", discordid);
                    cmd.Parameters.AddWithValue("@password", SHA2(password));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: SetNewPassword()\n{0}", e);
            }
        }

        public static void SetNewUsername(ulong discordid, string username)
        {
            try
            {
                using (MySqlConnection Connection = Connect())
                using (MySqlCommand cmd = new MySqlCommand("UPDATE `acct` SET `username` = @username WHERE `discordid` = @discordid",
                    Connection))
                {
                    cmd.Parameters.AddWithValue("@discordid", discordid);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: SetNewUsername()\n{0}", e);
            }
        }
        public static bool CreateUser(string user, string password, ulong discordid)
        {
            bool success = false;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO `acct` (`username`, `password`, `discordid`)  VALUES (@user, @pass, @discordid)", Connection))
                {
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@pass", SHA2(password));
                    cmd.Parameters.AddWithValue("@discordid", discordid);
                   
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

        public static string unSHA2(string value)
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
    }
}

using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;

namespace MusicPlayList.DataBase
{
    /// <summary>
    /// DataBaseConnection class.
    /// responsible of the connection of the application to the 
    /// database.
    /// </summary>
    class DataBaseConnection
    {
        private string databaseName =  string.Empty;
        private MySqlConnection connection = null;
        public static DataBaseConnection instance { get; } = new DataBaseConnection();

        private DataBaseConnection()
        {
            string settings = ConfigurationManager.AppSettings["parameters"];
            string[] settingsArray = settings.Split(';');
            string connstring = string.Format("Server={0};database={2};uid={1};password={3}", settingsArray[3], settingsArray[4], settingsArray[5], settingsArray[6]);
            connection = new MySqlConnection(connstring);
        }

        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }

        public MySqlConnection Connection
        {
            get { return connection; }
        }

        /// <summary>
        /// connnect function.
        /// connect to database.
        /// </summary>
        /// <returns>true if connect successfully else false</returns>
        public bool connect()
        {
            try
            { 
                connection.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Close function.
        /// close the connection.
        /// </summary>
        public void Close()
        {
            try
            {
                connection.Close();
            } catch (MySqlException e)
            {
                Console.WriteLine(e.Data);
            }
        }



    }
}

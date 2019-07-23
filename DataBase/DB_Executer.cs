using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using System.Data;

namespace MusicPlayList.DataBase
{
    /// <summary>
    /// DB_Executer class.
    /// responsible of interacting with the database
    /// via varios kind of queries
    /// </summary>
    class DB_Executer
    {
        public DataBaseConnection connection = DataBaseConnection.instance;
        /// <summary>
        /// execute witouth result
        /// </summary>
        /// <param name="query"></param>
        /// <returns>return -1 if there was a problem with 
        /// the conncetion or the query, else return the number of
        /// rows that was affected by the query
        public int ExecuteCommandWithoutResult(String query)
        {
            MySqlCommand command = ResolveCommand(query);
            if (this.connection.connect())
            {
                int status;
                try
                {
                    status = command.ExecuteNonQuery(); 
                } catch (MySqlException e)
                {
                    Console.WriteLine(e.Data);
                    return -1;
                }
                this.connection.Close();
                return status;
            }
            return -1;
        }
        public int ExecuteQueryWithoutDisconnect(String query)
        {
            MySqlCommand command = ResolveCommand(query);
            int s = 0;
            try
            {
                s = command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Data);
            }
            return s; 
        }
        /// <summary>
        /// ExecuteCommandWithResults method.
        /// execute a query with expected answer as dataTable.
        /// </summary>
        /// <param name="query">query</param>
        /// <returns>DataTable of the result</returns>
        public DataTable ExecuteCommandWithResults(String query)
        {
            MySqlCommand command = ResolveCommand(query);
            if (this.connection.connect())
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                this.connection.Close();
                return dt;
            }
             catch (MySqlException e)
             {
                 Console.WriteLine(e.Data);
                 return null; 

             }
            return null;
        }


        /// <summary>
        /// ResolveCommand method.
        /// with a given string represeny SQL query, 
        /// convert it to an Appropriate MySQl Command
        /// </summary>
        /// <param name="q">query</param>
        /// <returns>MYSQL command</returns>
        private MySqlCommand ResolveCommand(String q)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = q;
            cmd.Connection = this.connection.Connection;
            return cmd;
        }
    }
}

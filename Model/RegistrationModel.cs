using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayList.DataBase;
using MySql.Data.MySqlClient;
using MusicPlayList.Entities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;

namespace MusicPlayList.Model
{
    /// <summary>
    /// RegistrationModel class.
    /// responsible of the registration process.
    /// </summary>
    class RegistrationModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DB_Executer executer = new DB_Executer();
        public User user = new User();
        /// <summary>
        /// getIDFromTable method.
        /// with a given username and password from properties - 
        /// extract his id in table.
        /// </summary>
        public void getIDFromTable()
        {
            try
            {
                String query = "SELECT idUsers FROM users WHERE user_name = '" + Username + "' AND password = '" + Password + "'";
                DataTable dt = executer.ExecuteCommandWithResults(query);
                ID = dt.Rows[0].Field<int>(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }
        }
        /// <summary>
        /// SignUp method.
        /// after getting the username and password from user, check first if it
        /// alreay exist - if yes return false. then register him in table.
        /// </summary>
        /// <returns></returns>
        public Boolean SignUp()
        {
            try
            {
                String checkIfUserExist = "Select * FROM Users WHERE user_name = '" + Username + "'";
                if (executer.ExecuteCommandWithResults(checkIfUserExist).Rows.Count != 0)
                {
                    return false;
                }
                String query = "INSERT INTO Users (user_name, password) VALUES ('" + Username + "','" + Password + "')";
                if (executer.ExecuteCommandWithoutResult(query) != -1)
                {
                    getIDFromTable();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }
            return false;
                
        }
        /// <summary>
        /// ConvertToJson method.
        /// convert this window's parameters into a Jarray.
        /// </summary>
        /// <returns>Jarray with param</returns>
        public JArray ConvertToJson()
        {
            JArray arr = new JArray();
            arr.Add(JsonConvert.SerializeObject(this.user));
            return arr;
        }
        /// <summary>
        /// ConvertFromJson method
        /// </summary>
        public void ConvertFromJson()
        {
            ;
        }
        /// <summary>
        /// User id.
        /// </summary>
        public int ID
        {
            get
            {
                return user.ID;
            }
            set
            {
                user.ID = value;
            }
        }
        /// <summary>
        /// Password.
        /// </summary>
        public String Password
        {
            get
            {
                return user.Password;
            }
            set
            {
                user.Password = value;
            }
        }
        /// <summary>
        /// Username
        /// </summary>
        public String Username
        {
            get
            {
                return user.Name;
            }
            set
            {
                this.user.Name = value;
            }
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.Entities
{
    class User
    {
        private int user_id;
        private string name;
        private string password;

        public int ID
        {
            get
            {
                return user_id;
            }
            set
            {
                user_id = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                this.password = value;
            }
        }
        public User() { }

    }
}

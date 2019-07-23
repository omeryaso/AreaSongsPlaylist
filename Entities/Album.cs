using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.Entities
{
    class Album
    {
        private String name;
        private int album_id;

        public int ID
        {
            get
            {
                return album_id;
            }
            set
            {
                album_id = value;
            }
        }
        public String Name
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
    }
}

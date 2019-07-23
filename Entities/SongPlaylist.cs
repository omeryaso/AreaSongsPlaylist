using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.Entities
{
    class SongPlaylist
    {
        private ObservableCollection<Song> songsList;
        private int playlist_id;
        private User user;
        
        public SongPlaylist(SongPlaylist play)
        {
            this.ID = play.ID;
            this.Songs = new ObservableCollection<Song>(play.Songs);
            this.User = play.User; 
        }

        public SongPlaylist() { }


        public User User
        {
            get
            {
                return user;
            } 
            set
            {
                user = value;
            }
        }
        public int ID
        {
            get
            {
                return playlist_id;
            }
            set
            {
                playlist_id = value;
            }
        }
        public ObservableCollection<Song> Songs
        {
            get
            {
                return songsList;
            }
            set
            {
                songsList = value;
            }
        }
    }
}

using MusicPlayList.DataBase;
using MusicPlayList.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.Model
{
    /// <summary>
    /// LoginModel class.
    /// responsible of the login process (with option to sign up).
    /// </summary>
    class LoginModel : INotifyPropertyChanged
    {
        private DB_Executer dataBase = new DB_Executer();
        private User user = new User();
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// user.
        /// </summary>
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
        /// <summary>
        /// FindUser method.
        /// bind from the user the username and password, and confirm if
        /// he exists in table or not.
        /// </summary>
        /// <returns></returns>
        public bool FindUser()
        {
            try
            {
                String query = "SELECT * FROM users WHERE users.user_name = '" + User.Name + "' AND users.password = '" + User.Password + "'";
                DataTable dt = dataBase.ExecuteCommandWithResults(query);
                if (dt.Rows.Count != 0)
                {
                    user.ID = dt.Rows[0].Field<int>(0);
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
        /// specifically retrieve the user's playlist and convert
        /// it to Json
        /// </summary>
        /// <returns>Jarray with param</returns>
        public JArray ConvertToJson()
        {
            JArray j = new JArray();
            DataTable dt = dataBase.ExecuteCommandWithResults(getPlaylistString());
            ObservableCollection<string> temp = JsonConvert.DeserializeObject<ObservableCollection<string>>(QueryInterpreter.Instance.getQueryEntitesObject(QueryInterpreter.QueryType.ResolveInitialPlaylist, dt));
            ObservableCollection<Song> songs = new ObservableCollection<Song>();
            SongPlaylist playList = new SongPlaylist();
            playList.User = user;
            foreach (string item in temp)
            {
                songs.Add(JsonConvert.DeserializeObject<Song>(item));
            }
            playList.Songs = songs;

            j.Add(JsonConvert.SerializeObject(playList));
            return j;
        }
        /// <summary>
        /// ConvertFromJson
        /// </summary>
        /// <param name="j"></param>
        public void ConvertFromJson(JArray j)
        {
        }
        /// <summary>
        /// getPlaylistString.
        /// returns the string represent the query that
        /// retrieve the user's playlist.
        /// </summary>
        /// <returns>string query</returns>
        private string getPlaylistString()
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append("SELECT idSongs, song_name,year,song_hotness, song_duration, song_tempo, idArtists,artist_name, genre , album_name, idAlbum ");
                query.Append("from songs join (select Songs_idSongs from songs_in_playlist where Playlist_Users_idUsers = " + user.ID + ") as play_list ");
                query.Append("join artists join album ");
                query.Append("where play_list.Songs_idSongs = idSongs and idArtists = songs.Artists_idArtists and idAlbum = songs.Album_idAlbum GROUP BY idSongs order by idSongs");
                return query.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }
            return null;
        }
    }
}

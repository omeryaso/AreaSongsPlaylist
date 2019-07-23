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
    /// CountryChooserModel class.
    /// represent of displaying the Closest area's to 
    /// the spot that the user choose, and according to his choice
    /// generate initial playlist
    /// </summary>
    class CountryChooserModel : INotifyPropertyChanged
    {
        private User user;
        public DB_Executer executer = new DB_Executer();
        private Dictionary<Area, int> AreasAndNumberOfSongs = new Dictionary<Area, int>();
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ExtensionInfo> Areas_count = new ObservableCollection<ExtensionInfo>();



        private SongPlaylist model_playlist = new SongPlaylist();
        /// <summary>
        /// ConvertFromJson method.
        /// convert the Jarray token's into Parameters
        /// that the previus windows sent, and config
        /// this window's Model Properties
        /// </summary>
        /// <param name="j">Jarray</param>
        public void ConvertFromJson(JArray j)
        {
            Areas_count.Clear();
            User = JsonConvert.DeserializeObject<User>(j[0].ToString());
            AreaToNumberOfSongs.Clear();
            Dictionary<string, int> tempDic = JsonConvert.DeserializeObject<Dictionary<string, int>>(j[1].ToString());
            foreach (string item in tempDic.Keys)
            {
                int count;
                tempDic.TryGetValue(item, out count);
                Area temp = JsonConvert.DeserializeObject<Area>(item);
                AreasAndNumberOfSongs.Add(temp, count);
            }
            CreateAreasList();
            
        }
        /// <summary>
        /// CreateAreasList method.
        /// build the Area's count list according to
        /// info in Dictionary
        /// </summary>
        private void CreateAreasList()
        {
            foreach (Area item in AreasAndNumberOfSongs.Keys)
            {
                int count;
                AreasAndNumberOfSongs.TryGetValue(item, out count);
                Areas_count.Add(new ExtensionInfo(item.AreaName, count));
            }
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Areas_count"));

        }


        /// <summary>
        /// KeysToString method.
        /// take the area's that were checked by user
        /// and make from them a string include
        /// the area's id.
        /// </summary>
        /// <returns>string</returns>
        private String KeysToString()
        {
            StringBuilder build = new StringBuilder();
            build.Append("");
            bool flag = true;
            foreach(ExtensionInfo ex in Areas_count)
            {
                if (ex.IsChecked)
                {
                    foreach (Area a in AreaToNumberOfSongs.Keys)
                    {
                        if(a.AreaName.Equals(ex.Extension))
                        {
                            if(!flag)
                            {
                                build.Append("," + a.Id.ToString());
                            }
                            else
                            {
                                build.Append(a.Id.ToString());
                                flag = false;
                            }
                            
                        }
                    }
                }
            }
            return build.ToString();
        }
        /// <summary>
        /// CreateInitPlaylist method.
        /// according to the areas that the user choose, create
        /// a playlist that is built from all songs that are from
        /// there areas. execute a query to resolve that and then convert
        /// the results to the appropriate fields.
        /// </summary>
        /// <returns>true if the user choose at least one area, else false</returns>
        public bool CreateInitPlaylist()
        {

            string str = KeysToString();
            if(String.IsNullOrEmpty(str))
            {
                return false;
            }
            StringBuilder query = new StringBuilder();
            query.Append("SELECT idSongs,song_name,year,song_hotness,song_duration,song_tempo,artists_from_areas.idArtists,artists_from_areas.artist_name,artists_from_areas.genre,album_name,idAlbum ");
            query.Append("FROM songs JOIN (SELECT idArtists, artist_name, genre FROM artists WHERE Area_LocationId in (" + str + ") GROUP BY idArtists) AS artists_from_areas ");
            query.Append("join album ");
            query.Append("WHERE songs.Artists_idArtists = artists_from_areas.idArtists and songs.Album_idAlbum = album.idAlbum GROUP BY idSongs order by idSongs");
            try
            {
                DataTable dt = executer.ExecuteCommandWithResults(query.ToString());
                ObservableCollection<string> list = JsonConvert.DeserializeObject<ObservableCollection<string>>(QueryInterpreter.Instance.getQueryEntitesObject(QueryInterpreter.QueryType.ResolveInitialPlaylist, dt));
                ObservableCollection<Song> songs = new ObservableCollection<Song>();
                foreach (string item in list)
                {
                    songs.Add(JsonConvert.DeserializeObject<Song>(item));
                }
                model_playlist.Songs = songs;
                model_playlist.User = User;
                return true;
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Data);
            }
            return false;

        }
        /// <summary>
        /// ConvertToJson method.
        /// convert this window's parameters into a Jaraay.
        /// </summary>
        /// <returns>Jarray with param</returns>
        public JArray ConvertToJson()
        {
            JArray arr = new JArray();
            arr.Add(JsonConvert.SerializeObject(model_playlist));
            return arr;
        }
        /// <summary>
        /// mapping between Area and the number of song in each area
        /// </summary>
        public Dictionary<Area, int> AreaToNumberOfSongs
        {
            get
            {
                return AreasAndNumberOfSongs;
            }
            set
            {
                AreasAndNumberOfSongs = value;
            }
        }
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
        /// GetRandomAreas method.
        /// genereate a playlist randomaly
        /// </summary>
        public void GetRandomAreas()
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append("SELECT idSongs,song_name,year,song_hotness,song_duration,song_tempo,artists_from_areas.idArtists,artists_from_areas.artist_name,artists_from_areas.genre,album_name,idAlbum ");
                query.Append("FROM songs JOIN (SELECT idArtists, artist_name, genre FROM artists WHERE Area_LocationId in " + CheckForRandomAreas() + " GROUP BY idArtists) AS artists_from_areas ");
                query.Append("join album ");
                query.Append("WHERE songs.Artists_idArtists = artists_from_areas.idArtists and songs.Album_idAlbum = album.idAlbum GROUP BY idSongs order by idSongs");
                DataTable dt = executer.ExecuteCommandWithResults(query.ToString());
                ObservableCollection<string> list = JsonConvert.DeserializeObject<ObservableCollection<string>>(QueryInterpreter.Instance.getQueryEntitesObject(QueryInterpreter.QueryType.ResolveInitialPlaylist, dt));
                ObservableCollection<Song> songs = new ObservableCollection<Song>();
                foreach (string item in list)
                {
                    songs.Add(JsonConvert.DeserializeObject<Song>(item));
                }
                model_playlist.Songs = songs;
                model_playlist.User = User;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }
        }
        /// <summary>
        /// CheckForRandomAreas method.
        /// select from database locationID that have an Unknown latitude and
        /// longitude
        /// </summary>
        /// <returns></returns>
        public string CheckForRandomAreas()
        {
            try
            {
                StringBuilder q = new StringBuilder();
                q.Append("SELECT LocationId ");
                q.Append("FROM area WHERE area.longitude = 0 AND area.latitude = 0 order by rand() limit 7");
                DataTable dt = executer.ExecuteCommandWithResults(q.ToString());
                StringBuilder areasID = new StringBuilder();
                bool flag = true;
                areasID.Append("(");
                foreach (DataRow d in dt.Rows)
                {
                    if (!flag)
                    {
                        areasID.Append("," + d.Field<int>(0).ToString());
                    }
                    else
                    {
                        areasID.Append(d.Field<int>(0).ToString());
                        flag = false;
                    }
                }
                areasID.Append(")");
                return areasID.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayList.Entities;
using MusicPlayList.DataBase;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Data;

namespace MusicPlayList.Model
{
    /// <summary>
    /// LoactionMapChooserModel class.
    /// responsible of showing the user the world map, and when
    /// he press a point and submit - generate the 8 closest area's 
    /// to this point.
    /// </summary>
    class LocationMapChooserModel : INotifyPropertyChanged
    {
        public Map map = new Map();
        public event PropertyChangedEventHandler PropertyChanged;
        public DB_Executer executer = new DB_Executer();
        private Area area = new Area();
        private User user;
        private Dictionary<Area, int> mapper = new Dictionary<Area, int>();
        private ObservableCollection<String> areasName = new ObservableCollection<string>();

        /// <summary>
        /// CalculateAreaProps method.
        /// with a given x,y pixels, calculate the latitude and longtitude
        /// values.
        /// </summary>
        /// <param name="xVal">x value</param>
        /// <param name="yVal">y value</param>
        /// <param name="mapMinHeight">margin of window</param>
        /// <param name="mapMinWidth">margin of window</param>
        public void CalculateAreaProps(Double xVal, Double yVal, Double mapMinHeight, Double mapMinWidth)
        {
            //from xaml
            double[] dif = map.getMapSizeDiffernce();
            double x = (xVal - mapMinWidth) / dif[0];
            double y = (yVal - mapMinHeight) / dif[1];
            map.fromPixelToCoordinates(x, y);
            area.Longtitude = map.CurrentLongitude;
            area.Latitude = map.CurrentLatitude;
        }
        /// <summary>
        /// CheckForClosestCountries method.
        /// According to the longtitude and latitude that were
        /// calculated earlier - take from table the 8 most close
        /// areas to the latitude and longtitue point according
        /// to a well known formula
        /// </summary>
        public void CheckForClosestCountries()
        {
            try
            {
                StringBuilder subQuery = new StringBuilder();
                subQuery.Append("Select LocationId, location_name, count(location_name) FROM ");
                subQuery.Append("(SELECT area.LocationId,area.location_name FROM area ");
                subQuery.Append("WHERE area.latitude != 0 AND area.longitude != 0 ");
                subQuery.Append("GROUP BY area.location_name ");
                subQuery.Append("order by (6371 * acos( cos( radians(area.latitude) ) * cos( radians(" + area.Latitude.ToString() + ")) ");
                subQuery.Append("* cos( radians(" + Area.Longtitude.ToString() + ") - radians(area.longitude) ) + sin( radians(area.latitude) ) * sin(radians(" + Area.Latitude.ToString() + ")))) Asc ");
                subQuery.Append("LIMIT 20) AS country ");
                subQuery.Append("JOIN artists JOIN Songs ");
                subQuery.Append("WHERE songs.artists_idArtists = artists.idArtists AND artists.Area_LocationId = country.LocationId ");
                subQuery.Append("GROUP BY location_name order by count(location_name) desc Limit 10");
                DataTable set = this.executer.ExecuteCommandWithResults(subQuery.ToString());
                string ans = QueryInterpreter.Instance.getQueryEntitesObject(QueryInterpreter.QueryType.AreaSongsCount, set);
                Dictionary<string, int> tempDic = JsonConvert.DeserializeObject<Dictionary<string, int>>(ans);
                foreach (string item in tempDic.Keys)
                {
                    int count;
                    tempDic.TryGetValue(item, out count);
                    Mapper.Add(JsonConvert.DeserializeObject<Area>(item), count);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }

        }
        /// <summary>
        /// mapping between area int.
        /// </summary>
        public Dictionary<Area, int> Mapper
        {
            get
            {
                return mapper;
            }
            set
            {
                this.mapper = value;
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
        /// Area.
        /// </summary>
        public Area Area
        {
            get
            {
                return area;
            }
            set
            {
                area = value;
            }
        }
        /// <summary>
        /// ConvertToJson method.
        /// convert this window's parameters into a Jarray.
        /// </summary>
        /// <returns>Jarray with param</returns>
        public JArray ConvertToJson()
        {
            JArray j = new JArray();
            j.Add(JsonConvert.SerializeObject(User));
            Dictionary<string, int> tempDict = new Dictionary<string, int>();
            foreach(Area item in Mapper.Keys)
            {
                int count;
                Mapper.TryGetValue(item, out count);
                tempDict.Add(JsonConvert.SerializeObject(item), count);
            }
            j.Add(JsonConvert.SerializeObject(tempDict));
            Mapper.Clear();
            return j;
        }
        /// <summary>
        /// ConvertFromJson method.
        /// take from the jarray the jtokens and convert them
        /// into this appropriate parameters
        /// </summary>
        /// <param name="j">Jarray</param>
        public void ConvertFromJson(JArray j)
        {
            User = JsonConvert.DeserializeObject<User>(j[0].ToString());
        }

    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.Entities
{
    class QueryInterpreter
    {
        public enum QueryType { ResolveInitialPlaylist, AreaSongsCount};

        /// <summary>
        /// private constructor - for singelton.
        /// </summary>
        private QueryInterpreter() { }
        public static QueryInterpreter Instance { get; } = new QueryInterpreter();

        /// <summary>
        /// returns the right paramaters from the given datatable and query type.
        /// </summary>
        /// <param name="q"></param>
        /// <param name="dt"></param>
        public string getQueryEntitesObject(QueryType q, DataTable dt)
        {
            switch(q)
            {
                case QueryType.AreaSongsCount:
                    {
                        return GetAreaSongsCount(dt);
                    }
                case QueryType.ResolveInitialPlaylist:
                    {
                        return getSongsForPlayList(dt);
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        /// <summary>
        /// get a map converted to json by string with the areas and number of song in each one.
        /// </summary>
        /// <param name="dt"></param>
        private string GetAreaSongsCount(DataTable dt)
        {
            Dictionary<string, int> song_in_area = new Dictionary<string, int>();
            foreach (DataRow row in dt.Rows)
            {
                Area a = new Area();
                a.Id = row.Field<int>(0);
                a.AreaName = row.Field<string>(1);
                int count = (int)row.Field<Int64>(2);
                song_in_area.Add(JsonConvert.SerializeObject(a), count);
            }
             string json = JsonConvert.SerializeObject(song_in_area, Formatting.Indented);
            return json;
        }

        /// <summary>
        /// returns playlist as json string from datatable.
        /// </summary>
        /// <param name="dt"></param>
        private string getSongsForPlayList(DataTable dt)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();

            foreach (DataRow row in dt.Rows)
            {
                Song sng = new Song();
                sng.ID = row.Field<int>(0);
                sng.Name = row.Field<string>(1);
                sng.Year = row.Field<int>(2).ToString();
                sng.Hotness = row.Field<float>(3);
                sng.Duration = row.Field<float>(4);
                sng.Tempo = row.Field<float>(5);
                sng.Artist.ID = row.Field<string>(6);
                sng.Artist.Name = row.Field<string>(7);
                sng.Artist.Genre = row.Field<String>(8);
                sng.Album.ID = row.Field<int>(10);
                sng.Album.Name = row.Field<string>(9);
                list.Add(JsonConvert.SerializeObject(sng));
            }
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }
    }
}

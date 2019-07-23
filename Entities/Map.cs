using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MusicPlayList.Entities
{
    class Map
    {
        private const double mapWidthdef = 360;
        private const double mapHeightdef = 180;
        private int mapWidth;
        private int mapHeight;
        private string mapImagePath;

        /// <summary>
        /// map constructor, gets info from appconfig.
        /// </summary>
        public Map()
        {
            string settings = ConfigurationManager.AppSettings["parameters"];
            string[] settingsArray = settings.Split(';');
            mapImagePath = settingsArray[0];
            Int32.TryParse(settingsArray[2], out mapHeight);
            Int32.TryParse(settingsArray[1], out mapWidth);
            CurrentLatitude = 0;
            CurrentLongitude = 0;
        }

        public int MapHeight
        {
            get
            {
                return this.mapHeight;
            }
        }

        public int MapWidth
        {
            get
            {
                return this.mapWidth;
            }
        }

        public string MapImagePath
        {
            get
            {
                return this.mapImagePath;
            }
        }

        public double CurrentLatitude
        {
            get; set;
        }

        public double CurrentLongitude
        {
            get; set;
        }

        /// <summary>
        /// calculates latitude and longtiude from pixels.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void fromPixelToCoordinates(double x, double y)
        {
            CurrentLongitude = x - 180;
            CurrentLatitude = 90 - y;
        }

        /// <summary>
        /// returns map differnce.
        /// </summary>
        public double[] getMapSizeDiffernce()
        {
            double[] dif = { MapWidth / mapWidthdef, MapHeight / mapHeightdef };
            return dif;
        }
    }
}

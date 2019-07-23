using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.Entities
{
    class Area
    {
        private String areaName;
        private Double lattiude;
        private Double longtitude;
        private int id;

        public String AreaName
        {
            get
            {
                return areaName;
            }
            set
            {
                this.areaName = value;
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                this.id = value;
            }
        }

        public Double Latitude
        {
            get
            {
                return lattiude;
            }
            set
            {
                this.lattiude = value;
            }
        }
        public Double Longtitude
        {
            get
            {
                return longtitude;
            }
            set
            {
                this.longtitude = value;
            }
        }
    }
}

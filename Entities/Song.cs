using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.Entities
{
    class Song
    {
        private int song_id;
        private String name;
        private int year;
        private float hotness;
        private string hotness_stars = "";
        private float duration;
        private float tempo;
        private Artist artist = new Artist();
        private Album album = new Album();


        public Album Album
        {
            get
            {
                return album;
            }
            set
            {
                album = value;
            }
        }

        public int ID
        {
            get
            {
                return song_id;
            }
            set
            {
                song_id = value;
            }
        
        }
        public Artist Artist
        {
            get
            {
                return artist;
            }
            set
            {
                artist = value;
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
        public string Year
        {
            get
            {
                if(year == 0)
                {
                    return "Unknown";
                }
                return year.ToString();
            }
            set
            {
                 Int32.TryParse(value, out this.year);
            }
        }

        public string HotnessStar
        {
            get
            {
                return hotness_stars;
            }
            set
            {
                hotness_stars = value;
            }
        }

        public float Hotness
        {
            get
            {
                return this.hotness;
            }
            set
            {
                this.hotness = value;
                HotnessConvert convert = new HotnessConvert();
                HotnessStar = convert.convert(value);
            }
        }
        public float Duration
        {
            get
            {
                return duration;
            }
            set
            {
                this.duration = value;
            }
        }
        public float Tempo
        {
            get
            {
                return tempo;
            }
            set
            {
                this.tempo = value;
            }
        }
    }
}

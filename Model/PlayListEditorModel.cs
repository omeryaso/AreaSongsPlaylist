using MusicPlayList.DataBase;
using MusicPlayList.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.Model
{
    /// <summary>
    /// PlayListEditorModel
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    class PlayListEditorModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The original playlist fromthe data base
        /// </summary>
        private SongPlaylist full_playlist;
        /// <summary>
        /// The model playlist from Playlist
        /// </summary>
        private SongPlaylist model_playlist;
        /// <summary>
        /// The working playlist
        /// </summary>
        private SongPlaylist new_playlist;
        /// <summary>
        /// The data base
        /// </summary>
        private DB_Executer dataBase;
        private Song songRemove;
        private ObservableCollection<ExtensionInfo> tempoList;
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayListEditorModel"/> class.
        /// </summary>
        public PlayListEditorModel()
        {
            dataBase = new DB_Executer();
        }

        public ObservableCollection<ExtensionInfo> CurrentGeners
        {
            get;set;
        }

        public Song SongRemove
        {
            get
            {
                return songRemove;
            }
            set
            {
                songRemove = value;
            }
        }

        public ObservableCollection<ExtensionInfo> TempoList
        {
            get
            {
                return tempoList;
            }
            set
            {
                tempoList = value;
            }
        }


        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the original play list.
        /// </summary>
        /// <returns></returns>
        public SongPlaylist OriginalPlayList
        {
            get
            {
                return model_playlist;
            }
            set
            {
                model_playlist = value;
            }

        }
        /// <summary>
        /// gets the current playlist.
        /// </summary>
        public SongPlaylist CurrentPlayList
        {
            get
            {
                return new_playlist;
            }
            set
            {
                new_playlist = value;
            }

        }

        public SongPlaylist CopyCurrentPlayList
        {
            get
            {
                return full_playlist;
            }
            set
            {
                full_playlist = value;
            }
        }
        /// <summary>
        /// Filter method.
        /// filter playlist according to user choice.
        /// first check for generes that user want, than clear
        /// the collection that the playlist is based on and add
        /// the appropriate songs to the playlist, and notify the viewmodel
        /// </summary>
        public void Filter()
        {
            List<string> genres = choosenGenres();
            List<string> tempoes = chosenTempo();
            if (genres.Count == 0 && tempoes.Count == 0)
            {
                return;
            }
            ObservableCollection<Song> temp = new ObservableCollection<Song>(CurrentPlayList.Songs);
            CurrentPlayList.Songs.Clear();
            foreach (Song song in temp)
            {
                if(genres.Count == 0 && tempoes.Count != 0)
                {
                    string tempo = tempoType(song);
                    if(tempoes.Contains(tempo))
                    {
                        CurrentPlayList.Songs.Add(song);
                        continue;
                    }
                }
                if (genres.Contains(song.Artist.Genre))
                {
                    if (tempoes.Count == 0)
                    {
                        CurrentPlayList.Songs.Add(song);
                    } 
                    else
                    {
                        string tempo = tempoType(song);
                        if (tempoes.Contains(tempo))
                        {
                            CurrentPlayList.Songs.Add(song);
                        }
                    }

                }
            }

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VM_GetPlayList"));
            CurrentGeners.Clear();
            resolveGenres(CurrentPlayList);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vm_CurrentGenres"));
            TempoList.Clear();
            resolveTempo(CurrentPlayList);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vm_TempoList"));

        }
        /// <summary>
        /// choosenGeneres method.
        /// retrieve from user the genere that will
        /// be in the edited playlist
        /// </summary>
        /// <returns>list of strings (genres)</returns>
        private List<string> choosenGenres()
        {
            List<String> genres = new List<string>(); 
            foreach(ExtensionInfo ext in CurrentGeners)
            {
                if(ext.IsChecked)
                {
                    genres.Add(ext.Extension);
                    ext.IsChecked = false;
                }
            }
            return genres;
        }

        /// <summary>
        /// returns list of tempoes.
        /// </summary>
        /// <returns>list of strings (genres)</returns>
        private List<string> chosenTempo()
        {
            List<string> tempoes = new List<string>();
            foreach (ExtensionInfo ext in TempoList)
            {
                if (ext.IsChecked)
                {
                    tempoes.Add(ext.Extension);
                }
            }
            return tempoes;
        }


        /// <summary>
        /// reset method.
        /// change the current playlist that is showed to the user 
        /// to the original playlist that he entered with to the
        /// editor.
        /// </summary>
        public void reset()
        {
            CurrentPlayList.Songs.Clear();
            CurrentGeners.Clear();
            TempoList.Clear();
            foreach(Song item in OriginalPlayList.Songs)
            {
                CurrentPlayList.Songs.Add(item);
            }
            resolveGenres(CurrentPlayList);
            resolveTempo(CurrentPlayList);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VM_GetPlayList"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vm_CurrentGenres"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vm_TempoList"));

        }

        /// <summary>
        /// ConvertToJson method.
        /// convert this window's parameters into a Jarray.
        /// </summary>
        /// <returns>Jarray with param</returns>
        public JArray ConvertToJson()
        {
            JArray j = new JArray();
            j.Add(JsonConvert.SerializeObject(CurrentPlayList));
            return j;
        }
        /// <summary>
        /// ConvertFromJson method.
        /// convert from jarray jtokens to the appropriate 
        /// params for this window
        /// </summary>
        /// <param name="j"></param>
        public void ConvertFromJson(JArray j)
        {
            OriginalPlayList = JsonConvert.DeserializeObject<SongPlaylist>(j[0].ToString());
            CurrentPlayList = new SongPlaylist(OriginalPlayList);
            CopyCurrentPlayList = new SongPlaylist(CurrentPlayList);
            CurrentGeners = new ObservableCollection<ExtensionInfo>();
            TempoList = new ObservableCollection<ExtensionInfo>();
            resolveGenres(CurrentPlayList);
            resolveTempo(CurrentPlayList);
        }

        /// <summary>
        /// resolveGeners method.
        /// resolve the generes from a given playlist
        /// </summary>
        /// <param name="playList">playlist</param>
        public void resolveGenres(SongPlaylist playList)
        {
            bool flag = true;
            foreach (Song item in playList.Songs)
            {
                foreach (ExtensionInfo ex in CurrentGeners)
                {
                    if(ex.Extension.Equals(item.Artist.Genre))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    CurrentGeners.Add(new ExtensionInfo(item.Artist.Genre, 0));
                }
                flag = true;

            }
        }

        /// <summary>
        /// returns songs tempo type as a string.
        /// </summary>
        /// <param name="song">Song</param>
        private string tempoType(Song song)
        {
            string tempo = null;
            if (song.Tempo >= 0 && song.Tempo < 95)
            {
                tempo = "calm";
                return tempo;
            }
            if (song.Tempo >= 95 && song.Tempo < 185)
            {
                tempo = "normal";
            }
            else 
            {
                tempo = "rhythmic";
            }
            return tempo;
        }

        /// <summary>
        /// check what tympo types in the songs the are in the current playList.
        /// </summary>
        /// <param name="playList">SongPlaylist</param>
        private void resolveTempo(SongPlaylist playList)
        {
            bool flag = true;
            foreach (Song item in playList.Songs)
            {
                string tempo = tempoType(item);
                foreach (ExtensionInfo ex in TempoList)
                {
                    if (ex.Extension.Equals(tempo))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    TempoList.Add(new ExtensionInfo(tempo, 0));
                }
                flag = true;

            }

        }


        /// <summary>
        /// remove the choosen song from the playlist.
        /// </summary>
        public void RemoveSong()
        {
            if(SongRemove != null)
            {
                foreach (Song item in CurrentPlayList.Songs)
                {
                    if (item.ID == SongRemove.ID)
                    {
                        CurrentPlayList.Songs.Remove(item);
                        break;
                    }
                }
                CurrentGeners.Clear();
                TempoList.Clear();
                resolveGenres(CurrentPlayList);
                resolveTempo(CurrentPlayList);
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VM_GetPlayList"));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vm_CurrentGenres"));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vm_TempoList"));
            }

        }

    }

}
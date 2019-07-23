using MusicPlayList.Entities;
using MusicPlayList.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.ViewModel
{
    class PlayListVM : IVM
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private PlayListModel playList_model;
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayListEditorVM"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public PlayListVM()
        {
            this.playList_model = new PlayListModel();
            this.playList_model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    this.PropertyChanged?.Invoke(this, e);
                };
        }

        public ObservableCollection<Song> VM_LogsList {
            get
            {
                return playList_model.Playlist.Songs;
            }
            set
            {
                playList_model.Playlist.Songs = value;
                NotifyPropertyChanged("VM_LogsList");
            }
        }

        public ObservableCollection<Song> VM_CopyLogsList
        {
            get
            {
                return playList_model.copyPlaylist.Songs;
            }
            set
            {
                playList_model.copyPlaylist.Songs = value;
                NotifyPropertyChanged("VM_LogsList");
            }
        }



        override
        public void SendParameters()
        {
            BaseVM.GetInstance.SendParam(BaseVM.ViewModels.PlayList, BaseVM.ViewModels.PlayListEditor);
        }
        override
        public JArray GetParameters()
        {
            return playList_model.ConvertToJson();
        }
        override
        public void RecivedParameters(JArray arr)
        {
            playList_model.ConvertFromJson(arr);
        }

        /// <summary>
        /// save the playlist to the data base and exit the playlist.
        /// </summary>
        public void SaveAndExit()
        {
            this.playList_model.savePlaylist();

        }
    }
}

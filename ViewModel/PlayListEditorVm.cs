using MusicPlayList.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MusicPlayList.Entities;
using Prism.Commands;
using System.Windows.Input;

namespace MusicPlayList.ViewModel
{
    class PlayListEditorVM : IVM
    {
        private PlayListEditorModel model;
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayListEditorVM"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public PlayListEditorVM()
        {
            this.model = new PlayListEditorModel();
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    this.NotifyPropertyChanged("VM_" + e.PropertyName);
                };
            this.Remove = new DelegateCommand<object>(this.onRemove, this.canBeRemoved);
        }

        public ObservableCollection<ExtensionInfo> Vm_CurrentGenres
        {
            get
            {
                return model.CurrentGeners;
            }
            set
            {
                model.CurrentGeners = value;
                NotifyPropertyChanged("Vm_CurrentGenres");
            }
        }
        public ObservableCollection<ExtensionInfo> Vm_TempoList
        {
            get
            {
                return model.TempoList;
            }
            set
            {
                model.TempoList = value;
                NotifyPropertyChanged("Vm_TempoList");
            }
        }

        public SongPlaylist VM_GetPlayList
        {
            get { return model.CurrentPlayList; }
            set
            {
                this.model.CurrentPlayList = value;
                NotifyPropertyChanged("VM_GetPlayList");
            }
        }

        /// <summary>
        /// reset the changes in the playlist.
        /// </summary>
        public void reset()
        {
            model.reset();
        }

        /// <summary>
        /// make the changes on the playlist without saving it.
        /// </summary>
        public void Filter()
        {
            this.model.Filter();
        }

        override
        public void SendParameters()
        {
            BaseVM.GetInstance.SendParam(BaseVM.ViewModels.PlayListEditor, BaseVM.ViewModels.PlayList);
        }


        public ICommand Remove { get; private set; }


        /// <summary>
        /// remove the choosen song from playlist.
        /// </summary>
        private void onRemove(object obj)
        {
            model.RemoveSong();
        }

        public Song RemoveSong
        {
            get
            {
                return model.SongRemove;
            }
            set
            {
                this.model.SongRemove = value;
                var command = this.Remove as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
                NotifyPropertyChanged("RemoveSong");
            }
        }

        override
        public JArray GetParameters()
        {
            return model.ConvertToJson();
        }

        /// <summary>
        /// check if can be removed
        /// </summary>
        private bool canBeRemoved(object obj)
        {
            return true;
        }

        override
        public void RecivedParameters(JArray arr)
        {
            model.ConvertFromJson(arr);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace MusicPlayList.ViewModel
{
    abstract class IVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        

        public void NotifyPropertyChanged(string propname)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
            }
        }
        /// <summary>
        /// send paramaters to next view model.
        /// </summary>
        public abstract void SendParameters();


        /// <summary>
        /// convert the params to Jarray and returns it.
        /// </summary>
        public abstract JArray GetParameters();

        /// <summary>
        /// get params from previous viewmoedl.
        /// </summary>
        /// <param name="arr"></param>
        public abstract void RecivedParameters(JArray arr);
    }
}

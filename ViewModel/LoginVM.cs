using MusicPlayList.Entities;
using MusicPlayList.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.ViewModel
{
    class LoginVM : IVM
    {
        private LoginModel model;
        private string error = "";
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginVM"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public LoginVM()
        {
            this.model =  new LoginModel();
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                   this.PropertyChanged?.Invoke(this, e);
                };
        }

        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                NotifyPropertyChanged("Error");
            }
        }
        public string Name
        {
            get { return model.User.Name; }
            set
            {
                model.User.Name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string Password
        {
            get
            {
                return model.User.Password;
            }
            set
            {
                model.User.Password = value;
                NotifyPropertyChanged("Password");
            }
        }

        /// <summary>
        /// check if the username exist with the right password, returns true or false.
        /// </summary>
        public Boolean Confirm()
        {
           if(!model.FindUser())
            {
                Error = "invalid Username or Password";
                return false;
            }
            SendParameters();
            clear();
            return true;
        }
        /// <summary>
        /// clear the boxes and the error box.
        /// </summary>
        public void clear()
        {
            Password = "";
            Name = "";
            Error = "";
        }

        override
        public void SendParameters()
        {
            BaseVM.GetInstance.SendParam(BaseVM.ViewModels.Login, BaseVM.ViewModels.PlayList);
        }

        override
        public JArray GetParameters()
        {
            return model.ConvertToJson();

        }

        override
        public void RecivedParameters(JArray arr)
        {
            model.ConvertFromJson(arr);
        }

    }
}

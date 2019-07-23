using MusicPlayList.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MusicPlayList.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MusicPlayList.ViewModel
{
    class RegistrationVM : IVM
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RegistrationModel model;
        private string error = "";
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
                NotifyPropertyChanged("Error");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationVM"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public RegistrationVM()
        {
            this.model = new RegistrationModel();
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    this.PropertyChanged?.Invoke(this, e);
                };
        }


        /// <summary>
        /// check if the user name is exist or the user gave both the username and password.
        /// returns teue if yes else false and shows an error.
        /// creates the user if true.
        /// </summary>
        public Boolean CheckRegistraion()
        {
            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
            {
                Error = "One or More Fields missing";
                return false;
            }
            if (model.SignUp())
            {
                SendParameters();
                resetinput();  
                return true;
            }
            else
            {
                Error = "Username is already exists";
                return false;
            }
        }

        /// <summary>
        /// clear the boxes.
        /// </summary>
        public void resetinput()
        {
            Username = "";
            Password = "";
            Error = "";
        }

        override
        public void SendParameters()
        {
            BaseVM.GetInstance.SendParam(BaseVM.ViewModels.Registration, BaseVM.ViewModels.LocationMap);
        }
        override
        public JArray GetParameters() 
        {
            return model.ConvertToJson();
            
        }
        override
        public void RecivedParameters(JArray a)
        {
            ;
        }
        public String Username
        {
            get
            {
                return model.Username;
            }
            set
            {
                this.model.Username = value;
                NotifyPropertyChanged("UserName");
            }
        }
        public String Password
        {
            get
            {
                return model.Password;
            }
            set
            {
                this.model.Password = value;
                NotifyPropertyChanged("Password");
            }
        }
    }
}

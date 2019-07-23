using MusicPlayList.Entities;
using MusicPlayList.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MusicPlayList.ViewModel
{
    class LocationMapChooserVM : IVM
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private LocationMapChooserModel model;
        private bool isClicked = false;
        public Boolean CircleFlag { get; set;}
        public Thickness EllipseMargin { get; set; } = new Thickness();

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationMapChooserVM"/> class.
        /// </summary>
        public LocationMapChooserVM()
        {
            CircleFlag = false;
            this.model = new LocationMapChooserModel();
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    this.PropertyChanged?.Invoke(this, e);
                };
        }

        public string MapImgPath
        {
            get
            {
                return "/Resources/greenMap.png";
            }
            set
            {
                MapImgPath = "/Resources/greenMap.png";
                NotifyPropertyChanged("MapImgPath");
            }
        }

        /// <summary>
        /// calculate the longitude and latitude and put a circle on the map in the clicked position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="marginTop"></param>
        /// <param name="marginLeft"></param>
        public void onChooseSpot(double X, double Y, double marginTop, double marginLeft)
        {
            this.model.CalculateAreaProps(X, Y, marginTop, marginLeft);
            double left = X - marginLeft + 2;
            double top = Y - marginTop;
            EllipseMargin = new Thickness(left, top, 0, 0);
            NotifyPropertyChanged("EllipseMargin");
            CircleFlag = true;
            NotifyPropertyChanged("CircleFlag");
            isClicked = true;
        }

        /// <summary>
        /// returns true if a area was choosen in the map,else false.
        /// </summary>
        public bool Finish()
        {
            if(!isClicked)
            {
                return false;
            }
            CircleFlag = false;
            this.model.CheckForClosestCountries();
            this.SendParameters();
            isClicked = false;
            return true;
        }

        override
        public void SendParameters()
        {
            BaseVM.GetInstance.SendParam(BaseVM.ViewModels.LocationMap, BaseVM.ViewModels.AreaChooser);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MusicPlayList.View
{
    /// <summary>
    /// Interaction logic for LocationMapChooser.xaml
    /// </summary>
    public partial class LocationMapChooser : Window
    {
        private ViewModel.IVM locationChooserVM = ViewModel.BaseVM.GetInstance._LocationMapChoosenVM;

        /// <summary>
        /// Location map chooser constructor.
        /// </summary>
        public LocationMapChooser()
        {
            InitializeComponent();
            WindowLocationSeter.CenterWindowOnScreen(this);
            this.DataContext = locationChooserVM;
        }

        /// <summary>
        /// click event for next button after choosing area in the map. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (((ViewModel.LocationMapChooserVM)locationChooserVM).Finish())
            {
                Window chooser = new CountryChooser();
                //App.Current.MainWindow = chooser;
                WindowLocationSeter.changeWindow(chooser);
                this.Close();
                //chooser.Show();
            }
        }

        /// <summary>
        /// click event for clicking on the map,to save the location and draw circle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapClick(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(this);
            double top = border.Margin.Top + panel.Margin.Top + label.Height + separtor.Height + gridMap.Margin.Top + worldMap.Margin.Top;
            double left = border.Margin.Left + panel.Margin.Left + gridMap.Margin.Left + worldMap.Margin.Left;
            ((ViewModel.LocationMapChooserVM)locationChooserVM).onChooseSpot(p.X, p.Y, top, left);
        }
       
    }
}
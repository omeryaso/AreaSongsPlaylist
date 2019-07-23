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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private ViewModel.IVM registrationVM = ViewModel.BaseVM.GetInstance._RegistrationVM;

        /// <summary>
        /// registration constructor.
        /// </summary>
        public Registration()
        {
            InitializeComponent();
            WindowLocationSeter.CenterWindowOnScreen(this);
            this.DataContext = registrationVM;
        }

        /// <summary>
        /// cancel and move to the login window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel.RegistrationVM)registrationVM).resetinput();
            Login win = new Login();
            WindowLocationSeter.changeWindow(win);
            //App.Current.MainWindow = win;
            this.Close();
            //win.Show();
        }

        /// <summary>
        /// click event for submiting the user name and password.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            Boolean result = ((ViewModel.RegistrationVM)registrationVM).CheckRegistraion();
            if (result)
            {
                Window chooser = new LocationMapChooser();
                //App.Current.MainWindow = chooser;
                WindowLocationSeter.changeWindow(chooser);
                System.Threading.Thread.Sleep(300);
                this.Close();
                //chooser.Show();
            }
        }

        /// <summary>
        /// click event for reseting the text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel.RegistrationVM)registrationVM).resetinput();
        }
    }
}

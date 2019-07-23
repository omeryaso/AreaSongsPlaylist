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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        private ViewModel.IVM loginVM;

        /// <summary>
        /// Login constructor.
        /// </summary>>
        public Login()
        {
            InitializeComponent();

            ViewModel.BaseVM baseVM = ViewModel.BaseVM.GetInstance;
            loginVM = baseVM._LoginVM;
            WindowLocationSeter.CenterWindowOnScreen(this);
            this.DataContext = loginVM;
        }

        /// <summary>
        /// click event for submit buttn after filling the username and password. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Boolean result = ((ViewModel.LoginVM)loginVM).Confirm();
            if (result)
            {
                Window playList = new PlayList();
                //App.Current.MainWindow = playList;
                WindowLocationSeter.changeWindow(playList);
                this.Close();
                //playList.Show();
            }
        }

        /// <summary>
        /// click event for moving to registration. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel.LoginVM)loginVM).clear();
            Window reg = new Registration();
            //App.Current.MainWindow = reg;
            WindowLocationSeter.changeWindow(reg);
            System.Threading.Thread.Sleep(300);
            this.Close();
            //reg.ShowDialog();

        }



    }
}

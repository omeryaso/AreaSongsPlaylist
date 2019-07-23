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
    /// Interaction logic for PlayList.xaml
    /// </summary>
    public partial class PlayList : Window
    {

        private ViewModel.IVM playListVM = ViewModel.BaseVM.GetInstance._PlayListVM;

        /// <summary>
        ///play list constructor. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public PlayList()
        {
            InitializeComponent();
            WindowLocationSeter.CenterWindowOnScreen(this);
            this.DataContext = playListVM;
        }

        /// <summary>
        /// click event for editing the playlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtmEdit_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel.PlayListVM)playListVM).SendParameters();
            Window editor = new PlayListEditor();
            WindowLocationSeter.changeWindow(editor);

            //App.Current.MainWindow = editor;
            this.Close();
            //editor.Show();
        }


        /// <summary>
        /// click event for save the database and going back to the welcome window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtmExit_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel.PlayListVM)playListVM).SaveAndExit();
            Window mainWin = new MainWindow();
            WindowLocationSeter.changeWindow(mainWin);
            //App.Current.MainWindow = mainWin;
            this.Close();
            //mainWin.Show();
        }
    }
}

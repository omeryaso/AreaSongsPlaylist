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
    /// Interaction logic for PlayListEditor.xaml
    /// </summary>
    public partial class PlayListEditor : Window
    {
        private ViewModel.IVM editorVM = ViewModel.BaseVM.GetInstance._PlayListEditorVM;

        /// <summary>
        /// constructor playlist editor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public PlayListEditor()
        {
            InitializeComponent();
            WindowLocationSeter.CenterWindowOnScreen(this);
            this.DataContext = editorVM;
        }

        /// <summary>
        /// click event for submit the changes and moving to the playlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            editorVM.SendParameters();
            Window playList = new PlayList();
            WindowLocationSeter.changeWindow(playList);
            //App.Current.MainWindow = playList;
            this.Close();
            //playList.Show();
        }

        /// <summary>
        /// click event for filtering the playlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterBtn_click(object sender, RoutedEventArgs e)
        {
            ((ViewModel.PlayListEditorVM)editorVM).Filter();
        }

        /// <summary>
        /// click event for reset the playlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel.PlayListEditorVM)editorVM).reset();
        }
    }
}

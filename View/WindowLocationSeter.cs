using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MusicPlayList.View
{
    class WindowLocationSeter
    {
        /// <summary>
        /// put the given window in the middle of the screen.
        /// </summary>
        /// <param name="win"></param>
        public static void CenterWindowOnScreen(Window win)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = win.Width;
            double windowHeight = win.Height;
            win.Left = (screenWidth / 2) - (windowWidth / 2);
            win.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        public static void changeWindow(Window win)
        {
            if (!win.IsVisible)
            {
                win.Show();
            }

            if (win.WindowState == WindowState.Minimized)
            {
                win.WindowState = WindowState.Normal;
            }

            win.Activate();
            win.Topmost = true;
            win.Topmost = false; 
            win.Focus();
        }
    }
}

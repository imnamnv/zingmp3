using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZingMp3
{
    /// <summary>
    /// Interaction logic for Playlist.xaml
    /// </summary>
    public partial class Playlist : UserControl
    {
        private List<Song> list;
        public List<Song> List
        {
            get
            {
                return list;
            }

            set
            {

                list = new List<Song>();
                listSong.ItemsSource = null;
                list = value;
                listSong.ItemsSource = list;

            }
        }

        private event EventHandler backToMain;
        public event EventHandler BackToMain
        {
            add { backToMain += value; }
            remove { backToMain -= value; }
        }
        public Playlist()
        {
            InitializeComponent();
            ucSongInfor.BackToMain += ucSongInfor_BackToMain;
        }

        private void ucSongInfor_BackToMain(object sender, EventArgs e)
        {
            gridTop10.Visibility = Visibility.Visible;
            ucSongInfor.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (backToMain != null)
            {
                backToMain(this, new EventArgs());

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Song song = (sender as Button).DataContext as Song;

            gridTop10.Visibility = Visibility.Hidden;
            ucSongInfor.Visibility = Visibility.Visible;
            ucSongInfor.Song = song;
        }
    }
}

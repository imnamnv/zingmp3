using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaction logic for SongInfo.xaml
    /// </summary>
    public partial class SongInfo : UserControl
    {
        private Song song;


        public SongInfo()
        {
            InitializeComponent();

        }

        public Song Song
        {
            get
            {
                return song;
            }

            set
            {

                song = value;
                lyric.Text = song.Lyric;
                //playSong(song);
            }
        }

        private event EventHandler backToMain;
        public event EventHandler BackToMain
        {
            add { backToMain += value; }
            remove { backToMain -= value; }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (backToMain != null)
            {
                backToMain(this, new EventArgs());

            }


        }

        public void playSong(Song song)
        {
            var wplayer = new WMPLib.WindowsMediaPlayer();
            wplayer.URL = @"http:" + song.DownloadUrl;
            wplayer.controls.play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var wplayer = new WMPLib.WindowsMediaPlayer();
            wplayer.URL = @"http:" + song.DownloadUrl;
            wplayer.controls.play();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DAO dao = new DAO();
            bool check = dao.checkSong(song.DownloadUrl);
            if (check == true)
            {
                dao.addtoPlaylist(Song);

            }
            else
            {
                MessageBox.Show("Bài hát đã tồn tại");
            }
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = song.SongName;
            dialog.ShowDialog();
            WebClient web = new WebClient();
            //web.DownloadFile("http:" + song.DownloadUrl, @"C:\Users\NamNV\Desktop\ZingMp3\Song\" + song.SongName + ".mp3");
            web.DownloadFile("http:" + song.DownloadUrl, dialog.FileName+ song.SongName + ".mp3");

        }
    }
}

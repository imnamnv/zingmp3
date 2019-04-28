using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using xNet;

namespace ZingMp3
{
    /// <summary>
    /// Interaction logic for ReaultSearch.xaml
    /// </summary>
    public partial class ReaultSearch : UserControl
    {
        private event EventHandler backToMain;
        public event EventHandler BackToMain
        {
            add { backToMain += value; }
            remove { backToMain -= value; }
        }
        List<Song> list;


        private string nameSong;
        public string NameSong
        {
            get
            {
                return nameSong;
            }

            set
            {
                listSong.ItemsSource = null;
                list = new List<Song>();
                nameSong = value;
                crawlBxh();
            }
        }

        public ReaultSearch()
        {
            InitializeComponent();
            ucSongInfor.BackToMain += ucSongInfor_BackToMain;
            list = new List<Song>();
            
        }

        private void ucSongInfor_BackToMain(object sender, EventArgs e)
        {
            gridTop10.Visibility = Visibility.Visible;
            ucSongInfor.Visibility = Visibility.Hidden;
        }

        void crawlBxh()
        {
            HttpRequest http = new HttpRequest();
            string htmlBXHVN = http.Get(@"https://mp3.zing.vn/tim-kiem/bai-hat.html?q="+nameSong).ToString();
            getListBXH(list, htmlBXHVN);
        }

        private void getListBXH(List<Song> list, string htmlBXH)
        {
            string bxhPattern = @"<h3 class=""title-item ellipsis"">(.*?)</h3>";
            var listBXH = Regex.Matches(htmlBXH, bxhPattern, RegexOptions.Singleline);
            
            int stt = 1;
            foreach (var item in listBXH)
            {


                //get url
                var urlVar = Regex.Matches(item.ToString(), @"<a href=""(.*?)""", RegexOptions.Singleline);
                //get song name
                string urlString = urlVar[0].ToString();

                
                string url = urlString.Replace("href=\"", "").Replace("\"","").Replace("<a ","");


                //get lyric
                HttpRequest http = new HttpRequest();
                string htmlSong = http.Get(@"https://mp3.zing.vn" + url).ToString();
                var lyrics = Regex.Matches(htmlSong, @"<p class=""fn-wlyrics fn-content""(.*?)</p>", RegexOptions.Singleline);

                string lyric;
                if (lyrics.Count > 0)
                {
                    lyric = lyrics[0].ToString().Replace("<p class=\"fn-wlyrics fn-content\" data-min=\"300px\" data-max=\"auto\" style=\"overflow: hidden; height: 300px;\">", "");
                    lyric = lyric.Replace("<br>", "");
                    lyric = lyric.Replace("</p>", "");
                }
                else
                {
                    lyric = "Lời bài hát chưa được cập nhật";
                }


                var getJson = Regex.Matches(htmlSong, @"data-xml=""(.*?)""", RegexOptions.Singleline);
                string getJsonString = getJson[0].ToString().Replace(@"data-xml=""", "").Replace("\"", "");
                string jfonInfor = http.Get(@"https://mp3.zing.vn/xhr" + getJsonString).ToString();
                JObject jObject = JObject.Parse(jfonInfor);
                string songName = jObject["data"]["name"].ToString();
                string singerName = jObject["data"]["artists"][0]["name"].ToString();
                string urlDownLoad = jObject["data"]["source"]["128"].ToString();
                list.Add(new Song() { SongName = songName, SingerName = singerName, SongUrl = url, Stt = stt, Lyric = lyric, DownloadUrl = urlDownLoad });
                if (stt == 10)
                {
                    break;
                }
                stt++;
            }
            listSong.ItemsSource = list;
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

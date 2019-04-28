using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool isCheckVN;
        private bool isCheckEU;


        List<Song> listVN;
        List<Song> listEU;


        public bool IsCheckVN
        {
            get
            {
                return isCheckVN;
            }

            set
            {
                listSong.ItemsSource = null;
                listVN = new List<Song>();
                HttpRequest http = new HttpRequest();
                string htmlBXHVN = http.Get(@"https://mp3.zing.vn/zing-chart-tuan/Bai-hat-Viet-Nam/IWZ9Z08I.html").ToString();
                getListBXH(listVN, htmlBXHVN);
                isCheckVN = value;
                listSong.ItemsSource = listVN;
                isCheckEU = false;
             
                OnPropertyChanged("IsCheckVN");
                OnPropertyChanged("IsCheckEU");
            }
        }

        public bool IsCheckEU
        {
            get
            {
                return isCheckEU;
            }

            set
            {
                listSong.ItemsSource = null;
                listEU = new List<Song>();
                HttpRequest http = new HttpRequest();
                string htmlBXHEU = http.Get(@"https://mp3.zing.vn/zing-chart-tuan/Bai-hat-US-UK/IWZ9Z0BW.html").ToString();
                getListBXH(listEU, htmlBXHEU);
                isCheckEU = value;
                listSong.ItemsSource = listEU;
                isCheckVN = false;
             
                OnPropertyChanged("IsCheckVN");
                OnPropertyChanged("IsCheckEU");
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            ucSongInfor.BackToMain += ucSongInfor_BackToMain;
            resultSearch.BackToMain += ResultSearch_BackToMain;
            playList.BackToMain += PlayList_BackToMain;
            this.DataContext = this;
            listVN = new List<Song>();
            listEU = new List<Song>();
         
           // crawlBxh();
            IsCheckVN = true;
        }



        void crawlBxh()
        {
            HttpRequest http = new HttpRequest();
            string htmlBXHVN = http.Get(@"https://mp3.zing.vn/zing-chart-tuan/Bai-hat-Viet-Nam/IWZ9Z08I.html").ToString();
            getListBXH(listVN,htmlBXHVN);
            string htmlBXHEU = http.Get(@"https://mp3.zing.vn/zing-chart-tuan/Bai-hat-US-UK/IWZ9Z0BW.html").ToString();
            getListBXH(listEU, htmlBXHEU);


        }

        void getListBXH(List<Song> list,string htmlBXH)
        {

            string bxhPattern = @"<div class=""table-body"">(.*?)</ul>";
            var listBXH = Regex.Matches(htmlBXH, bxhPattern, RegexOptions.Singleline);
            string bxh = listBXH[0].ToString();
            var listSonghtml = Regex.Matches(bxh, @"<li(.*?)</li>", RegexOptions.Singleline);//return list song
            int stt = 1;
            foreach (var item in listSonghtml)
            {
                
                var songAndUrl = Regex.Matches(item.ToString(), @"<a\s\S*\stitle=""(.*?)""", RegexOptions.Singleline);
                //get song name
                string songString = songAndUrl[1].ToString();
                int start = songString.IndexOf("title=\"");
                int end = songString.Length - start - 1;
                string songName = songString.Substring(start, end).Replace("title=\"", "");

                //get singer name
                var singerList = Regex.Matches(item.ToString(), @"<h4(.*?)"">(.*?)"">", RegexOptions.Singleline);
                string singerName = "";
                foreach (var singer in singerList)
                {
                    string singerSave = singer.ToString();
                    int startSingerName = singerSave.IndexOf("title=\"");
                    int endSingerName = singerSave.Length - startSingerName - 2;
                    singerName += singerSave.Substring(startSingerName, endSingerName).Replace("title=\"", "") + "-";
                }
                singerName = singerName.Replace("Nghệ sĩ ", "");
                singerName = singerName.Substring(0, singerName.Length - 1);
                //get url
                int startUrl = songString.IndexOf("href=\"");
                string url = songString.Substring(startUrl, start - startUrl - 2).Replace("href=\"", "");


                //get lyric
                HttpRequest http = new HttpRequest();
                string htmlSong = http.Get(@"https://mp3.zing.vn"+url).ToString();
                var lyrics = Regex.Matches(htmlSong, @"<p class=""fn-wlyrics fn-content""(.*?)</p>",RegexOptions.Singleline);

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


                var getJson = Regex.Matches(htmlSong, @"data-xml=""(.*?)""",RegexOptions.Singleline);
                string getJsonString = getJson[0].ToString().Replace(@"data-xml=""","").Replace("\"","");
                string jfonInfor = http.Get(@"https://mp3.zing.vn/xhr" + getJsonString).ToString();
                JObject jObject = JObject.Parse(jfonInfor);


                string urlDownLoad = jObject["data"]["source"]["128"].ToString();
                list.Add(new Song() { SongName = songName, SingerName = singerName, SongUrl = url, Stt = stt,Lyric = lyric,DownloadUrl = urlDownLoad});
                if (stt ==10)
                {
                    break;
                }
                stt++;
            }

        }



        private void ucSongInfor_BackToMain(object sender, EventArgs e)
        {
            gridTop10.Visibility = Visibility.Visible;
            ucSongInfor.Visibility = Visibility.Hidden;
        }
        private void ResultSearch_BackToMain(object sender, EventArgs e)
        {
            gridTop10.Visibility = Visibility.Visible;
            resultSearch.Visibility = Visibility.Hidden;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Song song = (sender as Button).DataContext as Song;
            
            gridTop10.Visibility = Visibility.Hidden;
            ucSongInfor.Visibility = Visibility.Visible;
            ucSongInfor.Song = song;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string name = nameSong.Text.Trim().Replace(" ","+");
            

            gridTop10.Visibility = Visibility.Hidden;
            resultSearch.Visibility = Visibility.Visible;
            resultSearch.NameSong = name;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DAO dao = new DAO();
            
            gridTop10.Visibility = Visibility.Hidden;
            playList.Visibility = Visibility.Visible;
            List<Song> list2 = dao.getAll();
            playList.List = list2;
        }
        private void PlayList_BackToMain(object sender, EventArgs e)
        {
            gridTop10.Visibility = Visibility.Visible;
            playList.Visibility = Visibility.Hidden;
        }
    }
}

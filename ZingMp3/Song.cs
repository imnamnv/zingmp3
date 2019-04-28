using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZingMp3
{
    public class Song
    {
        private string songName;
        private string singerName;
        private string songUrl;
        private int stt;
        private string lyric;
        private string downloadUrl;
        private string imageUrl;
        private string playlist;
        public string SongName
        {
            get
            {
                return songName;
            }

            set
            {
                songName = value;
            }
        }

       
        public string SongUrl
        {
            get
            {
                return songUrl;
            }

            set
            {
                songUrl = value;
            }
        }

        public int Stt
        {
            get
            {
                return stt;
            }

            set
            {
                stt = value;
            }
        }

        public string SingerName
        {
            get
            {
                return singerName;
            }

            set
            {
                singerName = value;
            }
        }

        public string Lyric
        {
            get
            {
                return lyric;
            }

            set
            {
                lyric = value;
            }
        }

        public string DownloadUrl
        {
            get
            {
                return downloadUrl;
            }

            set
            {
                downloadUrl = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                return imageUrl;
            }

            set
            {
                imageUrl = value;
            }
        }

        public string Playlist
        {
            get
            {
                return playlist;
            }

            set
            {
                playlist = value;
            }
        }
    }
}

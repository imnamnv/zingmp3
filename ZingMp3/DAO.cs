using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ZingMp3
{
    public class DAO
    {

        public List<Song> getAll()
        {
            List<Song> list = new List<Song>();
            SqlConnection connection;
            string connectionString = ConfigurationManager.ConnectionStrings["ZingMp3"].ConnectionString;
            connection = new SqlConnection(connectionString);
            try
            {
                string query = "SELECT [songName],[songUrl],[lyric],[downloadUrl],[imageUrl],[singerName] FROM [dbo].[Song]";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader read = command.ExecuteReader();
                int stt = 1;
                while (read.Read())
                {
                    Song s = new Song()
                    {
                        Stt = stt,
                        SongName = read["songName"].ToString(),
                        SongUrl = read["songUrl"].ToString(),
                        Lyric = read["lyric"].ToString(),
                        DownloadUrl = read["downloadUrl"].ToString(),
                        SingerName = read["singerName"].ToString(),
                    };
                    stt++;
                    list.Add(s);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return list;

        }
        public List<string> getPlaylistName()
        {
            List<string> list = null;
            SqlConnection connection;
            string connectionString = ConfigurationManager.ConnectionStrings["ZingMp3"].ConnectionString;
            connection = new SqlConnection(connectionString);
            try
            {
                string query = "SELECT [playlist] FROM [dbo].[Song] Group by [playlist] having count(*) >=1 ";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    list.Add(read["playlist"].ToString());
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return list;
        }

        public void addtoPlaylist(Song s)
        {
            SqlConnection connection;
            string connectionString = ConfigurationManager.ConnectionStrings["ZingMp3"].ConnectionString;
            connection = new SqlConnection(connectionString);
            try
            {
                string query = "INSERT INTO [dbo].[Song] ([songName],[songUrl],[lyric],[downloadUrl],[singerName]) VALUES(@songName,@songUrl,@lyric,@downloadUrl,@singerName)";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.Parameters.Add(new SqlParameter("@songName", s.SongName));
                command.Parameters.Add(new SqlParameter("@songUrl", s.SongUrl));
                command.Parameters.Add(new SqlParameter("@lyric", s.Lyric));
                command.Parameters.Add(new SqlParameter("@downloadUrl", s.DownloadUrl));
                command.Parameters.Add(new SqlParameter("@singerName", s.SingerName));
                command.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

        }

        public bool checkSong(string name)
        {
            SqlConnection connection;
            string connectionString = ConfigurationManager.ConnectionStrings["ZingMp3"].ConnectionString;
            connection = new SqlConnection(connectionString);
            try
            {
                string query = "SELECT [songName],[songUrl],[lyric],[downloadUrl],[imageUrl],[singerName] FROM [dbo].[Song] where downloadUrl = @url";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.Parameters.Add(new SqlParameter("@url", name));
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return true;

        }

    }
}

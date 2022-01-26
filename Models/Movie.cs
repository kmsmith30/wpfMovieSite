using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Media.Imaging;

namespace Kvfx.Models
{
    public class Movie
    {
        private const int PosterImageWidth = 100;

        public enum MaturityRating 
        { 
            G = 4, PG = 8, PG13 = 12, R = 16, NC17 = 20, Null = -404
        }

        public const string DatabaseName = App.DatabaseName + "Movies";

        public const string ImagePath = App.ImagePath + @"Movies\";
        public const string VideoPath = App.VideoPath + @"Movies\";

        public const string PosterPath = ImagePath + @"Posters\";

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int RunTime { get; set; }

        public string RunTimeString
        {
            get
            {
                if (RunTime >= 60 )
                {
                    if (RunTime % 60 == 0)
                    {
                        return $"{ RunTime / 60 }h";
                    }

                    return $"{ RunTime / 60 }h { RunTime % 60 }m";
                }

                return $"{ RunTime }m";
            }
        }

        public MaturityRating Rating { get; set; }

        public string RatingString
        {
            get
            {
                if (Rating == MaturityRating.PG13)
                {
                    return "PG-13";
                }
                else if (Rating == MaturityRating.NC17)
                {
                    return "NC-17";
                }
                else if (Rating != MaturityRating.Null)
                {
                    return Rating.ToString();
                }

                return string.Empty;
            }
        }

        public string Description { get; set; }

        public string FileTag { get; private set; }

        public string ImageFile { get => $"{ ImagePath }{ FileTag }.jpg"; }

        public string VideoFile { get => $"{ VideoPath }{ FileTag }.mp4"; }

        public string PosterFile { get => $"{ PosterPath }{ FileTag }.jpg"; }

        public BitmapImage PosterImage
        {
            get
            {
                BitmapImage img = new BitmapImage();

                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.UriSource = new Uri(PosterFile);

                img.DecodePixelWidth = PosterImageWidth;
                img.EndInit();
                img.Freeze();

                return img;
            }
        }

        public static bool Insert(Movie mov)
        {
            bool result = true;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                using (SqlCommand cmd = new SqlCommand(App.DatabaseName + "uspNewMovie", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 500));
                    cmd.Parameters["@Title"].Value = mov.Title;

                    cmd.Parameters.Add(new SqlParameter("@ReleaseDate", SqlDbType.Date));
                    cmd.Parameters["@ReleaseDate"].Value = mov.ReleaseDate;

                    cmd.Parameters.Add(new SqlParameter("@RunTime", SqlDbType.Int));
                    cmd.Parameters["@RunTime"].Value = mov.RunTime;

                    cmd.Parameters.Add(new SqlParameter("@Rating", SqlDbType.Int));
                    cmd.Parameters["@Rating"].Value = (int)mov.Rating;

                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.Text));
                    cmd.Parameters["@Description"].Value = mov.Description;

                    try
                    {
                        conn.Open();

                        int check = cmd.ExecuteNonQuery();

                        if (check == 0)
                        {
                            result = false;
                        }
                    }
                    catch
                    {
                        result = false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return result;
        }

        public static Movie Load(int id)
        {
            Movie movie = null;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                string query = $"SELECT * FROM { DatabaseName } WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
                    cmd.Parameters["@Id"].Value = id;

                    try
                    {
                        conn.Open();

                        using (SqlDataReader data = cmd.ExecuteReader())
                        {
                            if (data.HasRows)
                            {
                                while (data.Read())
                                {
                                    movie = Create(data);
                                }
                            }
                        }
                    }
                    catch
                    {
                        movie = null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return movie;
        }

        public static List<Movie> LoadAll()
        {
            List<Movie> movies = null;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                string query = $"SELECT Id FROM { DatabaseName }";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();

                        using (SqlDataReader data = cmd.ExecuteReader())
                        {
                            if (data.HasRows)
                            {
                                movies = new List<Movie>();

                                while (data.Read())
                                {
                                    int id = data.GetInt32(data.GetOrdinal("Id"));

                                    movies.Add(Load(id));
                                }
                            }
                        }
                    }
                    catch
                    {
                        movies = null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return movies;
        }

        public static bool ResetId()
        {
            bool result = true;

            using (SqlConnection sqlConn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                string sqlQuery = $"DBCC CHECKIDENT ('Movies', RESEED, 309)";

                using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    try
                    {
                        sqlConn.Open();

                        int check = sqlCmd.ExecuteNonQuery();

                        if (check == 0)
                        {
                            result = false;
                        }
                    }
                    catch
                    {
                        result = false;
                    }
                    finally
                    {
                        sqlConn.Close();
                    }
                }
            }

            return result;
        }

        private static Movie Create(SqlDataReader data)
        {
            Movie movie =  new Movie
            {
                Id = data.GetInt32(data.GetOrdinal("Id")),
                Title = data.GetString(data.GetOrdinal("Title")),
                ReleaseDate = data.GetDateTime(data.GetOrdinal("ReleaseDate")),
                RunTime = data.GetInt32(data.GetOrdinal("RunTime"))
            };

            int r_i = data.GetOrdinal("Rating");
            int d_i = data.GetOrdinal("Description");
            int ft_i = data.GetOrdinal("FileTag");

            if (!data.IsDBNull(r_i))
            {
                movie.Rating = (MaturityRating)data.GetInt32(r_i);
            }

            if (!data.IsDBNull(d_i))
            {
                movie.Description = data.GetString(d_i);
            }

            if (!data.IsDBNull(ft_i))
            {
                movie.FileTag = data.GetString(ft_i);
            }

            return movie;
        }
    }
}

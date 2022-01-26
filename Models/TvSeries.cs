using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Media.Imaging;

namespace Kvfx.Models
{
    public class TvSeries
    {
        public enum MaturityRating
        {
            TVY = 3, TVY7 = 7, TVG = 11, TV14 = 15, TVMA = 19, Null = -404 
        }

        private const int PosterImageWidth = 200;

        public const string DatabaseName = App.DatabaseName + "TvSeries";

        public const string ImagePath = App.ImagePath + @"Tv\";

        public const string PosterPath = ImagePath + @"Posters\";

        public int Id { get; private set; }

        public string Title { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime FinalDate { get; private set; }

        public MaturityRating Rating { get; private set; }

        public string RatingString
        {
            get
            {
                if (Rating == MaturityRating.TVG)
                {
                    return "TV-G";
                }
                else if (Rating == MaturityRating.TVY)
                {
                    return "TV-Y";
                }
                else if (Rating == MaturityRating.TVY7)
                {
                    return "TV-Y7";
                }
                else if (Rating == MaturityRating.TV14)
                {
                    return "TV-14";
                }
                else if (Rating == MaturityRating.TVMA)
                {
                    return "TV-MA";
                }

                return "";
            }
        }

        public string DateString
        {
            get
            {
                if (FinalDate.Year != 1)
                {
                    return $"{ StartDate.Year } - { FinalDate.Year }";
                }
                else
                {
                    return $"{ StartDate.Year } -";
                }
            }
        }

        public string Description { get; private set; }

        public List<List<TvEpisode>> Seasons { get; private set; }

        public int NumSeasons { get => Seasons.Count; }

        public List<string> SeasonStrings
        {
            get
            {
                List<string> res = new List<string>();

                foreach (var s in Seasons)
                {
                    res.Add($"Season { s[0].Season }");
                }

                return res;
            }
        }

        public string FileTag { get; private set; }

        public string ImageFile { get => $"{ ImagePath }{ FileTag }.jpg"; }

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

        public static TvSeries Load(int id)
        {
            TvSeries series = null;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                string query = $"SELECT * FROM { DatabaseName } WHERE @Id = Id";

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
                                    series = Create(data);
                                }
                            }
                        }
                    }
                    catch
                    {
                        series = null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return series;
        }


        public static List<TvSeries> LoadAll()
        {
            List<TvSeries> series = null;

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
                                series = new List<TvSeries>();

                                while (data.Read())
                                {
                                    int id = data.GetInt32(data.GetOrdinal("Id"));

                                    series.Add(Load(id));
                                }
                            }
                        }
                    }
                    catch
                    {
                        series = null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return series;
        }

        public static bool ResetId()
        {
            bool result = true;

            using (SqlConnection sqlConn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                string sqlQuery = $"DBCC CHECKIDENT ('TvSeries', RESEED, 0)";

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

        private static TvSeries Create(SqlDataReader data)
        {
            TvSeries series = new TvSeries 
            { 
                Id = data.GetInt32(data.GetOrdinal("Id")),
                Title = data.GetString(data.GetOrdinal("Title")),
                StartDate = data.GetDateTime(data.GetOrdinal("StartDate")),
                Description = data.GetString(data.GetOrdinal("Description"))
            };

            int fd_i = data.GetOrdinal("FinalDate");
            int r_i = data.GetOrdinal("Rating");
            int ft_i = data.GetOrdinal("FileTag");

            if (!data.IsDBNull(fd_i))
            {
                series.FinalDate = data.GetDateTime(data.GetOrdinal("FinalDate"));
            }

            if (!data.IsDBNull(r_i))
            {
                series.Rating = (MaturityRating)data.GetInt32(data.GetOrdinal("Rating"));
            }

            if (!data.IsDBNull(ft_i))
            {
                series.FileTag = data.GetString(ft_i);
            }

            series.Seasons = TvEpisode.LoadAllSeasons(series.Id);

            return series;
        }
    }
}

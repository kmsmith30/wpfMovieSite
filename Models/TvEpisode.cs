using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Media.Imaging;

namespace Kvfx.Models
{
    public class TvEpisode
    {
        private const int IndexImageWidth = 200;

        public const string DatabaseName = App.DatabaseName + "TvEpisodes";

        public const string ImagePath = App.ImagePath + @"Tv\Episodes\";

        public const string VideoPath = App.VideoPath + @"Tv\";

        public int Id { get; set; }

        public int SeriesId { get; set; }

        public string Title { get; set; }

        public int Season { get; set; }

        public int Number { get; set; }

        public string InfoString { get => $"{ Title } (S{ Season } E{ Number })"; }

        public DateTime ReleaseDate { get; set; }

        public string ReleaseDateString 
        { 
            get => $"{ ReleaseDate.ToString("MMMM") } { ReleaseDate.Day }, " +
                $"{ ReleaseDate.Year }"; 
        }

        public int RunTime { get; set; }

        public string Description { get; set; }

        public string SeriesTag { get => $"{ TvSeries.Load(SeriesId).FileTag }"; }

        public string FileTag { get => $"{ SeriesTag }_S{ Season }E{ Number }"; }

        public string ImageFile { get => $@"{ ImagePath }{ SeriesTag }\{ FileTag }.jpg"; }

        public string VideoFile { get => $@"{ VideoPath }{ SeriesTag }\S{ Season }\{ FileTag }.mp4"; }

        public BitmapImage IndexImage
        {
            get
            {
                BitmapImage img = new BitmapImage();

                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.UriSource = new Uri(ImageFile);

                img.DecodePixelWidth = IndexImageWidth;
                img.EndInit();
                img.Freeze();

                return img;
            }
        }

        public static bool Insert(TvEpisode ep)
        {
            bool result = true;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                using (SqlCommand cmd = new SqlCommand(App.DatabaseName + "uspNewTvEpisode", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@SeriesId", SqlDbType.Int));
                    cmd.Parameters["@SeriesId"].Value = ep.SeriesId;

                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 500));
                    cmd.Parameters["@Title"].Value = ep.Title;

                    cmd.Parameters.Add(new SqlParameter("@Season", SqlDbType.Int));
                    cmd.Parameters["@Season"].Value = ep.Season;

                    cmd.Parameters.Add(new SqlParameter("@Number", SqlDbType.Int));
                    cmd.Parameters["@Number"].Value = ep.Number;

                    cmd.Parameters.Add(new SqlParameter("@ReleaseDate", SqlDbType.Date));
                    cmd.Parameters["@ReleaseDate"].Value = ep.ReleaseDate.Date;

                    cmd.Parameters.Add(new SqlParameter("@RunTime", SqlDbType.Int));
                    cmd.Parameters["@RunTime"].Value = ep.RunTime;

                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.Text));
                    cmd.Parameters["@Description"].Value = ep.Description;

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

        public static TvEpisode Load(int id)
        {
            TvEpisode ep = null;

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
                                    ep = Create(data);
                                }
                            }
                        }
                    }
                    catch
                    {
                        ep = null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return ep;
        }

        public static List<TvEpisode> LoadBySeries(int sid)
        {
            List<TvEpisode> eps = null;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                string query = $"SELECT Id FROM { DatabaseName } WHERE @SeriesId = SeriesId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@SeriesId", SqlDbType.Int));
                    cmd.Parameters["@SeriesId"].Value = sid;

                    try
                    {
                        conn.Open();

                        using (SqlDataReader data = cmd.ExecuteReader())
                        {
                            if (data.HasRows)
                            {
                                eps = new List<TvEpisode>();

                                while (data.Read())
                                {
                                    int id = data.GetInt32(data.GetOrdinal("Id"));

                                    eps.Add(Load(id));
                                }
                            }
                        }
                    }
                    catch
                    {
                        eps = null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return eps;
        }

        public static List<TvEpisode> LoadBySeason(int sid, int seas)
        {
            List<TvEpisode> eps = null;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                string query = $"SELECT Id FROM { DatabaseName } WHERE @SeriesId = SeriesId " +
                    $"AND @Season = Season";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@SeriesId", SqlDbType.Int));
                    cmd.Parameters["@SeriesId"].Value = sid;

                    cmd.Parameters.Add(new SqlParameter("@Season", SqlDbType.Int));
                    cmd.Parameters["@Season"].Value = seas;

                    try
                    {
                        conn.Open();

                        using (SqlDataReader data = cmd.ExecuteReader())
                        {
                            if (data.HasRows)
                            {
                                eps = new List<TvEpisode>();

                                while (data.Read())
                                {
                                    int id = data.GetInt32(data.GetOrdinal("Id"));

                                    eps.Add(Load(id));
                                }
                            }
                        }
                    }
                    catch
                    {
                        eps = null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return eps;
        }

        public static List<List<TvEpisode>> LoadAllSeasons(int sid)
        {
            List<List<TvEpisode>> seas = null;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                string query = $"SELECT Season FROM { DatabaseName } WHERE @SeriesId = SeriesId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@SeriesId", SqlDbType.Int));
                    cmd.Parameters["@SeriesId"].Value = sid;

                    try
                    {
                        conn.Open();

                        using (SqlDataReader data = cmd.ExecuteReader())
                        {
                            if (data.HasRows)
                            {
                                List<int> seasNums = new List<int>();

                                while (data.Read())
                                {
                                    int snum = data.GetInt32(data.GetOrdinal("Season"));

                                    if (!seasNums.Contains(snum))
                                    {
                                        seasNums.Add(snum);
                                    }
                                }

                                seas = new List<List<TvEpisode>>();

                                foreach (int s in seasNums)
                                {
                                    seas.Add(LoadBySeason(sid, s));
                                }
                            }
                        }
                    }
                    catch
                    {
                        seas = null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return seas;
        }

        public static bool ResetId()
        {
            bool result = true;

            using (SqlConnection sqlConn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                string sqlQuery = $"DBCC CHECKIDENT ('TvEpisodes', RESEED, 475)";

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
    
        private static TvEpisode Create(SqlDataReader data)
        {
            TvEpisode episode = new TvEpisode 
            {
                Id = data.GetInt32(data.GetOrdinal("Id")),
                SeriesId = data.GetInt32(data.GetOrdinal("SeriesId")),
                Title = data.GetString(data.GetOrdinal("Title")),
                Season = data.GetInt32(data.GetOrdinal("Season")),
                Number = data.GetInt32(data.GetOrdinal("Number")),
                ReleaseDate = data.GetDateTime(data.GetOrdinal("ReleaseDate")),
                RunTime = data.GetInt32(data.GetOrdinal("RunTime")),
                Description = data.GetString(data.GetOrdinal("Description"))
            };

            return episode;
        }
    }
}

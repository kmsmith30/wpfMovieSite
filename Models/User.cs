using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kvfx.Models
{
    public class User
    {
        public const string DatabaseName = App.DatabaseName + "Users";

        public int Id { get; private set; }

        public string Name { get; private set; }

        public static User Load(int id)
        {
            User user = null;

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
                                    user = Create(data);
                                }
                            }
                        }
                    }
                    catch
                    {
                        user = null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return user;
        }

        public static List<User> LoadAll()
        {
            List<User> users = null;

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
                                users = new List<User>();

                                while (data.Read())
                                {
                                    int id = data.GetInt32(data.GetOrdinal("Id"));

                                    users.Add(Load(id));
                                }
                            }
                        }
                    }
                    catch
                    {
                        users = null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return users;
        }

        public static List<object> LoadRecentlyWatched()
        {
            List<object> recents = null;

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
            {

            }

            return recents;
        }

        private static User Create(SqlDataReader data)
        {
            return new User
            {
                Id = data.GetInt32(data.GetOrdinal("Id")),
                Name = data.GetString(data.GetOrdinal("Name")),
            };
        }
    }
}

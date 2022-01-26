using System;
using System.IO;
using System.Collections.Generic;

using Kvfx.Models;

namespace Kvfx
{
    public class DataLoader
    {
        private const string TvDataFile = @"C:\Users\Kevin Smith\source\repos\Kvfx\Kvfx\tv.csv";
        private const string MovieDataFile = @"C:\Users\Kevin Smith\source\repos\Kvfx\Kvfx\movies.csv";

        public DataLoader()
        {
            Init();
        }

        private void Init()
        {

        }

        public void ReadTvEpisodeData()
        {
            using (var reader = new StreamReader(TvDataFile))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    string[] vals = Tokenize(line);

                    TvEpisode ep = new TvEpisode
                    {
                        SeriesId = int.Parse(vals[1]),
                        Title = vals[2],
                        Season = int.Parse(vals[3]),
                        Number = int.Parse(vals[4]),
                        ReleaseDate = DateTime.Parse(vals[5]),
                        RunTime = int.Parse(vals[6]),
                        Description = vals[7]
                    };

                    TvEpisode.Insert(ep);
                }
            }
        }

        public void ReadMovieData()
        {
            using (var reader = new StreamReader(MovieDataFile))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    string[] vals = Tokenize(line);

                    if (int.Parse(vals[0]) < 206)
                    {
                        continue;
                    }

                    string[] line2 = Tokenize(line);

                    Movie mov = new Movie 
                    {
                        Title = vals[1],
                        ReleaseDate = DateTime.Parse(vals[2]),
                        RunTime = int.Parse(vals[3]),
                        Rating = StrToRating(vals[4]),
                        Description = vals[5]
                    };

                    bool result = Movie.Insert(mov);

                    if (!result)
                    {
                        return;
                    }
                }
            }
        }

        private Movie.MaturityRating StrToRating(string rating)
        {
            switch (rating)
            {
                case "G":
                    return Movie.MaturityRating.G;
                case "PG":
                    return Movie.MaturityRating.PG;
                case "PG-13":
                    return Movie.MaturityRating.PG13;
                case "R":
                    return Movie.MaturityRating.R;
                case "NC-17":
                    return Movie.MaturityRating.NC17;
            }

            return Movie.MaturityRating.Null;
        }

        private string[] Tokenize(string str)
        {
            // Question Mark (unknown) character
            const char SpecialChar = (char)65533;

            string[] result = new string[8];

            int count = 0;

            string token = "";

            bool inQuotes = false;

            foreach (char c in str)
            {
                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',')
                {
                    if (inQuotes)
                    {
                        token += c;

                        continue;
                    }
                    else
                    {
                        result[count] = token;

                        count++;

                        if (count == 8)
                        {
                            break;
                        }

                        token = "";
                    }
                }
                else if (c == SpecialChar)
                {
                    token += '\'';
                }
                else
                {
                    token += c;
                }
            }

            return result;
        }
    }
}

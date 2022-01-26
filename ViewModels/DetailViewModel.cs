using Caliburn.Micro;

using System.Collections.Generic;
using System.Windows.Controls;

using Kvfx.Messages;
using Kvfx.Models;

namespace Kvfx.ViewModels
{
    public class DetailViewModel : Screen
    {
        private IEventAggregator eventAggregator;

        private object content;
        public object Content
        {
            get => content;
            set
            {
                if (value is Movie || value is TvSeries || value == null)
                {
                    content = value;

                    NotifyOfPropertyChange(() => Content);

                    if (content is Movie)
                    {
                        MovieVisibility = "Visible";
                        TvVisibility = "Collapsed";
                    }
                    else if (content is TvSeries)
                    {
                        CurrentSeason = (content as TvSeries).Seasons[0];

                        MovieVisibility = "Collapsed";
                        TvVisibility = "Visible";
                    }
                }
            }
        }

        private List<TvEpisode> currentSeason;
        public List<TvEpisode> CurrentSeason
        {
            get => currentSeason;
            set
            {
                currentSeason = value;

                if (currentSeason != null)
                {
                    SetSeasonString();
                }

                NotifyOfPropertyChange(() => CurrentSeason);
            }
        }

        private string currentSeasonString;
        public string CurrentSeasonString
        {
            get => currentSeasonString;
            set
            {
                currentSeasonString = value;

                NotifyOfPropertyChange(() => CurrentSeasonString);
            }
        }

        private string movieVisibility;
        private string tvVisibility;

        public string MovieVisibility
        {
            get => movieVisibility;
            set
            {
                movieVisibility = value;

                NotifyOfPropertyChange(() => MovieVisibility);
            }
        }

        public string TvVisibility
        {
            get => tvVisibility;
            set
            {
                tvVisibility = value;

                NotifyOfPropertyChange(() => TvVisibility);
            }
        }

        public DetailViewModel(IEventAggregator ea)
        {
            eventAggregator = ea;

            Content = null;

            MovieVisibility = "Hidden";
            TvVisibility = "Hidden";
        }

        public void ExitButtonClick()
        {
            eventAggregator.PublishOnUIThread(new DetailMessage
            {
                Topic = "ToBrowse",
            });

            CurrentSeason = null;
        }

        public void PlayButtonClick(int id)
        {
            eventAggregator.PublishOnUIThread(new DetailMessage
            { 
                Topic = "ToWatch",
                ContentType = "Movie",
                ContentId = id
            });
        }

        public void EpisodeClick(int id)
        {
            eventAggregator.PublishOnUIThread(new DetailMessage
            {
                Topic = "ToWatch",
                ContentType = "Tv",
                ContentId = id
            });
        }

        public void SeasonBoxChanged(ComboBox cb)
        {
            if (Content is TvSeries)
            {
                TvSeries show = Content as TvSeries;

                if (cb.SelectedIndex >= 0 && cb.SelectedIndex < show.Seasons.Count)
                {
                    CurrentSeason = show.Seasons[cb.SelectedIndex];
                }
            }
        }

        private void SetSeasonString()
        {
            TvSeries s = Content as TvSeries;

            int ord = 0;

            foreach (List<TvEpisode> seas in s.Seasons)
            {
                if (seas.Count > 0)
                {
                    if (seas[0].Season == currentSeason[0].Season)
                    {
                        currentSeasonString = (Content as TvSeries).SeasonStrings[ord];

                        break;
                    }
                }

                ord++;
            }
        }
    }
}

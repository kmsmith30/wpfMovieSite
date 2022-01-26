using Caliburn.Micro;

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

using Kvfx.Messages;
using Kvfx.Models;

namespace Kvfx.ViewModels
{
    public class WatchViewModel : Screen
    {
        /* Time Constants */

        private static TimeSpan OneMs = TimeSpan.FromMilliseconds(1);

        private static TimeSpan ZeroSec = TimeSpan.FromSeconds(0);
        private static TimeSpan OneSec = TimeSpan.FromSeconds(1);
        private static TimeSpan TenSec = TimeSpan.FromSeconds(10);

        private static TimeSpan TenMin = TimeSpan.FromMinutes(10);

        private static TimeSpan OneHrs = TimeSpan.FromHours(1);
        private static TimeSpan TenHrs = TimeSpan.FromHours(10);

        /* Fields and Properties */

        private IEventAggregator eventAggregator;

        private DispatcherTimer timer;

        private int count;

        private bool fullscreen;
        private bool playing;

        private object content;
        public object Content
        {
            get => content;
            set
            {
                if (value is Movie)
                {
                    SetMovie(value as Movie);


                    NotifyOfPropertyChange(() => Content);
                }
                else if (value is TvEpisode)
                {
                    SetEpisode(value as TvEpisode);

                    NotifyOfPropertyChange(() => Content);
                }

                Video.LoadedBehavior = MediaState.Manual;

                Video.Play();

                timer.Start();
            }
        }

        private bool contentInit;

        public MediaElement Video { get; private set; }

        private int duration;
        private int currentTime;

        public int Duration
        {
            get => duration;
            set
            {
                duration = value;

                NotifyOfPropertyChange(() => Duration);
            }
        }
        public int CurrentTime
        {
            get => currentTime;
            set
            {
                currentTime = value;

                NotifyOfPropertyChange(() => CurrentTime);
            }
        }

        private string titleText;
        public string TitleText
        {
            get => titleText;
            set
            {
                titleText = value;

                NotifyOfPropertyChange(() => TitleText);
            }
        }

        private string timeString;

        public string TimeString
        {
            get => timeString;
            set
            {
                timeString = value;

                NotifyOfPropertyChange(() => TimeString);
            }
        }

        private string controlVisibility;

        public string ControlVisibility
        {
            get => controlVisibility;
            set
            {
                controlVisibility = value;

                NotifyOfPropertyChange(() => ControlVisibility);
            }
        }

        private string pauseVisibility;
        private string playVisibility;

        public string PauseVisibility
        {
            get => pauseVisibility;
            set
            {
                pauseVisibility = value;

                NotifyOfPropertyChange(() => PauseVisibility);
            }
        }
        public string PlayVisibility
        {
            get => playVisibility;
            set
            {
                playVisibility = value;

                NotifyOfPropertyChange(() => PlayVisibility);
            }
        }

        private string exitscreenVisibility;
        private string fullscreenVisibility;

        public string ExitscreenVisibility
        {
            get => exitscreenVisibility;
            set
            {
                exitscreenVisibility = value;

                NotifyOfPropertyChange(() => ExitscreenVisibility);
            }
        }
        public string FullscreenVisibility
        {
            get => fullscreenVisibility;
            set
            {
                fullscreenVisibility = value;

                NotifyOfPropertyChange(() => FullscreenVisibility);
            }
        }

        private string timeStringVisibility;

        public string TimeStringVisibility
        {
            get => timeStringVisibility;
            set
            {
                timeStringVisibility = value;

                NotifyOfPropertyChange(() => TimeStringVisibility);
            }
        }

        private string cursorType;

        public string CursorType
        {
            get => cursorType;
            set
            {
                cursorType = value;

                NotifyOfPropertyChange(() => CursorType);
            }
        }

        public WatchViewModel(IEventAggregator ea)
        {
            eventAggregator = ea;

            eventAggregator.Subscribe(this);

            Init();

            InitTimers();
        }

        private void Init()
        {
            fullscreen = false;

            playing = true;

            Duration = 1;

            PlayVisibility = "Hidden";
            PauseVisibility = "Visible";

            ExitscreenVisibility = "Hidden";
            FullscreenVisibility = "Visible";

            ControlVisibility = "Hidden";

            CursorType = "Default";
        }

        private void InitTimers()
        {
            timer = new DispatcherTimer();

            timer.Interval = OneSec;

            timer.Tick += TimerTick;

            count = 0;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (Video.NaturalDuration.HasTimeSpan)
            {
                if (!contentInit)
                {
                    InitContent();
                }

                if (count >= 3)
                {
                    ControlVisibility = "Hidden";

                    CursorType = "None";
                }

                CurrentTime = (int)Video.Position.TotalSeconds;

                TimeString = MakeTimeString();

                count++;
            }
        }

        private void InitContent()
        {
            ControlVisibility = "Visible";

            TimeStringVisibility = "Visible";

            contentInit = true;

            CurrentTime = 0;
            Duration = (int)Video.NaturalDuration.TimeSpan.TotalSeconds;

            Video.Play();
        }

        private void SetMovie(Movie m)
        {
            content = m;

            titleText = m.Title;

            Video = new MediaElement
            {
                Source = new Uri((content as Movie).VideoFile)
            };
        }

        private void SetEpisode(TvEpisode e)
        {
            content = e;

            SetEpisodeTitleText(e);

            Video = new MediaElement
            {
                Source = new Uri((content as TvEpisode).VideoFile)
            };
        }

        private void SetEpisodeTitleText(TvEpisode e)
        {
            TvSeries show = TvSeries.Load(e.SeriesId);

            titleText = $"{ show.Title } S{ e.Season } E{ e.Number }:  { e.Title }";
        }

        private string MakeTimeString()
        {
            TimeSpan togo = Video.NaturalDuration.TimeSpan - Video.Position;

            string timeStr = togo.ToString();

            if (togo >= TenHrs)
            {
                return timeStr.Substring(0, 8);
            }
            else if (togo >= OneHrs)
            {
                return timeStr.Substring(1, 7);
            }
            else if (togo >= TenMin)
            {
                return timeStr.Substring(3, 5);
            }

            return timeStr.Substring(4, 4);
        }

        /* Event Handlers */

        public void BackClick()
        {
            TimeString = "";

            CursorType = "Default";

            ControlVisibility = "Hidden";

            count = 0;

            contentInit = false;

            CurrentTime = 0;

            if (Content is TvEpisode)
            {
                eventAggregator.PublishOnUIThread(new WatchMessage
                {
                    Topic = "ToDetail",
                    ContentId = (Content as TvEpisode).Id
                });
            }
            else if (Content is Movie)
            {
                eventAggregator.PublishOnUIThread(new WatchMessage
                {
                    Topic = "ToDetail",
                    ContentId = -1
                });
            }

            if (fullscreen)
            {
                ScreenClick();
            }

            if (!playing)
            {
                PlayPauseClick();
            }
        }

        public void BackwardClick()
        {
            if (Video.Position > TenSec)
            {
                Video.Position -= TenSec;
            }
            else
            {
                Video.Position = OneMs;
            }

            CurrentTime = (int)Video.Position.TotalSeconds;
            TimeString = MakeTimeString();
        }

        public void ForwardClick()
        {
            if (Video.Position + TenSec < Video.NaturalDuration)
            {
                Video.Position += TenSec;
            }
            else
            {
                Video.Position = OneSec;
            }

            CurrentTime = (int)Video.Position.TotalSeconds;
            TimeString = MakeTimeString();
        }

        public void PlayPauseClick()
        {
            if (playing)
            {
                Video.Pause();

                PlayVisibility = "Visible";
                PauseVisibility = "Hidden";
            }
            else
            {
                Video.Play();

                PlayVisibility = "Hidden";
                PauseVisibility = "Visible";
            }

            playing = !playing;
        }

        public void ScreenClick()
        {
            WatchMessage msg = new WatchMessage { Topic = "Command", };

            if (fullscreen)
            {
                msg.Contents = "Exit";

                ExitscreenVisibility = "Hidden";
                FullscreenVisibility = "Visible";
            }
            else
            {
                msg.Contents = "Full";

                ExitscreenVisibility = "Visible";
                FullscreenVisibility = "Hidden";
            }

            eventAggregator.PublishOnUIThread(msg);

            fullscreen = !fullscreen;
        }

        public void VolumeClick()
        {

        }

        public void MouseMove(Point p)
        {
            if (contentInit)
            {
                count = 0;

                ControlVisibility = "Visible";

                CursorType = "Default";
            }
        }

        public void TimeSliderDragStarted(Slider s)
        {
            if (contentInit)
            {
                //TimeSliderString = MakeTimeSliderString(TimeSpan.FromSeconds(s.Value));

                int dx = (int)(s.Value / (s.Maximum - s.Minimum) * s.Width);

                //TimeSliderStringVisibility = "Hidden";

                Video.Pause();
            }
        }

        public void TimeSliderDragDelta(Slider s)
        {
            TimeStringVisibility = "Hidden";

            //int dx = (int)((s.Value / (s.Maximum - s.Minimum)) * s.Width);

            //TimeSliderString = MakeTimeSliderString(TimeSpan.FromSeconds(s.Value));

            TimeStringVisibility = "Visible";
        }

        public void TimeSliderDragCompleted(Slider s)
        {
            if (contentInit)
            {
                TimeStringVisibility = "Hidden";

                Video.Position = TimeSpan.FromSeconds(s.Value);

                CurrentTime = (int)Video.Position.TotalSeconds;
                TimeString = MakeTimeString();

                if (playing)
                {
                    Video.Play();
                }
                else
                {
                    Video.Pause();
                }
            }
        }
    }
}

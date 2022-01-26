using Caliburn.Micro;

using System.IO;
using System.Windows.Input;

using Kvfx.Messages;
using Kvfx.Models;
using System;

namespace Kvfx.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<object>
    {
        private enum State {
            AccountState, BrowseState, DetailState, ErrorState, SearchState, WatchState,
        };

        /* Fields and Properties */

        private IEventAggregator eventAggregator;

        private State state;

        private AccountViewModel accountVM;
        private BrowseViewModel browseVM;
        private DetailViewModel detailVM;
        private ErrorViewModel errorVM;
        private SearchViewModel searchVM;
        private WatchViewModel watchVM;

        private string screenMode;
        private string screenState;
        private string screenStyle;

        public string ScreenMode
        {
            get => screenMode;

            set
            {
                screenMode = value;

                NotifyOfPropertyChange(() => ScreenMode);
            }
        }

        public string ScreenState
        {
            get => screenState;

            set
            {
                screenState = value;

                NotifyOfPropertyChange(() => ScreenState);
            }
        }

        public string ScreenStyle
        {
            get => screenStyle;

            set
            {
                screenStyle = value;

                NotifyOfPropertyChange(() => ScreenStyle);
            }
        }

        private string navVisibility;

        public string NavVisibility
        {
            get => navVisibility;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    navVisibility = value;
                }

                NotifyOfPropertyChange(() => NavVisibility);
            }
        }

        private bool maximized;

        private User currentUser;
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;

                NotifyOfPropertyChange(() => CurrentUser);
            }
        }

        /* Constructor */

        public ShellViewModel()
        {
            Init();
        }

        /* Helper Methods */

        private void Init()
        {
            InitEventAggregator();

            InitViewModels();

            //if (!TvEpisode.ResetId()) LoadError();

            //if (!Movie.ResetId()) LoadError();

            CurrentUser = null;

            ScreenMode = "CanResize";
            ScreenState = "Normal";
            ScreenStyle = "SingleBorderWindow";

            maximized = false;

            if (Directory.Exists(App.ResourcePath))
            {
                LoadAccount();
            }
            else
            {
                LoadError();
            }

            InitDataLoader();
        }

        private void InitDataLoader()
        {
            DataLoader dloader = new DataLoader();

            //dloader.ReadMovieData();
        }

        private void InitEventAggregator()
        {
            eventAggregator = new EventAggregator();

            eventAggregator.Subscribe(this);
        }

        private void InitViewModels()
        {
            accountVM = new AccountViewModel(eventAggregator);
            browseVM = new BrowseViewModel(eventAggregator);
            detailVM = new DetailViewModel(eventAggregator);
            errorVM = new ErrorViewModel();
            searchVM = new SearchViewModel(eventAggregator);
            watchVM = new WatchViewModel(eventAggregator);

            browseVM.FetchContent();
        }

        private void LoadAccount()
        {
            state = State.AccountState;

            HideNav();

            ActivateItem(accountVM);
        }

        private void LoadBrowse()
        {
            state = State.BrowseState;

            ShowNav();

            browseVM.SetCurrentUser(CurrentUser);

            //browseVM.FetchContent();

            ActivateItem(browseVM);
        }

        private void LoadDetail()
        {
            state = State.DetailState;

            ShowNav();

            ActivateItem(detailVM);
        }

        private void LoadError()
        {
            state = State.ErrorState;

            HideNav();

            ActivateItem(errorVM);
        }

        private void LoadSearch()
        {
            state = State.SearchState;

            HideNav();

            ActivateItem(searchVM);
        }

        private void LoadWatch()
        {
            state = State.WatchState;

            HideNav();

            ActivateItem(watchVM);
        }

        private void ShowNav()
        {
            if (NavVisibility != "Visible")
            {
                NavVisibility = "Visible";
            }
        }

        private void HideNav()
        {
            if (NavVisibility != "Collapsed")
            {
                NavVisibility = "Collapsed";
            }
        }

        /* Handle Methods */

        public void Handle(object message)
        {
            if (message is AccountMessage)
            {
                HandleAccount(message as AccountMessage);
            }
            else if (message is BrowseMessage)
            {
                HandleBrowse(message as BrowseMessage);
            }
            else if (message is DetailMessage)
            {
                HandleDetail(message as DetailMessage);
            }
            else if (message is WatchMessage)
            {
                HandleWatch(message as WatchMessage);
            }
        }

        private void HandleAccount(AccountMessage msg)
        {
            switch (msg.Topic)
            {
                case "ToBrowse":

                    CurrentUser = User.Load(msg.UserId);

                    LoadBrowse();

                    break;
                default:
                    break; // Do Nothing
            }
        }

        private void HandleBrowse(BrowseMessage msg)
        {
            switch (msg.Topic)
            {
                case "ToDetail":
                    if (msg.ContentType == "Movie")
                    {
                        detailVM.Content = Movie.Load(msg.ContentId);
                    }
                    else if (msg.ContentType == "Tv")
                    {
                        detailVM.Content = TvSeries.Load(msg.ContentId);
                    }

                    LoadDetail();
                    break;
                case "ToSearch":
                    LoadSearch();
                    break;
                default:
                    break; // Do Nothing
            }
        }

        private void HandleDetail(DetailMessage msg)
        {
            switch (msg.Topic)
            {
                case "ToBrowse":
                    LoadBrowse();
                    break;
                case "ToWatch":
                    maximized = false;

                    if (msg.ContentType == "Movie")
                    {
                        watchVM.Content = Movie.Load(msg.ContentId);
                    }
                    else if (msg.ContentType == "Tv")
                    {
                        watchVM.Content = TvEpisode.Load(msg.ContentId);
                    }

                    LoadWatch();
                    break;
                default:
                    break; // Do Nothing
            }
        }

        private void HandleWatch(WatchMessage msg)
        {
            switch (msg.Topic)
            {
                case "Command":
                    if (msg.Contents == "Exit")
                    {
                        if (maximized)
                        {
                            ScreenState = "Maximized";
                        }
                        else
                        {
                            ScreenState = "Normal";
                        }

                        ScreenMode = "CanResize";
                        ScreenStyle = "SingleBorderWindow";

                        maximized = false;
                    }
                    else if (msg.Contents == "Full")
                    {
                        if (ScreenState == "Maximized")
                        {
                            ScreenState = "Normal";
                        }

                        ScreenMode = "NoResize";
                        ScreenState = "Maximized";
                        ScreenStyle = "None";

                        maximized = true;
                    }

                    break;
                case "ToDetail":
                    if (maximized)
                    {
                        maximized = false;

                        ScreenMode = "CanResize";
                        ScreenState = "Normal";
                        ScreenStyle = "SingleBorderWindow";
                    }

                    LoadDetail();

                    break;
                default:
                    break; // Do Nothing
            }
        }

        /* Event Handlers */

        public void SearchClick()
        {
            LoadSearch();
        }

        public void KeyDown(Key k)
        {
            switch (state)
            {
                case State.WatchState:
                    if (k == Key.P || k == Key.Space || k == Key.Enter)
                    {
                        watchVM.PlayPauseClick();
                    }
                    else if ((k == Key.Escape && maximized) || (k == Key.F && !maximized))
                    {
                        watchVM.ScreenClick();
                    }
                    else if (k == Key.Left)
                    {
                        watchVM.BackwardClick();
                    }
                    else if (k == Key.Right)
                    {
                        watchVM.ForwardClick();
                    }
                    else if (k == Key.Up)
                    {
                        // Increase volume
                    }
                    else if (k == Key.Down)
                    {
                        // Decrease volume
                    }
                    break;

                default:
                    break; // Do nothing
            }
        }
    }
}

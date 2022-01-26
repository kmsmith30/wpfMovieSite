using Caliburn.Micro;

using System.Collections.Generic;

using Kvfx.Messages;
using Kvfx.Models;

namespace Kvfx.ViewModels
{
    public class AccountViewModel : Screen
    {
        private IEventAggregator eventAggregator;

        public List<User> Users { get; private set; }

        public AccountViewModel(IEventAggregator ea)
        {
            eventAggregator = ea;

            Users = User.LoadAll();
        }

        public void UserButtonClick(int id)
        {
            eventAggregator.PublishOnUIThread(new AccountMessage
            {
                Topic = "ToBrowse",
                UserId = id,
            });
        }
    }
}

using Caliburn.Micro;

using System.Windows.Input;

namespace Kvfx.ViewModels
{
    public class SearchViewModel : Screen
    {
        private IEventAggregator eventAggregator;

        public SearchViewModel(IEventAggregator ea)
        {
            eventAggregator = ea;
        }

        public void KeyDown(Key k)
        {

        }
    }
}

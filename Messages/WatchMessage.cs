using System.Windows.Input;

namespace Kvfx.Messages
{
    public class WatchMessage : BaseMessage
    {
        public int ContentId { get; set; }

        public Key Key { get; set; }
    }
}

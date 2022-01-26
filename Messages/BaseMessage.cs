namespace Kvfx.Messages
{
    public class BaseMessage
    {
        public string Topic { get; set; }

        public string Contents { get; set; }

        public object Sender { get; set; }
    }
}

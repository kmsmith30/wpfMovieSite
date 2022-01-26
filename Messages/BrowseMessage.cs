namespace Kvfx.Messages
{
    public class BrowseMessage : BaseMessage
    {
        public string ContentType { get; set; }

        public int ContentId { get; set; }
    }
}

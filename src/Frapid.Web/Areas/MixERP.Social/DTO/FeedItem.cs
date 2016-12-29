namespace MixERP.Social.DTO
{
    public class FeedItem : Feed
    {
        public long RowNumber { get; set; }
        public string CreatedByName { get; set; }
        public long ChildCount { get; set; }
    }
}
namespace Frapid.Messaging
{
    internal interface IMessage
    {
        string FromName { get; set; }
        string FromEmail { get; set; }
        string SentTo { get; set; }
        string Message { get; set; }
    }
}
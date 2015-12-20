namespace Frapid.Messaging
{
    public sealed class EmailProcessor
    {
        //Todo
        public static IEmailProcessor GetDefault()
        {
            return new Processor();
        }
    }
}
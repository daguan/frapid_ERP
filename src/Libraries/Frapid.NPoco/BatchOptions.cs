namespace Frapid.NPoco
{
    public class BatchOptions
    {
        public BatchOptions()
        {
            this.BatchSize = 20;
            this.StatementSeperator = ";";
        }

        public int BatchSize { get; set; }
        public string StatementSeperator { get; set; }
    }
}
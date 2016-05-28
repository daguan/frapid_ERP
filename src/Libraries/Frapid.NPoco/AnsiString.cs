namespace Frapid.NPoco
{
    public class AnsiString
    {
        public AnsiString(string str)
        {
            this.Value = str;
        }
        public string Value { get; private set; }
    }
}
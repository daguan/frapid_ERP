namespace Frapid.Reports.Engine.Parsers
{
    public sealed class TitleParser
    {
        public string Path { get; set; }

        public TitleParser(string path)
        {
            this.Path = path;
        }
        public string Get()
        {
            return XmlHelper.GetNodeText(this.Path, "/FrapidReport/Title");
        }
    }
}
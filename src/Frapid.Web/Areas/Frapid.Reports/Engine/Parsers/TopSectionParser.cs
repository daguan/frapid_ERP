namespace Frapid.Reports.Engine.Parsers
{
    public sealed class TopSectionParser
    {
        public string Path { get; set; }

        public TopSectionParser(string path)
        {
            this.Path = path;
        }

        public string Get()
        {
            return XmlHelper.GetNodeText(this.Path, "/FrapidReport/TopSection");
        }
    }
}
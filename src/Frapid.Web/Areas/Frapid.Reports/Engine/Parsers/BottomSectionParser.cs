namespace Frapid.Reports.Engine.Parsers
{
    public sealed class BottomSectionParser
    {
        public string Path { get; set; }

        public BottomSectionParser(string path)
        {
            this.Path = path;
        }

        public string Get()
        {
            return XmlHelper.GetNodeText(this.Path, "/FrapidReport/BottomSection");
        }
    }
}
using Frapid.WebsiteBuilder.Contracts;

namespace Frapid.WebsiteBuilder.Plugins
{
    public class HitExtension : IContentExtension
    {
        const string Template = @"<script>
                                $(document).ready(function($)
                                        {
                                            (function() { $.post(document.location + ""/hit""); })();
                                        });
                                </script>";
        public string ParseHtml(string tenant, string html)
        {
            return html + Template;
        }
    }
}
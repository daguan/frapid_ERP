using System;
using Frapid.Framework.StaticContent;
using Serilog;

namespace Frapid.AssetBundling
{
    public class StyleBundler : Bundler
    {
        public StyleBundler(ILogger logger, Asset asset) : base(logger, asset)
        {
        }

        protected override string Minify(string fileName, string contents)
        {
            try
            {
                return this.Minifier.MinifyStyleSheet(contents);
            }
            catch (Exception ex)
            {
                //Swallow   
                this.Logger.Error("The file {fileName} could not be minified due to error. {Message}", fileName, ex.Message);
            }

            //Fallback to original content.
            return contents;
        }
    }
}
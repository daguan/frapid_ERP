using System;
using Frapid.Framework.StaticContent;
using Microsoft.Ajax.Utilities;
using Serilog;

namespace Frapid.AssetBundling
{
    public class ScriptBundler : Bundler
    {
        public ScriptBundler(ILogger logger, Asset asset) : base(logger, asset)
        {
        }

        protected override string Minify(string file, string contents)
        {
            try
            {
                return this.Minifier.MinifyJavaScript(contents, new CodeSettings());
            }
            catch (Exception ex)
            {
                //Swallow   
                this.Logger.Error("The file \"{file}\" could not be minified due to error. {Message}", file, ex.Message);
            }

            //Fallback to original content.
            return contents;
        }
    }
}
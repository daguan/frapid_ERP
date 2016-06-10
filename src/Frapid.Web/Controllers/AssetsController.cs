using System;
using System.Web.Mvc;
using Frapid.ApplicationState;
using Frapid.ApplicationState.CacheFactory;
using Frapid.Areas;
using Frapid.AssetBundling;
using Serilog;

namespace Frapid.Web.Controllers
{
    [Route("assets")]
    public sealed class AssetsController : FrapidController
    {
        public AssetsController()
        {
            this.Factory = new DefaultCacheFactory();
        }

        private ICacheFactory Factory { get; }

        private string GetContents(string key)
        {
            return this.Factory.Get<string>(key);
        }

        private void SetContents(string key, string contents, DateTimeOffset expiresOn)
        {
            this.Factory.Add(key, contents, expiresOn);
        }

        [Route("assets/js/{*name}")]
        public ActionResult Js(string name)
        {
            var asset = AssetDiscovery.FindByName(name);

            if (asset == null)
            {
                return this.HttpNotFound();
            }

            string key = "assets.scripts." + name;
            string contents = this.GetContents(key);

            if (string.IsNullOrWhiteSpace(contents))
            {
                var compressor = new ScriptBundler(Log.Logger, asset);
                contents = compressor.Compress();

                this.SetContents(key, contents, DateTimeOffset.UtcNow.AddMinutes(asset.CacheDurationInMinutes));
            }

            this.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(asset.CacheDurationInMinutes));
            return this.Content(contents, "text/javascript");
        }

        [Route("assets/css/{*name}")]
        public ActionResult Css(string name)
        {
            var asset = AssetDiscovery.FindByName(name);
            string key = "assets.styles." + name;
            string contents = this.GetContents(key);

            if (string.IsNullOrWhiteSpace(contents))
            {
                var compressor = new StyleBundler(Log.Logger, asset);
                contents = compressor.Compress();

                this.SetContents(key, contents, DateTimeOffset.UtcNow.AddMinutes(asset.CacheDurationInMinutes));
            }

            this.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(asset.CacheDurationInMinutes));
            return this.Content(contents, "text/css");
        }
    }
}
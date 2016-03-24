using System.Collections.Generic;

namespace Frapid.Framework
{
    public interface ISiteMapGenerator
    {
        List<SiteMapUrl> Generate();
    }
}
namespace Sitecore.Support.XA.Feature.ErrorHandling
{
    using Sitecore.Configuration;
    using Sitecore.Data.Items;
    using Sitecore.Links;
    using Sitecore.Web;
    using System;

    public class StaticErrorPageRenderer : Sitecore.XA.Feature.ErrorHandling.StaticErrorPageRenderer
    {
        protected override string GetItemUrl(Item item, SiteInfo site)
        {
            UrlOptions options = LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = true;
            options.Site = Factory.GetSite(site.Name);
            
            string itemUrl = LinkManager.GetItemUrl(item, options);
            if (itemUrl.StartsWith("://", StringComparison.Ordinal))
            {
                itemUrl = "http" + itemUrl;
            }
            #region Added code
            Uri host = new Uri(itemUrl);
            string[] hostNames = site.HostName.Split(new char[] {'|'});
            foreach (var hostName in hostNames)
            {
                if (!hostName.Contains("*"))
                {
                    return host.Scheme + "://" + hostName + host.PathAndQuery;
                }
            }
            if (site.Matches(WebUtil.GetHostName()) && !string.IsNullOrEmpty(site.TargetHostName))
            {
                return host.Scheme + "://" + WebUtil.GetHostName() + host.PathAndQuery;
            }
            #endregion
            return itemUrl;
        }
    }
}
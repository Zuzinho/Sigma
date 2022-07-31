using System;
using System.Collections.Generic;
using Sigma.Models;

namespace Sigma
{
    public class LinksConverter
    {
        private static readonly List<string> _linksProviders = new List<string>
        {
            "vk.com",
            "github.com",
            "linkedin.com",
            "telegram.org",
            "facebook.com",
            "twitter.com",
            "youtube.com"
        };
        private static string _getProvider(string Url)
        {
            foreach(var Link in _linksProviders)
            {
                if (Url.Contains(Link))
                {
                    return Link;
                }
            }
            return null;
        }
        public static Link GetLink(string Url)
        {
            string link_provider = _getProvider(Url);
            if (link_provider != null)
            {
                Link link = new Link(1,link_provider,Url,1, "/Content/img/links_icons/" + link_provider + ".png");
                return link;
            }
            return null;
        }
    }
}

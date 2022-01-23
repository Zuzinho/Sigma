using System;
using System.Collections.Generic;
using Sigma;

namespace Sigma
{
    public class Links
    {
        private static List<String> Links_providers = new List<String>
        {
            "vk.com",
            "github.com",
            "linkedin.com",
            "telegram.org",
            "facebook.com",
            "twitter.com",
            "youtube.com"
        };
        private static Dictionary<string, string> links_icons = new Dictionary<string, string> {
            { "vk.com", "/Content/img/links_icons/VK - Original.png"},
            { "github.com", "/Content/img/links_icons/Github - Original.png"},
            { "linkedin.com", "/Content/img/links_icons/Linkedln - Original.png"},
            { "telegram.org", "/Content/img/links_icons/Telegram - Original.png"},
            { "facebook.com", "/Content/img/links_icons/Facebook - Original.png"},
            { "twitter.com", "/Content/img/links_icons/Twitter - Original.png"},
            { "youtube.com", "/Content/img/links_icons/YouTube - Original.png"}
        };
        private static string getProvider(string Url)
        {
            string Url_provider=null;
            foreach(var Link in Links_providers)
            {
                if (Url.Contains(Link))
                {
                    Url_provider = Link;
                    break;
                }
            }
            return Url_provider;
        }
        public static Models.Link getLink(string Url)
        {
            string link_provider = getProvider(Url);
            if (link_provider != null)
            {
                var Link_logo = links_icons[link_provider];
                if (!string.IsNullOrEmpty(Link_logo))
                {
                    Models.Link link = new Models.Link
                    {
                        Provider = link_provider,
                        Link1 = Url,
                        provider_avatar = Link_logo
                    };
                    return link;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static bool existProvider(string Url, List<Models.Link> user_links)
        {
            bool exist = false;
            string urlProvider = getProvider(Url);
            foreach (var Link in user_links)
            {
                if(Link.Provider == urlProvider)
                {
                    exist = true;
                    break;
                }
            }
            return exist;
        }
    }
}

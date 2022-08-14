namespace Sigma.Models
{
    public class Link
    {
        public Link(int id, string provider, string url, int userId, string providerAvatarUrl)
        {
            Id = id;
            Provider = provider;
            Url = url;
            UserId = userId;
            ProviderAvatarUrl = providerAvatarUrl;
        }

        public int Id { get; set; }
        public string Provider { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
        public string ProviderAvatarUrl { get; set; }
    }
}
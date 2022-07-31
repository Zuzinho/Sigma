namespace Sigma.Models
{
    public class Project
    {
        public Project(int id, string title, string about, int userId, string technology, bool selected = false, string photoUrl = "/Content/img/title.jpg", string url = "")
        {
            Id = id;
            Title = title;
            PhotoUrl = photoUrl;
            Url = url;
            About = about;
            UserId = userId;
            Technology = technology;
            Selected = selected;
        }
        public void ChangeData(string title,string about,string technology, bool selected = false, string photoUrl = "/Content/img/title.jpg", string url = "") 
        {
            Title = title;
            PhotoUrl = photoUrl;
            Url = url;
            About = about;
            Technology = technology;
            Selected = selected;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public string Url { get; set; }
        public string About { get; set; }
        public int UserId { get; set; }
        public string Technology { get; set; }
        public bool Selected { get; set; }
    }
}
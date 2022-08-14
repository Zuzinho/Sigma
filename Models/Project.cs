using System.IO;
using System.Web;

namespace Sigma.Models
{
    public class Project
    {

        public Project(int id, string title, string about, int userId, string technology, HttpPostedFileBase Avatar, string url = "")
        {

            Id = id;
            Title = title;
            Url = url;
            About = about;
            UserId = userId;
            Technology = technology;
            Selected = false;
            if (Avatar == null) return;
            string path = "C:\\Users\\user\\source\\repos\\Sigma\\Content\\img\\Projects_avatars";
            string ProjectAvatarName = "avatar" + id.ToString() + ".jpg";
            Avatar.SaveAs(Path.Combine(path, ProjectAvatarName));
            PhotoUrl = "/Content/img/Projects_avatars/" + ProjectAvatarName;
        }

        public Project(int id, string title, string about, int user_id, string technology, bool selected, string photoUrl, string url)
        {
            Id = id;
            Title = title;
            About = about;
            UserId = user_id;
            Technology = technology;
            Selected = selected;
            PhotoUrl = photoUrl;
            Url = url;
        }

        public void ChangeData(string title, string about, string technology, HttpPostedFileBase Avatar = null, string url = "")
        {
            Title = title;
            Url = url;
            About = about;
            Technology = technology;
            if (Avatar == null) return;
            string path = "C:\\Users\\user\\source\\repos\\Sigma\\Content\\img\\Projects_avatars";
            string ProjectAvatarName = "avatar" + Id.ToString() + ".jpg";
            Avatar.SaveAs(Path.Combine(path, ProjectAvatarName));
            PhotoUrl = "/Content/img/Projects_avatars/" + ProjectAvatarName;
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
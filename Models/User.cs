using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Sigma.Models
{
    public class User
    {
        public User()
        {

        }
        public User(int id, string name, string avatarUrl, string position, string about)
        {
            Id = id;
            Name = name;
            AvatarUrl = avatarUrl;
            Position = position;
            About = about;
        }

        public User(int id)
        {
            Id = id;
            Name = "Jane Keptton";
            AvatarUrl = "/Content/img/avatars/avatar0.jpg";
            Position = "senior .Net programmer";
            DataBase.CreateProjectsTable(Id);
            DataBase.CreateLinksTable(Id);
            InitData();
        }
        public void InitData()
        {
            Projects = DataBase.GetProjects(Id);
            Links = DataBase.GetLinks(Id);
            Form = DataBase.GetForm(Id);
        }
        public void InitData(Form userForm)
        {
            Form = userForm;
            Projects = DataBase.GetProjects(Id);
            Links = DataBase.GetLinks(Id);

        }
        public void ChangeData(string UserName, string UserPosition, string UserAbout, HttpPostedFileBase UserAvatar)
        {
            Name = UserName;
            Position = UserPosition;
            About = UserAbout;
            if (UserAvatar == null) return;
            string path = "C:\\Users\\user\\source\\repos\\Sigma\\Content\\img\\avatars";
            string UserAvatarName = "avatar" + Id.ToString() + ".jpg";
            UserAvatar.SaveAs(Path.Combine(path, UserAvatarName));
            AvatarUrl = "/Content/img/avatars/" + UserAvatarName;
        }
        public static bool IsOwner(User currentUser,int userId) { 
            return currentUser != null && (currentUser.Id == userId); 
        }
        public static int GetNewUserId(List<User> users)
        {
            var lastUser = users.AsEnumerable().LastOrDefault();
            return lastUser == null ? 1 : lastUser.Id + 1;
        }
        public int GetNewProjectId()
        {
            var lastUser = Projects.AsEnumerable().LastOrDefault();
            return lastUser == null ? 1 : lastUser.Id + 1;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Position { get; set; }
        public string About { get; set; }
        public List<Project> Projects { get; set; }
        public List<Link> Links { get; set; }
        public Form Form { get; set; }
    }
}
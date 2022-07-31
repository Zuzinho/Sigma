using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sigma.Models;
using System.Data.Entity;
using System.IO;
using System.Web;

namespace Sigma.Controllers
{
    public class HomeController : Controller
    {
        private static int _userId = 0;
        private static User _userProfile;
        private static List<Project> _userProjects;
        private static List<Link> _userLinks;
        private static Form _userForm;

        private static Database1Entities _db;

        private static readonly string _fullPath = "C:\\Users\\user\\Source\\Repos\\Sigma";

        private static Mail _mail;

        public HomeController()
        {
            _db = new Database1Entities();
            _mail = new Mail();
        }

        public ActionResult Index(string order = "", int page = 1)
        {
            int pageSize = 12;
            string filterOrder = order.ToLower().Trim();
            List<User> users = _db.Users.Where(x => x.Name.ToLower().Contains(filterOrder)).ToList();
            List<List<Project>> usersProjects = new List<List<Project>>();
            List<User> usersOnPage = new List<User>();
            for (int i = (page - 1) * pageSize; i < Math.Min(page * pageSize,users.Count); i++) usersOnPage.Add(users[i]);
            foreach(var user in usersOnPage) usersProjects.Add(DataBase.GetProjects(user.Id));
            int pagesCount = 1 + users.Count/(pageSize + 1);
            if (page > pagesCount) return Content("<h1>Страница не найдена</h1>");
            var pages = Paginations.GetPagination(pagesCount, page);
            pages.Add(page);
            return View((usersProjects, usersOnPage, pages,order));
        }
        public ActionResult UserPage(int userId)
        {
            bool isOwner = userId == _userId;
            User user = isOwner ? _userProfile : _db.Users.FirstOrDefault(x=>x.Id == userId);
            if(user == null) return Content("<h1>Страница не найдена</h1>");
            List<Project> projects = isOwner ? _userProjects : DataBase.GetProjects(user.Id);
            List<Link> links = isOwner ? _userLinks : DataBase.GetLinks(user.Id);
            return View((projects,user,isOwner,links));
        }
        public ActionResult AboutPage()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string email, string password)
        {
            _userForm = _db.Forms.FirstOrDefault(x => x.Email == email);
            if (_userForm != null) return View(); // Уже используется
            _userForm = new Form()
            {
                Email = email,
                Password = password
            };
            var lastForm = _db.Forms.AsEnumerable().LastOrDefault();
            _userForm.Id = lastForm == null ? 1 : lastForm.Id + 1;
            var lastUser = _db.Users.AsEnumerable().LastOrDefault();
            _userId = lastUser == null ? 1 : lastUser.Id + 1;
            _userProfile = new User(_userId);
            _userProjects = new List<Project>();
            _userLinks = new List<Link>();
            DataBase.CreateProjectsTable(_userId);
            DataBase.CreateLinksTable(_userId);
            _userForm.UserId = _userId;
            _db.Forms.Add(_userForm);
            _db.Users.Add(_userProfile);
            _db.SaveChanges();
            return RedirectToAction("UserPage",new {userId = _userId});
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string email, string password)
        {
            email = email.Trim();
            _userForm = _db.Forms.FirstOrDefault(x=>x.Email == email);
            if (_userForm == null) return View();//Неверный логин или пароль
            if(_userForm.Password != password) return View();//Неверный логин или пароль
            _userId = (int)_userForm.UserId;
            _userProfile = _db.Users.FirstOrDefault(x=>x.Id == _userId);
            _userProjects = DataBase.GetProjects(_userId);
            _userLinks = DataBase.GetLinks(_userId);
            return RedirectToAction("UserPage", new { userId = _userId });
        }

        public ActionResult MyPortfolio()
        {
            return _userId>0? RedirectToAction("UserPage", "Home", new { userId = _userId }): RedirectToAction("SignIn", "Home");
        }

        public ActionResult DeleteProfile()
        {
            _db.Entry(_userProfile).State = EntityState.Deleted;
            _db.Entry(_userForm).State = EntityState.Deleted;
            DataBase.DeleteData(_userId);
            _userProfile = null;
            _userForm = null;
            _userProjects.Clear();
            _userLinks.Clear();
            _userId = 0;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditUserPage()
        {
            var view = (_userProjects, _userProfile, _userLinks);
            return View(view);
        }

        [HttpPost]
        public ActionResult EditUserPage(HttpPostedFileBase UserAvatar, string UserName, string UserPosition, string UserAbout)
        {
            _userProfile.ChangeData(UserName, UserPosition, UserAbout);
            string path = _fullPath + "\\Content\\img\\avatars";
            if (UserAvatar != null)
            {
                string UserAvatarName = "avatar" + _userId.ToString() + ".jpg";
                UserAvatar.SaveAs(Path.Combine(path, UserAvatarName));
                _userProfile.AvatarUrl = "/Content/img/avatars/" + UserAvatarName;
            }
            _db.Entry(_userProfile).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("UserPage", "Home", new { userId = _userId });
        }

        [HttpGet]
        public ActionResult AddProject()
        {
            string[] avatar = { _userProfile.AvatarUrl };
            return View(avatar);
        }
        [HttpPost]
        public ActionResult AddProject(string Name,string Technos,string ProjectAbout, HttpPostedFileBase AvatarUrl)
        {
            var lastProject = _userProjects.AsEnumerable().LastOrDefault();
            var projectId = lastProject == null ? 1 : lastProject.Id + 1;
            Project project = new Project(projectId, Name, ProjectAbout, _userId, Technos);
            if (AvatarUrl != null)
            {
                string path = _fullPath + "\\Content\\img\\Projects_avatars";
                string ProjectAvatarName = "avatar" + projectId.ToString() + ".jpg";
                AvatarUrl.SaveAs(Path.Combine(path, ProjectAvatarName));
                project.PhotoUrl = "/Content/img/Projects_avatars/" + ProjectAvatarName;
            }
            _userProjects.Add(project);
            DataBase.AddProject(_userId, project);
            _db.SaveChanges();
            return RedirectToAction("UserPage", "Home", new { userId = _userId });
        }

        public ActionResult AllProjects(int userId,string order = "", int page = 1)
        {
            int pageSize = 6;
            string filterOrder = order.ToLower().Trim();
            var user = userId == _userId? _userProfile: _db.Users.FirstOrDefault(x=>x.Id == userId);
            var projects = userId == _userId? _userProjects: DataBase.GetProjects(user.Id);
            projects = projects.Where(x=>x.Title.ToLower().Contains(filterOrder)).ToList();
            List<Project> projectsOnPage = new List<Project>();
            for (int i = (page - 1) * pageSize; i < Math.Min(projects.Count, page * pageSize); i++) projectsOnPage.Add(projects[i]);
            int pagesCount = 1 + projects.Count / (pageSize + 1);
            if (page > pagesCount) return Content("<h1>Страница не найдена</h1>");
            var pages = Paginations.GetPagination(pagesCount, page);
            pages.Add(page);
            return View((projectsOnPage, user, pages,order));
        }

        public ActionResult ProjectPage(int projectId,int userId)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            var projects = DataBase.GetProjects(user.Id);
            Project project = projects.FirstOrDefault(x => x.Id == projectId);
            if (project == null) return Content("<h1>Страница не найдена</h1>");
            var otherProjects = Reference.GetRange(projects, project);
            return View((project, user, otherProjects, userId == _userId));
        }
        [HttpGet]
        public ActionResult EditProjectPage(int projectId)
        {
            var project = _userProjects.FirstOrDefault(x => x.Id == projectId);
            return View((project, _userProfile.AvatarUrl));
        }
        [HttpPost]
        public ActionResult EditProjectPage(string Name, string Technos, string ProjectAbout, HttpPostedFileBase AvatarUrl, int projectId)
        {
            var project = _userProjects.FirstOrDefault(x => x.Id == projectId);
            project.ChangeData(Name, ProjectAbout, Technos);
            if (AvatarUrl != null)
            {
                string path = _fullPath + "\\Content\\img\\Projects_avatars";
                string ProjectAvatarName = "avatar" + projectId.ToString() + ".jpg";
                AvatarUrl.SaveAs(Path.Combine(path, ProjectAvatarName));
                project.PhotoUrl = "/Content/img/Projects_avatars/" + ProjectAvatarName;
            }
            DataBase.ChangeData(_userId, project);
            _db.SaveChanges();
            return RedirectToAction("ProjectPage", "Home", new { projectId , userId = _userId});
        }

        public ActionResult DeleteProject(int projectId)
        {
            var project = _userProjects.Find(x => x.Id == projectId);
            if (!project.PhotoUrl.Contains("title"))
            {
                System.IO.File.Delete(_fullPath + project.PhotoUrl);
            }
            DataBase.DeleteProject(_userId, projectId);
            _userProjects.Remove(project);
            _db.SaveChanges();
            return RedirectToAction("UserPage", "Home", new { userId  = _userId });
        }

        [HttpGet]
        public ActionResult PasswordRecover()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordRecover(string email, string password)
        {
            _userForm = _db.Forms.FirstOrDefault(x => x.Email == email);
            if (_userForm == null) return View();//Неверный логин
            _userForm.Password = password;
            var code = Generator.GenerateCode();
            _userForm.RecoverCode = Convert.ToInt32(code);
            _mail.SendMessage(email, code);
            return RedirectToAction("CodeConfirmation", "Home", new { email, code });
        }
        [HttpGet]
        public ActionResult CodeConfirmation(string email) {
            string[] view = { email };
            return View(view);
        }

        [HttpPost]
        public ActionResult CodeConfirmation(int code)
        {
            string[] view = { _userForm.Email };
            if (code != _userForm.RecoverCode) return View(view);
            _db.Entry(_userForm).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("SignIn");
        }
        public ActionResult ExitProfile() {
            _userId = 0;
            return RedirectToAction("Index");
        }

        public void DeleteLink(int id)
        {
            var deleting_link = _userLinks.Find(x => x.Id == id);
            DataBase.DeleteLink(_userId,deleting_link.Id);
            _userLinks.Remove(deleting_link);
            _db.SaveChanges();
        }
        [HttpPost]
        public void GetProjects(string idsArray_str)
        {
            var idsList = idsArray_str.Split(',');
            for (int i = 0; i < idsList.Length; i++)
            {
                idsList[i] = idsList[i].Substring(7);
            }
            foreach (var project in _userProjects)
            {
                project.Selected = idsList.Contains(project.Id.ToString());
                DataBase.ChangeData(_userId,project);
            }
            if (_userProjects.Count > 0) _db.SaveChanges();
        }

        public void AddLink(string Url,string id)
        {
            Link link = LinksConverter.GetLink(Url);
            link.UserId = _userId;
            id = id.Substring(4);
            link.Id = Convert.ToInt32(id);
            DataBase.AddLink(_userId, link);
            _userLinks.Add(link);
            _db.SaveChanges();
        }
    }
}   
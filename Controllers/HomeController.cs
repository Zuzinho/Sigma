using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sigma.Models;
using System.IO;
using System.Web;

namespace Sigma.Controllers
{
    public class HomeController : Controller
    {
        private static User _currentUser;

        private static readonly List<User> _users = DataBase.GetUsers();

        private static Form _passwordRecoverForm;

        private static readonly string _directoryPath = "C:\\Users\\user\\source\\repos\\Sigma";

        private static readonly Mail _mail = new Mail();

        [Route("{Controller}/{action}/?order={order}&page={page}")]
        public ActionResult Index(string order = "", int page = 1)
        {
            int pageSize = 12;
            string filterOrder = order.ToLower().Trim();
            List<User> users = _users.Where(user => user.Name.ToLower().Contains(filterOrder)).ToList();
            int pagesCount = 1 + users.Count / (pageSize + 1);
            if (page > pagesCount) return Content("<h1>Страница не найдена</h1>");
            var pages = Paginations.GetPagination(pagesCount, page);
            pages.Add(page);
            int startedIndex = (page - 1) * pageSize;
            List<User> usersOnPage = users.GetRange(startedIndex, Math.Min(pageSize,users.Count - startedIndex));
            return View((usersOnPage, pages,order));
        }
        [Route("/{action}/?userId={userId}")]
        public ActionResult UserPage(int userId)
        {
            bool isOwner = Models.User.IsOwner(_currentUser,userId);
            User user = isOwner ? _currentUser : _users.FirstOrDefault(x=>x.Id == userId);
            if(user == null) return Content("<h1>Страница не найдена</h1>");
            return View((user,isOwner));
        }
        [Route("/{action}")]
        public ActionResult AboutPage()
        {
            return View();
        }
        [HttpGet]
        [Route("/{action}")]
        public ActionResult SignUp(string message = "")
        {
            return View(new string[] { message });
        }

        [HttpPost]
        public ActionResult SignUp(string email, string password)
        {
            _currentUser = _users.FirstOrDefault(x => x.Form.Email == email);
            if (_currentUser != null) return RedirectToAction("SignUp",new {message = "This email is already used" });
            int userId = Models.User.GetNewUserId(_users);
            _currentUser = new User(userId);
            _currentUser.InitData(new Form(userId, email, password));
            _users.Add(_currentUser);
            return RedirectToAction("UserPage",new { userId = _currentUser.Id });
        }

        [HttpGet]
        [Route("/{action}")]
        public ActionResult SignIn(string message = "")
        {
            return View(new string[] { message });
        }

        [HttpPost]
        public ActionResult SignIn(string email, string password)
        {
            email = email.Trim();
            _currentUser = _users.FirstOrDefault(x=>x.Form.Email == email);
            if (_currentUser == null) return RedirectToAction("SignIn", new { message = "Incorrect email or password" });//Неверный логин или пароль
            if(_currentUser.Form.Password != password) RedirectToAction("SignIn", new { message = "Incorrect email or password" });//Неверный логин или пароль
            return RedirectToAction("UserPage", new { userId = _currentUser.Id });
        }

        public ActionResult MyPortfolio()
        {
            if(_currentUser == null) return RedirectToAction("SignIn", "Home");
            return _currentUser.Id>0? RedirectToAction("UserPage", "Home", new { userId = _currentUser.Id }): RedirectToAction("SignIn", "Home");
        }

        public ActionResult DeleteProfile()
        {
            DataBase.DeleteData(_currentUser.Id);
            _users.Remove(_currentUser);
            _currentUser = null;
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("UserPage/{action}")]
        public ActionResult EditUserPage()
        {
            return View(_currentUser);
        }

        [HttpPost]
        public ActionResult EditUserPage(HttpPostedFileBase UserAvatar, string UserName, string UserPosition, string UserAbout)
        {
            _currentUser.ChangeData(UserName, UserPosition, UserAbout,UserAvatar);
            DataBase.ChangeData(_currentUser);
            return RedirectToAction("UserPage", "Home", new { userId = _currentUser.Id });
        }

        [HttpGet]
        [Route("/UserPage/{action}")]
        public ActionResult AddProject()
        {
            return View(new string[] { _currentUser.AvatarUrl });
        }
        [HttpPost]
        public ActionResult AddProject(string Name,string Technos,string ProjectAbout, HttpPostedFileBase Avatar)
        {
            var projectId = _currentUser.GetNewProjectId();
            Project project = new Project(projectId, Name, ProjectAbout, _currentUser.Id, Technos, Avatar);
            _currentUser.Projects.Add(project);
            DataBase.AddProject(_currentUser.Id, project);
            return RedirectToAction("UserPage", "Home", new { userId = _currentUser.Id });
        }
        [Route("/UserPage/?userId={userId}/{action}/?order={order}&page={page}")]
        public ActionResult AllProjects(int userId,string order = "", int page = 1)
        {
            int pageSize = 6;
            string filterOrder = order.ToLower().Trim();
            bool isOwner = Models.User.IsOwner(_currentUser,userId);
            var user = isOwner? _currentUser: _users.FirstOrDefault(x=>x.Id == userId);
            var projects = isOwner? _currentUser.Projects: DataBase.GetProjects(userId);
            projects = projects.Where(x=>x.Title.ToLower().Contains(filterOrder)).ToList();
            int pagesCount = 1 + projects.Count / (pageSize + 1);
            if (page > pagesCount) return Content("<h1>Страница не найдена</h1>");
            int startedIndex = (page - 1) *pageSize;
            List<Project> projectsOnPage = projects.GetRange(startedIndex, Math.Min(pageSize, projects.Count - startedIndex));
            var pages = Paginations.GetPagination(pagesCount, page);
            pages.Add(page);
            return View((projectsOnPage, user, pages,order));
        }

        [Route("UserPage/?userId={userId}/{action}/?projectId={projectId}")]
        public ActionResult ProjectPage(int projectId,int userId)
        {
            var user = _users.FirstOrDefault(x => x.Id == userId);
            var projects = user.Projects;
            Project project = projects.FirstOrDefault(x => x.Id == projectId);
            if (project == null) return Content("<h1>Страница не найдена</h1>");
            var otherProjects = Reference.GetRange(projects, project,4);
            bool isOwner = Models.User.IsOwner(_currentUser, userId);
            return View((project, user, otherProjects, isOwner));
        }
        [HttpGet]
        [Route("/UserPage/{action}/?projectId={projectId}")]
        public ActionResult EditProjectPage(int projectId)
        {
            var project = _currentUser.Projects.FirstOrDefault(x => x.Id == projectId);
            return View((project, _currentUser.AvatarUrl));
        }
        [HttpPost]
        public ActionResult EditProjectPage(string Name, string Technos, string ProjectAbout, HttpPostedFileBase Avatar, int projectId)
        {
            var project = _currentUser.Projects.FirstOrDefault(x => x.Id == projectId);
            project.ChangeData(Name, ProjectAbout, Technos,Avatar);
            DataBase.ChangeData(_currentUser.Id, project);
            return RedirectToAction("ProjectPage", "Home", new { projectId , userId = _currentUser.Id});
        }

        public ActionResult DeleteProject(int projectId)
        {
            var project = _currentUser.Projects.Find(x => x.Id == projectId);
            if (!project.PhotoUrl.Contains("title"))
            {
                System.IO.File.Delete(_directoryPath + project.PhotoUrl);
            }
            DataBase.DeleteProject(_currentUser.Id, projectId);
            _currentUser.Projects.Remove(project);
            return RedirectToAction("UserPage", "Home", new { userId  = _currentUser.Id });
        }

        [HttpGet]
        [Route("/{action}")]
        public ActionResult PasswordRecover(string message = "")
        {
            return View(new string[] { message });
        }

        [HttpPost]
        public ActionResult PasswordRecover(string email, string password)
        {
            var user = _users.FirstOrDefault(x => x.Form.Email == email);
            _passwordRecoverForm = user.Form;
            if (_passwordRecoverForm == null) return RedirectToAction("PasswordRecover",new {message = "Incorrect login" });//Неверный логин
            _passwordRecoverForm.Password = password;
            var code = Generator.GenerateCode();
            _passwordRecoverForm.RecoverCode = Convert.ToInt32(code);
            _mail.SendMessage(email, code);
            return RedirectToAction("CodeConfirmation", new { email });
        }
        [HttpGet]
        [Route("/{action}")]
        public ActionResult CodeConfirmation(string email,string message = "") {
            return View(new string[] { email, message });
        }

        [HttpPost]
        public ActionResult CodeConfirmation(int code)
        {
            if (code != _passwordRecoverForm.RecoverCode) return RedirectToAction("CodeConfirmation",new { email = _passwordRecoverForm.Email,message =  "Incorrect code" });
            _currentUser = _users.FirstOrDefault(user => user.Id == _passwordRecoverForm.Id);
            _currentUser.InitData(_passwordRecoverForm);
            _passwordRecoverForm = null;
            DataBase.ChangeData(_currentUser.Form);
            return RedirectToAction("SignIn");
        }
        public ActionResult ExitProfile() {
            _currentUser = null;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public void DeleteLink(string idStr)
        {
            idStr = idStr.Substring(4);
            int id = Convert.ToInt32(idStr);
            var deleting_link = _currentUser.Links.Find(x => x.Id == id);
            DataBase.DeleteLink(_currentUser.Id,deleting_link.Id);
            _currentUser.Links.Remove(deleting_link);
        }
        [HttpPost]
        public void GetProjects(string idsArray_str)
        {
            var idsList = idsArray_str.Split(',');
            for (int i = 0; i < idsList.Length - 1; i++)
            {
                idsList[i] = idsList[i].Substring(7);
            }
            int countChange = 0;
            foreach (var project in _currentUser.Projects)
            {
                bool wasSelected = project.Selected;
                project.Selected = idsList.Contains(project.Id.ToString());
                if (wasSelected == project.Selected) continue;
                DataBase.ChangeData(_currentUser.Id,project);
                countChange++;
            }
        }
        [HttpPost]
        public void AddLink(string Url,string id)
        {
            Link link = LinksConverter.GetLink(Url);
            link.UserId = _currentUser.Id;
            link.Id = Convert.ToInt32(id.Substring(4));
            DataBase.AddLink(_currentUser.Id, link);
            _currentUser.Links.Add(link);
        }
    }
}
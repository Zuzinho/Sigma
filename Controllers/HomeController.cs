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
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Mvc.ApiController]
    public class HomeController : Controller
    {
        public static int user_id = 0;
        private int pageSize = 12;
        private int projectSize = 6;
        private static User user_profile = new User();
        private static User item_profile = new User(); 
        private static List<Project> user_projects = new List<Project>();
        private static List<Project> item_projects = new List<Project>();
        private static Form form;
        private static Database1Entities db = new Database1Entities();
        private static List<Link> user_links = new List<Link>();
        private string fullPath = "C:\\Users\\user\\Source\\Repos\\Sigma";
        private static string _Code;
        Mail mail = new Mail();

        public ActionResult Index(string order, int page = 1)
        {
            ViewData["order"] = order;
            var ordquery = from x in db.Projects select x;
            List<User> users_list = new List<User>();
            List<Project> projects_list = new List<Project>();
            if (!string.IsNullOrEmpty(order))
            {
                order = order.Trim().ToLower();
                ordquery = ordquery.Where(x => x.user_name.ToLower().Contains(order) || x.Title.ToLower().Contains(order) || x.technology.ToLower().Contains(order));
                foreach (var item in ordquery)
                {
                    var projects = ordquery.Where(x => x.technology.ToLower().Contains(order) || x.Title.ToLower().Contains(order));
                    projects_list.Add(item);
                    var user = db.Users.FirstOrDefault(x => x.Id == item.user_id);
                    if (!users_list.Contains(user))
                    {
                        users_list.Add(user);
                    }
                }
                var users = db.Users.Where(x => x.Name.ToLower().Contains(order));
                foreach (var user in users)
                {
                    if (!users_list.Contains(user))
                    {
                        users_list.Add(user);
                    }
                }
                List<User> users_list_ = new List<User>();
                for (int i = (page - 1) * pageSize; i < (page) * pageSize; i++)
                {
                    if (i >= users_list.Count)
                    {
                        break;
                    }
                    users_list_.Add(users_list[i]);
                }
                int count = 0;
                count = users_list.Count() / pageSize;
                if (users_list.Count() % pageSize > 0)
                {
                    count++;
                }
                if (count > 0)
                {
                    if(page > count)
                    {
                        return Content("<h1>Страница не найдена</h1>");
                    }
                    List<List<int>> pages_list = Paginations.paginations_list(count);
                    List<int> pages = pages_list[page - 1];
                    pages.Add(page);
                    var view = (projects_list, users_list_, pages);
                    return View(view);
                }
                else
                {
                    List<int> pages = new List<int>() { 0 };
                    var view = (projects_list, users_list_, pages);
                    return View(view);
                }
            }
            else
            {
                var projects = ordquery.ToList();
                var user = db.Users.ToList();
                List<User> users = new List<User>();
                for (int i = (page - 1) * pageSize; i < (page) * pageSize; i++)
                {
                    if (i >= user.Count)
                    {
                        break;
                    }
                    users.Add(user[i]);
                }
                int count = 0;
                count = user.Count / pageSize;
                if (user.Count % pageSize > 0)
                {
                    count++;
                }
                if (count > 0)
                {
                    if (page > count)
                    {
                        return Content("<h1>Страница не найдена</h1>");
                    }
                    List<List<int>> pages_list = Paginations.paginations_list(count);
                    List<int> pages = pages_list[page - 1];
                    pages.Add(page);
                    var view = (projects, users, pages);
                    return View(view);
                }
                else
                {
                    List<int> pages = new List<int> { 0 };
                    var view = (projects, users, pages);
                    return View(view);
                }
            }
        }
        public ActionResult UserPage(int item_id = -1)
        {
            bool is_owner;
            is_owner = (item_id == user_id);
            if (is_owner)
            {
                item_profile = user_profile;
                item_projects = user_projects;
                var view = (user_projects, user_profile, is_owner, user_links);
                return View(view);
            }
            else
            {
                var user = db.Users.FirstOrDefault(x => x.Id == item_id);
                var projects = db.Projects.Where(x => x.user_id == item_id).ToList();
                if (user == null)
                {
                    return Content("<h1>Страница не найдена</h1>");
                }
                item_profile = user;
                item_projects = projects;
                var links = db.Links.Where(x => x.user_id == item_id).ToList();
                var view = (projects, user, is_owner, links);
                return View(view);
            }
        }
        public ActionResult AboutPage()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Sign_up()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sign_up(string email, string password)
        {
            email = email.Trim();
            password = password.Trim();
            var user = db.Forms.FirstOrDefault(x => x.Email.Trim() == email);
            if (user == null)
            {
                form = new Form
                {
                    Email = email.Trim(),
                    Password = password.Trim()
                };
                _Code = Generator.GenerateCode();
                mail.SendMessage("nik.zozulya.04@mail.ru", _Code);
                return RedirectToAction("CodeConfirmation","Home",new {email = email,password = password});
            }
            else
            {
                return RedirectToAction("Sign_up", "Home");//Почта уже используется
            }

        }

        [HttpGet]
        public ActionResult Sign_in()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sign_in(string email, string password)
        {
            if (email.Trim().Length > 0 && password.Trim().Length > 0)
            {
                email = email.Trim();
                password = email.Trim();
                var user = db.Forms.FirstOrDefault(x => x.Email.Trim() == email);
                if (user != null)
                {
                    if (user.Password.Trim() == password)
                    {
                        user_id = user.Id;
                        user_profile = db.Users.FirstOrDefault(x => x.Id == user_id);
                        user_projects = db.Projects.Where(x => x.user_id == user_id).ToList();
                        user_links = db.Links.Where(x => x.user_id == user_id).ToList();
                        return RedirectToAction("UserPage", "Home", new { item_id = user_id });
                    }
                    else
                    {
                        return RedirectToAction("Sign_in", "Home");//неправильный логин или пароль
                    }
                }
                else
                {
                    return RedirectToAction("Sign_in", "Home");//неправильный логин или пароль
                }
            }
            else
            {
                return RedirectToAction("Sign_in", "Home");//неправильный логин или пароль
            }
        }

        public ActionResult MyPortfolio()
        {
            if (user_id > 0)
            {
                return RedirectToAction("UserPage", "Home", new { item_id = user_id });
            }
            else
            {
                return RedirectToAction("Sign_in", "Home");
            }
        }

        public ActionResult DeleteProfile()
        {
            db.Entry(user_profile).State = EntityState.Deleted;
            if (!user_profile.avatarUrl.Contains("avatar0.jpg"))
            {
                System.IO.File.Delete(fullPath + user_profile.avatarUrl);
            }
            foreach (var project in user_projects)
            {
                db.Entry(project).State = EntityState.Deleted;
            }
            db.Forms.Remove(db.Forms.FirstOrDefault(x => x.Id == user_id));
            db.SaveChanges();
            user_id = 0;
            return RedirectToAction("index", "Home", new { page = 1 });
        }

        [HttpGet]
        public ActionResult EditUserPage()
        {
            var view = (user_projects, user_profile, user_links);
            return View(view);
        }

        [HttpPost]
        public ActionResult EditUserPage(HttpPostedFileBase UserAvatar, string UserName, string User_position, string UserAbout)
        {
            var user_info = db.Users.FirstOrDefault(x => x.Id == user_id);
            string path = fullPath + "\\Content\\img\\avatars";
            if (UserAvatar != null)
            {
                string UserAvatarName = "avatar" + user_profile.Id.ToString() + ".jpg";
                UserAvatar.SaveAs(Path.Combine(path, UserAvatarName));
                user_info.avatarUrl = "/Content/img/avatars/" + UserAvatarName;
            }
            user_info.Name = UserName;
            user_info.position = User_position;
            user_info.about = UserAbout;
            var user_projects_info = db.Projects.Where(x => x.user_id == user_id).ToList();
            foreach (var project in user_projects_info)
            {
                project.user_name = UserName;
            }
            user_profile = user_info;
            user_projects = user_projects_info;
            db.SaveChanges();
            return RedirectToAction("UserPage", "Home", new { item_id = user_id });
        }

        [HttpGet]
        public ActionResult AddProject()
        {
            List<string> view = new List<string>
            {
                user_profile.avatarUrl
            };
            return View(view);
        }
        [HttpPost]
        public ActionResult AddProject(string Name,string Technos,string ProjectAbout, HttpPostedFileBase AvatarUrl)
        {
            Project project = new Project()
            {
                Title = Name,
                technology = Technos,
                About = ProjectAbout,
                user_name = user_profile.Name,
                user_id = user_id,
                selected = false
            };
            if (db.Projects.Count() > 0)
            {
                project.Id = db.Projects.AsEnumerable().Last().Id +1;
            }
            else
            {
                project.Id = 1;
            }
            if (AvatarUrl != null)
            {
                string path = fullPath + "\\Content\\img\\Projects_avatars";
                string ProjectAvatarName = "avatar" + project.Id.ToString() + ".jpg";
                AvatarUrl.SaveAs(Path.Combine(path, ProjectAvatarName));
                project.PhotoUrl = "/Content/img/Projects_avatars/" + ProjectAvatarName;
            }
            else
            {
                project.PhotoUrl = "/Content/img/title.jpg";
            }
            db.Projects.Add(project);
            user_projects.Add(project);
            db.SaveChanges();
            return RedirectToAction("UserPage", "Home", new { item_id = user_id });
        }

        public ActionResult AllProjects(string order, int page = 1, int item_id = 0)
        {
            User user = new User();
            List<Project> projects = new List<Project>();
            user = item_profile;
            projects = item_projects;
            if (user != null)
            {
                if (user.Id != item_id)
                {
                    user = db.Users.FirstOrDefault(x => x.Id == item_id);
                    item_profile = user;
                    if (user == null)
                    {
                        return Content("<h1>Страница не найдена</h1>");
                    }
                    else
                    {
                        projects = db.Projects.Where(x => x.user_id == item_id).ToList();
                        item_projects = projects;
                    }
                }
            }
            else
            {
                user = db.Users.FirstOrDefault(x => x.Id == item_id);
                if (user == null)
                {
                    return Content("<h1>Страница не найдена</h1>");
                }
                else
                {
                    projects = db.Projects.Where(x => x.user_id == item_id).ToList();
                    item_profile = user;
                    item_projects = projects;
                }
            }
            ViewData["order"] = order;
            if (String.IsNullOrEmpty(order))
            {
                List<Project> projects_list_ = new List<Project>();
                for (int i = (page - 1) * projectSize; i < (page) * projectSize; i++)
                {
                    if (i >= projects.Count)
                    {
                        break;
                    }
                    projects_list_.Add(projects[i]);
                }
                int count = 0;
                count = projects_list_.Count() / projectSize;
                if (projects_list_.Count() % projectSize > 0)
                {
                    count++;
                }
                if (count > 0)
                {
                    if (page > count)
                    {
                        return Content("<h1>Страница не найдена</h1>");
                    }
                    List<List<int>> pages_list = Paginations.paginations_list(count);
                    List<int> pages = pages_list[page - 1];
                    pages.Add(page);
                    var view = (projects_list_, user, pages);
                    return View(view);
                }
                else
                {
                    List<int> pages = new List<int> { 0 };
                    var view = (projects_list_, user, pages);
                    return View(view);
                }
            }
            else
            {
                order = order.Trim().ToLower();
                List<Project> projects_list_ = new List<Project>();
                var user_projects_ = projects.Where(x => x.Title.ToLower().Contains(order) || (x.technology.ToLower().Contains(order) && order!=",")).ToList();
                for (int i = (page - 1) * projectSize; i < (page) * projectSize; i++)
                {
                    if (i >= user_projects_.Count)
                    {
                        break;
                    }
                    projects_list_.Add(user_projects_[i]);
                }
                int count = 0;
                count = projects_list_.Count() / projectSize;
                if (projects_list_.Count() % projectSize > 0)
                {
                    count++;
                }
                if (count > 0)
                {
                    if (page > count)
                    {
                        return Content("<h1>Страница не найдена</h1>");
                    }
                    List<List<int>> pages_list = Paginations.paginations_list(count);
                    List<int> pages = pages_list[page - 1];
                    pages.Add(page);
                    var view = (projects_list_, user, pages);
                    return View(view);
                }
                else
                {
                    List<int> pages = new List<int> { 0 };
                    var view = (projects_list_, user, pages);
                    return View(view);
                }
            }
        }

        public ActionResult ProjectPage(int item_id = 0)
        {
            Project project = item_projects.FirstOrDefault(x => x.Id == item_id);
            if(project == null)
            {
                project = db.Projects.FirstOrDefault(x => x.Id == item_id);
                if (project == null)
                {
                    return Content("<h1>Страница не найдена</h1>");
                }
                else
                {
                    User user = db.Users.FirstOrDefault(x => x.Id == project.user_id);
                    var item_projects = db.Projects.Where(x => x.user_id == user.Id).ToList();
                    var other_projects = Reference.GetRange(item_projects, project);
                    var view = (project,user,other_projects,user == user_profile);
                    return View(view);
                }
            }
            else
            {
                var other_projects = Reference.GetRange(item_projects, project);
                var view = (project,item_profile,other_projects , item_profile == user_profile);
                return View(view);
            }
        }
        [HttpGet]
        public ActionResult EditProjectPage(int item_id = 0)
        {
            var project = item_projects.FirstOrDefault(x => x.Id == item_id);
            List<string> avatar = new List<string>
            {
                user_profile.avatarUrl
            };
            var view = (project, avatar);
            return View(view);
        }
        [HttpPost]
        public ActionResult EditProjectPage(string Name, string Technos, string ProjectAbout, HttpPostedFileBase AvatarUrl, int item_id = 0)
        {
            var project = db.Projects.FirstOrDefault(x => x.Id == item_id);
            if (AvatarUrl != null)
            {
                string path = fullPath + "\\Content\\img\\Projects_avatars";
                string ProjectAvatarName = "avatar" + project.Id.ToString() + ".jpg";
                AvatarUrl.SaveAs(Path.Combine(path, ProjectAvatarName));
                project.PhotoUrl = "/Content/img/Projects_avatars/" + ProjectAvatarName;
            }
            project.Title = Name;
            project.technology = Technos;
            project.About = ProjectAbout;
            foreach (var proj in user_projects)
            {
                if (proj.Id == item_id)
                {
                    proj.Title = Name;
                    proj.technology = Technos;
                    proj.About = ProjectAbout;
                    proj.PhotoUrl = project.PhotoUrl;
                    break;
                }
            }
            db.SaveChanges();
            return RedirectToAction("ProjectPage", "Home", new { item_id = item_id });
        }

        public ActionResult DeleteProject(int item_id = 0)
        {
            var project = user_projects.Find(x => x.Id == item_id);
            if (!project.PhotoUrl.Contains("title"))
            {
                System.IO.File.Delete(fullPath + project.PhotoUrl);
            }
            db.Entry(project).State = EntityState.Deleted;
            user_projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("UserPage", "Home", new { item_id  = user_id });
        }

        [HttpGet]
        public ActionResult CodeConfirmation(string email,string password) {
            var view = (email,password);
            return View(view);
        }

        [HttpPost]
        public ActionResult CodeConfirmation(string code) 
        {
            if (code == _Code)
            {
                if (db.Forms.Count() > 0)
                {
                    form.Id = db.Forms.AsEnumerable().Last().Id + 1;
                }
                else
                {
                    form.Id = 1;
                }
                user_id = form.Id;
                User user1 = new User
                {
                    Name = "Jane Keptton",
                    avatarUrl = "/Content/img/avatars/avatar0.jpg",
                    position = "Senior web enginer",
                    Id = user_id
                };
                db.Users.Add(user1);
                user_profile = user1;
                db.Forms.Add(form);
                db.SaveChanges();
                return RedirectToAction("UserPage", "Home", new { item_id = user_id });
            }
            return RedirectToAction("CodeConfirmation", "Home", new { email = form.Email, password = form.Password});
        }

        public void DeleteLink(string id_string = "")
        {
            var id = Convert.ToInt32(id_string.Substring(4));
            var deleting_link = user_links.Find(x => x.Id == id);
            if (deleting_link != null)
            {
                db.Entry(deleting_link).State = EntityState.Deleted;
                user_links.Remove(deleting_link);
            }
            db.SaveChanges();
        }

        public void GetProjects(string ids_array)
        {
            var user_projects_info = db.Projects.Where(x => x.user_id == user_id).ToList();
            var ids_array_ = ids_array.Split(',');
            if (ids_array.Length > 0)
            {
                for (int i = 0; i < ids_array_.Length; i++)
                {
                    ids_array_[i] = ids_array_[i].Substring(7);
                }
            }
            foreach (var project in user_projects)
            {
                project.selected = ids_array_.Contains(project.Id.ToString());
            }
            foreach (var project in user_projects_info)
            {
                project.selected = ids_array_.Contains(project.Id.ToString());
            }
            db.SaveChanges();
        }

        public void AddLink(string Url,string id)
        {
            Link link = Links.getLink(Url);
            link.user_id = user_id;
            id = id.Substring(4);
            link.Id = Convert.ToInt32(id);
            db.Links.Add(link);
            user_links.Add(link);
            db.SaveChanges();
        }
    }
}   
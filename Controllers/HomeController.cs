using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sigma.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Sigma.Controllers
{
    public class HomeController : Controller
    {
        public static int user_id = 0;
        const int page = 1;
        public int pageSize = 12    ;
        private Models.Database1Entities db = new Models.Database1Entities();
        public ActionResult Index(string order,int page = page)
        {
            ViewData["order"] = order;
            var ordquery = from x in db.Projects select x;
            List<User> users_list = new List<User>();
            List<Project> projects_list = new List<Project>();
            if (!String.IsNullOrEmpty(order))
            {
                order = order.Trim().ToLower();
                ordquery = ordquery.Where(x => x.user_name.Trim().ToLower().Contains(order) || x.Title.Trim().ToLower().Contains(order) || x.technology.Trim().ToLower().Contains(order));
                foreach (var item in ordquery)
                {
                    var projects = ordquery.Where(x => x.technology.Trim().ToLower().Contains(order) || x.Title.Trim().ToLower().Contains(order));
                    projects_list.Add(item);
                    var user = db.Users.FirstOrDefault(x => x.Id == item.user_id);
                    if (!users_list.Contains(user))
                    {
                        users_list.Add(user);   
                    }
                }
                var users = db.Users.Where(x => x.Name.Trim().ToLower().Contains(order));
                foreach(var user in users)
                {
                    if (!users_list.Contains(user))
                    {
                        users_list.Add(user);
                    }
                }
                List<User> users_list_ = new List<User>();
                for(int i = (page-1) * pageSize; i < (page) * pageSize; i++)
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
                List<List<int>> pages_list = Paginations.paginations_list(count);
                List<int> pages = pages_list[page - 1];
                pages.Add(page);
                var view = (projects_list, users_list_,pages);
                return View(view);
            }
            else
            {
                var projects = ordquery.ToList();
                var user = db.Users.ToList();
                List<User> users = new List<User>();
                for (int i = (page-1) * pageSize; i < (page) * pageSize; i++)
                {
                    if (i >= user.Count)
                    {
                        break;
                    }
                    users.Add(user[i]);
                }
                int count = 0;
                count = user.Count / pageSize;
                if(user.Count% pageSize > 0)
                {
                    count++;
                }
                List<List<int>> pages_list = Paginations.paginations_list(count);
                List<int> pages = pages_list[page - 1];
                pages.Add(page);
                var view = (projects, users,pages);
                return View(view);
            }
        }
        public ActionResult UserPage(int item_id = -1)
        {
            if(item_id == null)
            {
                item_id = -1;
            }
            var user = db.Users.FirstOrDefault(x => x.Id == item_id);
            var projects = db.Projects.Where(x=>x.user_id == item_id).ToList();
            if (user == null)
            {
                return Content("<h1>Страница не найдена</h1>");
            }
            List<bool> ids = new List<bool>();
            ids.Add(item_id==user_id);
            var view = (projects, user, ids);
            return View(view);
        }
        public ActionResult AboutPage()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Sign_up( )
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sign_up(string email, string password)
        {
            if (email.Trim().Length > 0 && password.Trim().Length > 0)
            {
                email = email.Trim();
                password = email.Trim();
                var user = db.Forms.FirstOrDefault(x => x.Email.Trim() == email);
                if (user == null)
                {
                    Form form = new Form
                    {
                        Email = email.Trim(),
                        Password = password.Trim(),
                        Id = db.Forms.Count() + 1
                    };
                    user_id = form.Id;
                    db.Forms.Add(form);
                    db.SaveChanges();
                    return RedirectToAction("UserPage", "Home", new { item_id = user_id });
                }
                else
                {
                    return RedirectToAction("Sign_up", "Home");//Почта уже используется
                }
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
        public ActionResult Sign_in(string email,string password)
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
                return RedirectToAction("Sign_in","Home");
            }
        }
        [HttpGet]
        public ActionResult DeleteProfile()
        {
            var user_profile = db.Users.FirstOrDefault(x=>x.Id == user_id);
            db.Users.Remove(user_profile);
            var user_projects = db.Projects.Where(x => x.user_id == user_id);
            foreach(var project in user_projects)
            {
                db.Projects.Remove(project);
            }
            db.SaveChanges();
            user_id = 0;
            return RedirectToAction("index","Home",new {page = 1});
        }
    }
}
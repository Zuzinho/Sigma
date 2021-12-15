using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sigma.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Sigma.Controllers
{
    public class HomeController : Controller
    {
        public static int user_id = 0;
        private Models.Database1Entities db = new Models.Database1Entities();
        public IActionResult Index()
        {
            var projects = db.Projects.ToList();
            var users = db.Users.ToList();
            var view = (projects, users);
            return View(view);
        }
        public ActionResult UserPage(int item_id)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == item_id);
            if (user == null)
            {
                return Content("<h1>Страница не найдена</h1>");
            }
            var projects = db.Projects.ToList();
            var view = (projects, user);
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
                Console.WriteLine(user);
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
        public Task<ActionResult> Index(string order)
        {
            order = order.Trim().ToLower();
            ViewData["order"] = order;
            var ordquery = from x in db.Projects select x;
            List<User> users_list = new List<User>();
            List<Project> projects_list = new List<Project>();
            if (!String.IsNullOrEmpty(order))
            {
                ordquery = ordquery.Where(x=>x.user_name.Trim().ToLower().Contains(order) || x.Title.Trim().ToLower().Contains(order) || x.technology.Trim().ToLower().Contains(order));
                foreach (var item in ordquery)
                {
                    var users = ordquery.Where(x => x.user_name.Trim().ToLower().Contains(order));
                    var projects = ordquery.Where(x => x.technology.Trim().ToLower().Contains(order) || x.Title.Trim().ToLower().Contains(order));
                    projects_list.Add(item);
                    users_list.Add(db.Users.FirstOrDefault(x => x.Id == item.user_id));
                }
                var list = (projects_list, users_list);
                return View(list);
            }
            else
            {
                return View();
            }

        }
    }
}
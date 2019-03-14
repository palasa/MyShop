using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private Entities db = new Entities();


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if ( ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();

                return Content("OK!");
            }
            return View();
        }


        public ActionResult CheckUsername(string username)
        {            
            return Json(db.User.Where(u => u.Username == username).Count() == 0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult UserList() {
            return Json(db.User.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
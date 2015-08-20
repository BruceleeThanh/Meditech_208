using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meditech.Controllers;

namespace Meditech.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Session["LogeUser"]==null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}
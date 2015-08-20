using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meditech.Models;
using System.Threading.Tasks;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace Meditech.Controllers
{
    public class AccountController : Controller
    {
        private MeditechEntities _meditech = new MeditechEntities();

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Session["LogeUser"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Login");
        }

        //POST
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection loginAccount)
        {
            
            string b = loginAccount["Username"];
            string c = loginAccount["Password"];
            c = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(c)).Select(s => s.ToString("x2")));
            var v = _meditech.USERS.Where(a => a.USERNAME == b && a.PASSWORD == c).Single();
            if (v != null)
            {
                Session["LogeUser"] = v.USERNAME;
                return RedirectToAction("Index","Home");
            }
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session.Remove("LogeUser");
            return View("Login");
        }

        [AllowAnonymous]
        public ActionResult Signup()
        {
            if (Session["LogeUser"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        //POST
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signup(FormCollection signupAccount)
        {
            
            string c = signupAccount["Username"];
            var v = _meditech.USERS.Where(a => a.USERNAME.Equals(c)).FirstOrDefault();
            if (v == null)
            {
                
                USERS ac = new USERS();
                ac.USERNAME = signupAccount["Username"];
                ac.PASSWORD = c = GetMD5HashData(signupAccount["Password"]);
                _meditech.USERS.Add(ac);
                _meditech.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            return View("Signup");
        }
        [AllowAnonymous]
        public ActionResult Profile()
        {
            if (Session["LogeUser"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var c = Session["LogeUser"];
            var dbUser = _meditech.USERS.Where(a => a.USERNAME.Equals(c.ToString())).FirstOrDefault();
            ViewData["userdetail"] = dbUser;

            return View("Profile");
        }

        [AllowAnonymous]
        public ActionResult Edit()
        {
            if (Session["LogeUser"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var c = Session["LogeUser"];
            var dbUser = _meditech.USERS.Where(a => a.USERNAME.Equals(c.ToString())).FirstOrDefault();
            ViewData["infAccout"] = dbUser;
            return View("Edit");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FormCollection fc)
        {
            if (Session["LogeUser"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var c = Session["LogeUser"];
            USERS v = _meditech.USERS.Single(a => a.USERNAME.Equals(c.ToString()));
            _meditech.SaveChanges();
            return RedirectToAction("Edit", "Account");
        }
        private string GetMD5HashData(string data)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();

        }

        

       
    }
}
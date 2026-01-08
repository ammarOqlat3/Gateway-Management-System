using GetWayPro.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;




namespace GetWayPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _context;
        public HomeController(MyDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }









        public IActionResult login()
        {
            return View();
        }


        public IActionResult regster()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult regster(User users, string repeatpass)
        {
            // ÊÍÞÞ ÅÐÇ ßÇäÊ ßáãÇÊ ÇáãÑæÑ ãÊØÇÈÞÉ
            if (users.Password != repeatpass)
            {
                ModelState.AddModelError(string.Empty, "Password and Repeat Password do not match.");
                TempData["LoginResult"] = "Failed";

                return View(users);
            }

            else if (ModelState.IsValid)
            {

                _context.Users.Add(users);
                _context.SaveChanges();
                TempData["LoginSuccess"] = "login Success";

                return RedirectToAction(nameof(login));
            }

            return View(users);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                HttpContext.Session.SetString("LoginResult", "Failed");
                return View();
            }

            // ÇáÈÍË Úä ÇáãÓÊÎÏã Ýí ÞÇÚÏÉ ÇáÈíÇäÇÊ
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("LoginSuccess", "login Success");

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetString("UserEmail", user.Email);

                return RedirectToAction("Index", "Home"); // ÅÚÇÏÉ ÇáÊæÌíå ááÕÝÍÉ ÇáÑÆíÓíÉ
            }
            else
            {
                // ÈíÇäÇÊ ÛíÑ ÕÍíÍÉ
                TempData["LoginResult"] = "Failed";
                return View();
            }
        }



        [HttpGet]
        [HttpGet]
        public IActionResult shop(string category)
        {
            // ÍÝÙ ÇáÝÆÉ ÇáãÎÊÇÑÉ ÏÇÎá ÇáÜ Session
            HttpContext.Session.SetString("SelectedCategory", category ?? "");

            // ÌáÈ ÇáãäÊÌÇÊ ÈäÇÁð Úáì ÇáÝÆÉ ÇáãÎÊÇÑÉ
            var products = string.IsNullOrEmpty(category)
                ? _context.AddProduct.ToList()
                : _context.AddProduct.Where(p => p.Category == category).ToList();

            return View(products);
        }

    } 
}

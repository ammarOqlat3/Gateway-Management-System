using GetWayPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace GetWayPro.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyDbContext _context;
        public UsersController(MyDbContext context)
        {
            _context = context;
        }
        // عرض الصفحة
        [HttpGet]
        public IActionResult myProfile()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("login", "Home");
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value;
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // حفظ التعديلات
        [HttpPost]
        [HttpPost]
        public IActionResult myProfile(User updatedUser)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("login", "Home");
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value;
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                return NotFound();
            }

            // تحديث البيانات
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Address = updatedUser.Address;
            user.Education = updatedUser.Education;
            user.Country = updatedUser.Country;

            _context.SaveChanges();

            // بدل TempData استخدم Session لإعلام الواجهة أنه تم الحفظ
            HttpContext.Session.SetString("ProfileSaved", "true");

            return RedirectToAction("myProfile");
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (sessionUserId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int userId = sessionUserId.Value;
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                return NotFound();
            }

            // تحقق من الباس القديم
            if (user.Password != currentPassword)
            {
                ViewBag.Message = "Current password is incorrect.";
                return View(); // ابقاء بنفس الصفحة
            }

            // تحقق من الباس الجديد وتأكيده
            if (newPassword != confirmPassword)
            {
                ViewBag.Message = "New password and confirmation do not match.";
                return View(); // ابقاء بنفس الصفحة
            }

            // تغيير كلمة المرور وحفظ التغييرات
            user.Password = newPassword;
            _context.SaveChanges();

            // إرسال رسالة نجاح عبر Session ليتم عرضها في صفحة البروفايل
            HttpContext.Session.SetString("PasswordChangedSuccess", "Password updated successfully!");

            return RedirectToAction("myProfile");
        }


        [HttpGet]
        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPost(Post post, IFormFile? ImageFile)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("login", "Home");
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value;

            // ربط المنشور بالمستخدم الحالي
            post.UserId = userId;

            // حفظ الصورة في المجلد وتحويل المسار إلى ImgPath
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{ImageFile.FileName}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                post.ImgPath = "/uploads/" + fileName; // المسار المستخدم في العرض
            }

            _context.Posts.Add(post);
            _context.SaveChanges();

            HttpContext.Session.SetString("PostAdded", "true");
            return RedirectToAction("AddPost");
        }



        //public IActionResult myPosts()
        //{
        //    return View();
        //}

        public IActionResult MyPosts()
        {
            // تأكد إذا المستخدم مسجل دخول
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("login", "Home");
            }

            // جلب البوستات الخاصة بالمستخدم فقط
            var posts = _context.Posts
                                .Where(p => p.UserId == userId)
                                .OrderByDescending(p => p.Id)
                                .ToList();

            return View(posts); // عرض البوستات في الـ View
        }

        public IActionResult Logout()
        {

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");


        }

        [HttpGet]
        [HttpGet]
        public IActionResult AddProduct()
        {
            

            return View();
        }







        [HttpPost]
        public IActionResult AddProduct(AddProduct product, IFormFile ImageFile)
        {

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("login", "Home");
            }
            int userId = HttpContext.Session.GetInt32("UserId").Value;
            product.UserId = userId;


            if (!ModelState.IsValid)
            {
                return View(product);
            }

            // رفع الصورة
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgProdact", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                     ImageFile.CopyTo(stream);
                }

                product.ImgPath = "/imgProdact/" + fileName;
            }

        

            _context.AddProducts.Add(product);
             _context.SaveChangesAsync();

            // رسالة تأكيد باستخدام الجلسة
            HttpContext.Session.SetString("ProductAdded", "true");
            //HttpContext.Session.Remove("Products");


            return RedirectToAction("AddProduct");
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.AddProducts.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.AddProducts.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction("AddProduct");
        }




    }
}













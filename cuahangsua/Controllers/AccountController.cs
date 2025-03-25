using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using cuahangsua.Data;
using System.Linq;
using cuahangsua.Models;

namespace cuahangsua.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Đăng nhập người dùng thường
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
            if (user == null)
            {
                TempData["Error"] = "Email hoặc mật khẩu không đúng!";
                return View();
            }

            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

            TempData["Success"] = "Đăng nhập thành công!";
            return RedirectToAction("Index", "Home");
        }

        // Đăng nhập admin
        public IActionResult AdminLogin() => View();

        [HttpPost]
        public IActionResult AdminLogin(string email, string password)
        {
            var admin = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password && u.IsAdmin);
            if (admin == null)
            {
                TempData["Error"] = "Email hoặc mật khẩu không đúng hoặc không phải admin!";
                return View();
            }

            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("UserName", admin.FullName);
            HttpContext.Session.SetString("IsAdmin", "True");

            TempData["Success"] = "Đăng nhập admin thành công!";
            return RedirectToAction("Dashboard", "Admin");
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string fullName, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                TempData["Error"] = "Mật khẩu xác nhận không khớp!";
                return View();
            }

            var existingUser = _context.Users.Any(u => u.Email == email);
            if (existingUser)
            {
                TempData["Error"] = "Email đã được sử dụng!";
                return View();
            }

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = password, // Lưu trực tiếp mật khẩu, không mã hóa
                IsAdmin = false
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("UserName", fullName);

            TempData["Success"] = "Đăng ký thành công!";
            // Chuyển hướng đến trang chủ thay vì trang đăng nhập
            return RedirectToAction("Index", "Home");
        }

        // Đăng xuất
        public IActionResult Logout()
        {
            // Xóa toàn bộ session
            HttpContext.Session.Clear();

            // Xóa cookie đăng nhập (nếu có)
            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            Response.Cookies.Delete(".AspNetCore.Session");

            // Chuyển hướng về trang chủ thay vì trang đăng nhập
            return RedirectToAction("Index", "Home");
        }
    }
}
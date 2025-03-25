using Microsoft.AspNetCore.Mvc;
using cuahangsua.Data;
using cuahangsua.Models;
using System.Linq;

namespace cuahangsua.Controllers
{
   
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị trang đăng nhập admin (GET)
        public IActionResult Login()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";

            // Nếu đã đăng nhập và là admin, chuyển thẳng đến Dashboard
            if (!string.IsNullOrEmpty(userEmail) && isAdmin)
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            return View();
        }

        // Xử lý đăng nhập admin (POST từ modal)
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var admin = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password && u.IsAdmin);

            if (admin == null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
                if (user != null && !user.IsAdmin)
                {
                    TempData["Error"] = "Tài khoản này không phải admin!";
                }
                else
                {
                    TempData["Error"] = "Sai email hoặc mật khẩu!";
                }
                return RedirectToAction("Index", "Home"); // Nếu sai thông tin, về trang chủ
            }

            // Lưu thông tin vào session
            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("UserName", admin.FullName);
            HttpContext.Session.SetString("IsAdmin", "True");

           

            // Chuyển hướng đến Dashboard thay vì Login
            return RedirectToAction("Dashboard", "Admin");
        }

        // Trang Dashboard (Chỉ admin truy cập được)
        public IActionResult Dashboard()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";

            // Nếu chưa đăng nhập hoặc không phải admin, chuyển về trang chủ
            if (string.IsNullOrEmpty(userEmail) || !isAdmin)
            {
                TempData["Error"] = "Vui lòng đăng nhập với tài khoản admin!";
                return RedirectToAction("Index", "Home"); // Quay về trang chủ thay vì trang Login
            }

            ViewData["Users"] = _context.Users.ToList();
            ViewData["Products"] = _context.Products.ToList();
            return View();
        }

        // Quản lý sản phẩm
        public IActionResult Products()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.Name) || product.Price <= 0)
                return Json(new { success = false, message = "Tên sản phẩm và giá phải hợp lệ!" });

            if (string.IsNullOrEmpty(product.ImageUrl))
                product.ImageUrl = "/images/default.png";

            if (string.IsNullOrEmpty(product.Category))
                product.Category = "sua";

            _context.Products.Add(product);
            _context.SaveChanges();
            return Json(new { success = true, message = "Thêm sản phẩm thành công!" });
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            var existingProduct = _context.Products.Find(product.Id);
            if (existingProduct == null)
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });

            if (string.IsNullOrEmpty(product.Name) || product.Price <= 0)
                return Json(new { success = false, message = "Tên sản phẩm và giá phải hợp lệ!" });

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.ImageUrl = product.ImageUrl ?? existingProduct.ImageUrl;
            existingProduct.Category = product.Category ?? existingProduct.Category;
            _context.SaveChanges();
            return Json(new { success = true, message = "Cập nhật sản phẩm thành công!" });
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });

            _context.Products.Remove(product);
            _context.SaveChanges();
            return Json(new { success = true, message = "Xóa sản phẩm thành công!" });
        }

        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
            return Json(new { success = true, product });
        }

        // Quản lý người dùng
        public IActionResult Users()
        {
            return View(_context.Users.ToList());
        }

        [HttpPost]
        public IActionResult AddUser(string fullName, string email, string password)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email))
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ họ tên và email!" });

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
                return Json(new { success = false, message = "Email đã tồn tại!" });

            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = password
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return Json(new { success = true, message = "Thêm người dùng thành công!" });
        }

        [HttpPost]
        public IActionResult UpdateUser(User user)
        {
            var existingUser = _context.Users.Find(user.Id);
            if (existingUser == null)
                return Json(new { success = false, message = "Người dùng không tồn tại!" });

            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            if (!string.IsNullOrEmpty(user.PasswordHash))
                existingUser.PasswordHash = user.PasswordHash;
            _context.SaveChanges();
            return Json(new { success = true, message = "Cập nhật người dùng thành công!" });
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return Json(new { success = false, message = "Người dùng không tồn tại!" });

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Json(new { success = true, message = "Xóa người dùng thành công!" });
        }

        // Quản lý đơn hàng
        public IActionResult DonHang()
        {
            var donHangs = _context.donhangs.ToList();
            return View(donHangs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDonHang(donhang donHang)
        {
            if (donHang == null)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            // Kiểm tra dữ liệu cơ bản
            if (donHang.Quantity <= 0 || donHang.Price <= 0)
            {
                return Json(new { success = false, message = "Số lượng và giá phải lớn hơn 0!" });
            }

            try
            {
                _context.donhangs.Add(donHang);
                _context.SaveChanges();
                return Json(new { success = true, message = "Thêm đơn hàng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi khi lưu dữ liệu: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDonHang(donhang donHang)
        {
            if (donHang == null)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            var existingOrder = _context.donhangs.Find(donHang.Id);
            if (existingOrder == null)
            {
                return Json(new { success = false, message = "Đơn hàng không tồn tại!" });
            }

            if (donHang.Quantity <= 0 || donHang.Price <= 0)
            {
                return Json(new { success = false, message = "Số lượng và giá phải lớn hơn 0!" });
            }

            try
            {
                existingOrder.ProductId = donHang.ProductId;
                existingOrder.ProductName = donHang.ProductName;
                existingOrder.Quantity = donHang.Quantity;
                existingOrder.Price = donHang.Price;
                _context.SaveChanges();
                return Json(new { success = true, message = "Cập nhật đơn hàng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi khi cập nhật dữ liệu: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDonHang(int id)
        {
            var donHang = _context.donhangs.Find(id);
            if (donHang == null)
            {
                return Json(new { success = false, message = "Đơn hàng không tồn tại!" });
            }

            try
            {
                _context.donhangs.Remove(donHang);
                _context.SaveChanges();
                return Json(new { success = true, message = "Xóa đơn hàng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi khi xóa dữ liệu: " + ex.Message });
            }
        }
    }
}
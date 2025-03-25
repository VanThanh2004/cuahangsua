using Microsoft.AspNetCore.Mvc;
using cuahangsua.Data; // Import DbContext
using cuahangsua.Models;
using System.Linq;

namespace cuahangsua.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang chủ hiển thị danh sách tất cả sản phẩm từ Database
        public IActionResult Index()
        {
            ViewData["Title"] = "Cửa hàng Mẹ & Bé";
            var products = _context.Products.ToList(); // Lấy dữ liệu từ SQL
            return View(products);
        }

        // Lọc sản phẩm theo danh mục & tìm kiếm
        public IActionResult GetProducts(string category, string search)
        {
            var filteredProducts = _context.Products.AsQueryable(); // Truy vấn từ SQL

            // Lọc theo danh mục (nếu có)
            if (!string.IsNullOrEmpty(category) && category != "all")
            {
                filteredProducts = filteredProducts.Where(p => p.Category == category);
            }

            // Tìm kiếm sản phẩm theo tên
            if (!string.IsNullOrEmpty(search))
            {
                filteredProducts = filteredProducts.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            }

            return PartialView("_ProductList", filteredProducts.ToList());
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ProductControl.Controllers
{
    public class ProductController : Controller
    {
        private static List<Product> _products = new List<Product>
        {
           new Product { Id = 1, Name = "Sữa Friso Gold", Description = "Sữa cho trẻ 0-6 tháng", Price = 450000, ImageUrl = "/images/friso.jpg", Category = "sua" },
                 new Product { Id = 2, Name = "Sữa Enfa A+", Description = "Sữa Giúp Bé Thông Minh", Price = 480000, ImageUrl = "/images/enfa.png", Category = "sua" },
                 new Product { Id = 3, Name = "Sữa Nan Optipro",Description = "Sữa cho bé phát triển toàn diện", Price = 380000,ImageUrl= "/images/nan.jpg", Category = "sua" },
                 new Product { Id = 4, Name = "Sữa Meiji",Description = "Sữa Nhật giúp bé tăng cân khoẻ mạnh", Price = 420000,ImageUrl= "/images/meiji.jpg", Category = "sua" },
                 new Product { Id = 5, Name = "Bộ quần áo bé gái",Description = "Bộ quần áo cotton mềm mại cho bé", Price = 150000, ImageUrl = "/images/quanao.png", Category = "clothes" },
                 new Product { Id = 6, Name = "Váy bé gái",Description = "Váy dễ thương cho bé gái", Price = 180000, ImageUrl = "/images/quanao2.jpg", Category = "clothes" },
                 new Product { Id = 7, Name = "Bộ quần áo sơ sinh",Description = "Bộ quần áo cotton mềm mại cho bé sơ sinh", Price = 150000, ImageUrl = "/images/quanao1.jpg", Category = "clothes" },
                 new Product { Id = 8, Name = "Áo thun bé trai",Description = "Áo thun chất liệu thoáng mát cho bé trai", Price = 120000, ImageUrl = "/images/quanao3.jpg", Category = "clothes" },
                 new Product { Id = 9, Name = "Bộ đồ chơi xếp hình",Description = "Bộ xếp hình giúp phát triển tư duy sáng tạo", Price = 300000, ImageUrl = "/images/toy_blocks.jpg" },
                 new Product { Id = 10, Name = "Gấu bông dễ thương",Description = "Gấu bông dễ thương mềm cho bé", Price = 200000, ImageUrl = "/images/teddy.jpg" },
                 new Product { Id = 11, Name = "Xe điều khiển từ xa",Description = "Xe hơi điều khiển từ xa tốc độ cao", Price = 350000, ImageUrl = "/images/dochoi1.jpg" },
                 new Product { Id = 12, Name = "Búp bê Barbie",Description = "Búp bê Barbie thời trang cho bé gái", Price = 220000, ImageUrl = "/images/dochoi2.jpg" },
                 new Product { Id = 13, Name = "Bỉm trẻ em", Description = "Bỉm mềm mại thấm hút tốt", Price = 250000, ImageUrl = "/images/bim.jpg", Category = "bim" },
                 new Product { Id = 14, Name = "Bỉm BobBy", Description = "Bỉm Bobby thấm hút nhanh, khô thoáng cả ngày", Price = 200000, ImageUrl = "/images/bim.png", Category = "bim" },
                 new Product { Id = 15, Name = "Bỉm Merries", Description = "Bỉm Merries Nhật Bản siêu mềm mịn", Price = 250000, ImageUrl = "/images/bim2.jpg", Category = "bim" },
                 new Product { Id = 16, Name = "Bỉm quần Pampers", Description = "Bỉm thấm hút nhanh, khô thoáng cả ngày", Price = 190000, ImageUrl = "/images/bim3.jpg", Category = "bim" },
        };

        // Trang chi tiết sản phẩm
        public IActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        // Tìm kiếm sản phẩm
        [HttpGet]
        public IActionResult Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return Json(new { success = false, message = "Từ khóa tìm kiếm không hợp lệ." });
            }

            var results = _products
                .Where(p => p.Name.Contains(keyword, System.StringComparison.OrdinalIgnoreCase))
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.ImageUrl
                })
                .ToList();

            return Json(results);
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
    }
}

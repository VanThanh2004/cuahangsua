using Microsoft.AspNetCore.Mvc;
using cuahangsua.Models;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace cuahangsua.Controllers
{
    public class MilkController : Controller
    {
        private readonly List<Product> milkProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Sữa Friso Gold",Description = "Sữa cho trẻ 0-6 tháng", Price = 450000, ImageUrl = "/images/friso.jpg", Category = "sua" },
            new Product { Id = 2, Name = "Sữa Enfa A+",Description = "Sữa Giúp Bé Thông Minh", Price = 250000, ImageUrl = "/images/enfa.png", Category = "sua" },
            new Product { Id = 3, Name = "Sữa Nan Optipro",Description = "Sữa cho bé phát triển toàn diện", Price = 380000,ImageUrl= "/images/nan.jpg", Category = "sua" },
            new Product { Id = 4, Name = "Sữa Meiji",Description = "Sữa Nhật giúp bé tăng cân khoẻ mạnh", Price = 420000,ImageUrl= "/images/meiji.jpg", Category = "sua" },
        };

        public IActionResult Index()
        {
            ViewData["Title"] = "Sản phẩm Sữa";
            return View(milkProducts);
        }

        public IActionResult Details(int id)
        {
            var product = milkProducts.Find(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}


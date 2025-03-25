using cuahangsua.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace cuahangsua.Controllers
{
    public class ToysController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<Product>
            {
                new Product { Id = 9, Name = "Bộ đồ chơi xếp hình",Description = "Bộ xếp hình giúp phát triển tư duy sáng tạo", Price = 300000, ImageUrl = "/images/toy_blocks.jpg" },
                new Product { Id = 10, Name = "Gấu bông dễ thương",Description = "Gấu bông dễ thương mềm cho bé", Price = 200000, ImageUrl = "/images/teddy.jpg" },
                new Product { Id = 11, Name = "Xe điều khiển từ xa",Description = "Xe hơi điều khiển từ xa tốc độ cao", Price = 350000, ImageUrl = "/images/dochoi1.jpg" },
                new Product { Id = 12, Name = "Búp bê Barbie",Description = "Búp bê Barbie thời trang cho bé gái", Price = 220000, ImageUrl = "/images/dochoi2.jpg" },
            };

            return View(products);
        }
    }
}

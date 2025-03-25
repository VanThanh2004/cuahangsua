using Microsoft.AspNetCore.Mvc;
using cuahangsua.Models;
using System.Collections.Generic;

namespace cuahangsua.Controllers
{
    public class ClothesController : Controller
    {
        private readonly List<Product> clothesProducts = new List<Product>
        {
            new Product { Id = 5, Name = "Bộ quần áo bé gái",Description = "Bộ quần áo cotton mềm mại cho bé", Price = 150000, ImageUrl = "/images/quanao.png", Category = "clothes" },
            new Product { Id = 6, Name = "Váy bé gái",Description = "Váy dễ thương cho bé gái", Price = 180000, ImageUrl = "/images/quanao2.jpg", Category = "clothes" },
            new Product { Id = 7, Name = "Bộ quần áo sơ sinh",Description = "Bộ quần áo cotton mềm mại cho bé sơ sinh", Price = 150000, ImageUrl = "/images/quanao1.jpg", Category = "clothes" },
            new Product { Id = 8, Name = "Áo thun bé trai",Description = "Áo thun chất liệu thoáng mát cho bé trai", Price = 120000, ImageUrl = "/images/quanao3.jpg", Category = "clothes" },
        };

        public IActionResult Index()
        {
            ViewData["Title"] = "Quần Áo Trẻ Em";
            return View(clothesProducts);
        }

        public IActionResult Details(int id)
        {
            var product = clothesProducts.Find(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
using cuahangsua.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using cuahangsua.Data;

namespace cuahangsua.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Thêm DbContext

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // Gán context vào bi?n _context
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetProducts(string search)
        {
            if (string.IsNullOrEmpty(search))
                return Json(new List<object>());

            var products = _context.Products
                .Where(p => p.Name.Contains(search))
                .Select(p => new { p.Id, p.Name, p.ImageUrl })
                .Take(5) // Gi?i h?n 5 s?n ph?m g?i ý
                .ToList();

            return Json(products);
        }

        public IActionResult Search(string query)
        {
            var products = string.IsNullOrEmpty(query)
                ? new List<Product>()
                : _context.Products.Where(p => p.Name.Contains(query)).ToList();

            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

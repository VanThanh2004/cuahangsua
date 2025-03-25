using Microsoft.AspNetCore.Mvc;
using cuahangsua.Models; // Thay thế bằng namespace thực tế
using System.Collections.Generic;


public class BimController : Controller
{
    public IActionResult Index()
    {
        var products = new List<Product>
        {
            new Product { Id = 13, Name = "Bỉm trẻ em", Description = "Bỉm mềm mại thấm hút tốt", Price = 250000, ImageUrl = "/images/bim.jpg", Category = "bim" },
            new Product { Id = 14, Name = "Bỉm BobBy", Description = "Bỉm Bobby thấm hút nhanh, khô thoáng cả ngày", Price = 200000, ImageUrl = "/images/bim.png", Category = "bim" },
            new Product { Id = 15, Name = "Bỉm Merries", Description = "Bỉm Merries Nhật Bản siêu mềm mịn", Price = 250000, ImageUrl = "/images/bim2.jpg", Category = "bim" },
            new Product { Id = 16, Name = "Bỉm quần Pampers", Description = "Bỉm thấm hút nhanh, khô thoáng cả ngày", Price = 190000, ImageUrl = "/images/bim3.jpg", Category = "bim" },
        };

        return View(products);
    }
}

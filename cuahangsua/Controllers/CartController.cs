using Microsoft.AspNetCore.Mvc;
using cuahangsua.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using cuahangsua.Data;

namespace cuahangsua.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "CartSession";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            return cartJson != null ? JsonConvert.DeserializeObject<List<CartItem>>(cartJson) : new List<CartItem>();
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));
        }

        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var userName = HttpContext.Session.GetString("UserName");

            if (!string.IsNullOrEmpty(userName))
            {
                ViewBag.UserName = userName;
            }
            else
            {
                ViewBag.UserName = "Khách";
            }

            ViewBag.TotalPrice = cart.Sum(item => item.Product.Price * item.Quantity);
            return View(cart);
        }

        [HttpPost]
        public IActionResult Add(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id); // Lấy từ DB
            if (product != null)
            {
                var cart = GetCartFromSession();
                var cartItem = cart.FirstOrDefault(c => c.Product.Id == id);

                var userEmail = HttpContext.Session.GetString("UserEmail");
                var userName = HttpContext.Session.GetString("UserName");

                if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userName))
                {
                    return Json(new { success = false, message = "Vui lòng đăng ký để thêm sản phẩm vào giỏ hàng!" });
                }

                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem
                    {
                        Product = product,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl,
                        Quantity = 1,
                        UserName = userName,
                        UserEmail = userEmail
                    });
                }

                SaveCartToSession(cart);

                int totalQuantity = cart.Sum(item => item.Quantity);
                return Json(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng!", cartCount = totalQuantity });
            }

            return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
        }

        [HttpGet]
        public JsonResult GetCartCount()
        {
            var cart = GetCartFromSession();
            int totalQuantity = cart.Sum(item => item.Quantity);
            return Json(new { count = totalQuantity });
        }

        [HttpPost]
        public JsonResult UpdateQuantityAjax(int id, int quantity)
        {
            var cart = GetCartFromSession();
            var cartItem = cart.FirstOrDefault(c => c.Product.Id == id);

            if (cartItem != null)
            {
                if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                }
                else
                {
                    cart.Remove(cartItem);
                }

                SaveCartToSession(cart);
            }
            else
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng!" });
            }

            var totalPrice = cart.Sum(item => item.Product.Price * item.Quantity);
            var itemTotal = cartItem != null ? cartItem.Product.Price * cartItem.Quantity : 0;

            return Json(new { success = true, totalPrice = totalPrice.ToString("N0"), itemTotal = itemTotal.ToString("N0") });
        }

        [HttpPost]
        public JsonResult RemoveAjax(int id)
        {
            var cart = GetCartFromSession();
            var cartItem = cart.FirstOrDefault(c => c.Product.Id == id);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCartToSession(cart);
            }
            else
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng!" });
            }

            var totalPrice = cart.Sum(item => item.Product.Price * item.Quantity);
            return Json(new { success = true, totalPrice = totalPrice.ToString("N0") });
        }

        [HttpPost]
        public JsonResult CheckoutAjax()
        {
            try
            {
                var cart = GetCartFromSession();
                if (cart == null || !cart.Any())
                {
                    return Json(new { success = false, message = "Giỏ hàng trống!" });
                }

                // Lưu tất cả đơn hàng vào cơ sở dữ liệu
                foreach (var item in cart)
                {
                    var order = new donhang
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    };
                    _context.donhangs.Add(order);
                }

                _context.SaveChanges();

                // Xóa giỏ hàng sau khi thanh toán
                HttpContext.Session.Remove(CartSessionKey);

                return Json(new { success = true, message = "🛍 Thanh Toán Thành Công! Cảm Ơn Quý Khách Đã Ghé Thăm Shop Em! 🎉" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi thanh toán: " + ex.Message });
            }
        }
    }
}
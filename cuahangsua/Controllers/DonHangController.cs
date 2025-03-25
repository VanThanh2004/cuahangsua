using Microsoft.AspNetCore.Mvc;
using cuahangsua.Models;
using cuahangsua.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace cuahangsua.Controllers
{
    public class DonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách đơn hàng
        public IActionResult Index()
        {
            var donHangs = _context.donhangs.ToList();
            return View(donHangs);
        }

        // Hiển thị form thêm đơn hàng
        public IActionResult Create()
        {
            return View();
        }

        // Thêm đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(donhang donHang)
        {
            if (ModelState.IsValid)
            {
                _context.donhangs.Add(donHang);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(donHang);
        }

        // Hiển thị form sửa đơn hàng
        public IActionResult Edit(int id)
        {
            var donHang = _context.donhangs.Find(id);
            if (donHang == null)
            {
                return NotFound();
            }
            return View(donHang);
        }

        // Sửa đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, donhang donHang)
        {
            if (id != donHang.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.donhangs.Update(donHang);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.donhangs.Any(d => d.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(donHang);
        }

        // Xóa đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var donHang = _context.donhangs.Find(id);
            if (donHang != null)
            {
                _context.donhangs.Remove(donHang);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

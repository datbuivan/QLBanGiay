using Microsoft.AspNetCore.Mvc;
using Nhom3_QLBanGiay.Models;
using System.Data.Entity;
using System.Diagnostics;
using X.PagedList;

namespace Nhom3_QLBanGiay.Controllers
{
    public class HomeController : Controller
    {
        QlbanGiayContext db = new QlbanGiayContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int? page)
        {
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            int pageSize = 8;
            var lstSanpham = db.SanPhams.AsNoTracking().OrderBy(x => x.MaSanPham);
            PagedList<SanPham> lst = new PagedList<SanPham>(lstSanpham, pageNumber, pageSize);
            return View(lst);

        }
        public IActionResult ChiTietSanPham(String maSp)
        {
            var anhSanPham = db.HinhAnhSps.Where(x => x.MaSanPham == maSp).ToList();
            ViewBag.anhSanPham = anhSanPham;
            var sanpham = db.SanPhams.SingleOrDefault(x => x.MaSanPham == maSp);
            if (sanpham == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(sanpham);
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using Nhom3_QLBanGiay.Models;
using Nhom3_QLBanGiay.Models.ProductModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Nhom3_QLBanGiay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsAPIController : ControllerBase
    {
        QlbanGiayContext db = new QlbanGiayContext();
        [HttpGet]
        public IEnumerable<Product> getProductsByCategory(string? maLoaiSp, string? maDTMh)
        {
            IList<Product> products = new List<Product>();

            if (!String.IsNullOrEmpty(maLoaiSp) && !String.IsNullOrEmpty(maDTMh))
            {
                // Both maLoaiSp and maDTMh parameters are provided
                var sanPhams = db.SanPhams.Where(x => x.MaLoaiSp == maLoaiSp && x.MaDoiTuongMh == maDTMh).ToList();
                foreach (var s in sanPhams)
                {
                    products.Add(new Product
                    {
                        MaSanPham = s.MaSanPham,
                        TenSanPham = s.TenSanPham,
                        GiaBan = s.GiaBan,
                        HinhAnhAvatar = s.HinhAnhAvatar,
                        MaLoaiSp = s.MaLoaiSp,
                        MaDoiTuongMh = s.MaDoiTuongMh,
                    });
                }
            }
            else if (!String.IsNullOrEmpty(maLoaiSp) && String.IsNullOrEmpty(maDTMh))
            {
                // Only maLoaiSp parameter is provided
                var sanPhams = db.SanPhams.Where(x => x.MaLoaiSp == maLoaiSp).ToList();
                foreach (var s in sanPhams)
                {
                    products.Add(new Product
                    {
                        MaSanPham = s.MaSanPham,
                        TenSanPham = s.TenSanPham,
                        GiaBan = s.GiaBan,
                        HinhAnhAvatar = s.HinhAnhAvatar,
                        MaLoaiSp = s.MaLoaiSp,
                        MaDoiTuongMh = s.MaDoiTuongMh,
                    });
                }
            }
            else if (String.IsNullOrEmpty(maLoaiSp) && !String.IsNullOrEmpty(maDTMh))
            {
                var sanPhams = db.SanPhams.Where(x => x.MaDoiTuongMh == maDTMh).ToList();
                foreach (var s in sanPhams)
                {
                    products.Add(new Product
                    {
                        MaSanPham = s.MaSanPham,
                        TenSanPham = s.TenSanPham,
                        GiaBan = s.GiaBan,
                        HinhAnhAvatar = s.HinhAnhAvatar,
                        MaLoaiSp = s.MaLoaiSp,
                        MaDoiTuongMh = s.MaDoiTuongMh,
                    });
                }
            }
            else if (String.IsNullOrEmpty(maLoaiSp) && String.IsNullOrEmpty(maDTMh))
            {
                var sanPhams = db.SanPhams.ToList();
                foreach (var s in sanPhams)
                {
                    products.Add(new Product
                    {
                        MaSanPham = s.MaSanPham,
                        TenSanPham = s.TenSanPham,
                        GiaBan = s.GiaBan,
                        HinhAnhAvatar = s.HinhAnhAvatar,
                        MaLoaiSp = s.MaLoaiSp,
                        MaDoiTuongMh = s.MaDoiTuongMh,
                    });
                }
            }

            return products;
        }

        [HttpDelete]
        [Route("sanpham/{id}")]
        public IActionResult DeleteSanPham([FromRoute] String id)
        {
            var sanPham = db.SanPhams.SingleOrDefault(x => x.MaSanPham == id);
            if (sanPham == null)
            {
                return NotFound();
            }
            else
            {
                bool exit1 = db.ChiTietHoaDonBans.Any(x => x.MaSanPham == id);
                bool exit2 = db.ChiTietHoaDonNhaps.Any(x => x.MaSanPham == id);
                if (exit1 || exit2)
                {
                    return NotFound();

                }
                List<ChiTietSanPham> ctsp = db.ChiTietSanPhams.Where(x => x.MaSanPham == id).ToList();
                List<HinhAnhSp> hasp = db.HinhAnhSps.Where(x => x.MaSanPham == id).ToList();
                if (ctsp != null && ctsp.Any())
                {
                    db.ChiTietSanPhams.RemoveRange(ctsp);
                    db.SaveChanges();
                }
                if (hasp != null && hasp.Any())
                {
                    db.HinhAnhSps.RemoveRange(hasp);
                    db.SaveChanges();
                }
                db.SanPhams.Remove(sanPham);
                db.SaveChanges();
                return new StatusCodeResult(200);
            }
        }
    }
}

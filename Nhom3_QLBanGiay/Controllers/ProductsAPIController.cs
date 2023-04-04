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
        public IEnumerable<Product> getAllProducts()
        {
            var sanPham = (from p in db.SanPhams
                           select new Product
                           {
                               MaSanPham=p.MaSanPham,
                               TenSanPham= p.TenSanPham,
                               GiaBan=p.GiaBan,
                               HinhAnhAvatar=p.HinhAnhAvatar,
                               MaLoaiSp =p.MaLoaiSp
                           }).ToList();

            return sanPham;
        }
        [HttpGet("{maLoaiSp}")]
        public IEnumerable<Product> getProductsByCategory(string maLoaiSp)
        {
            IList<Product> products = new List<Product>();
            var sanPhams = db.SanPhams.Where(x => x.MaLoaiSp == maLoaiSp).ToList();
            foreach(var s in sanPhams)
            {
                products.Add(new Product
                {
                    MaSanPham = s.MaSanPham,
                    TenSanPham = s.TenSanPham,
                    GiaBan = s.GiaBan,
                    HinhAnhAvatar = s.HinhAnhAvatar,
                    MaLoaiSp = s.MaLoaiSp
                });
                
            }
            return products;
        }
    }
}

using Azure;
using Bai2.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Nhom3_QLBanGiay.Models;
using System.Data.Entity;
using System.Diagnostics;
using X.PagedList;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

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

    
        public IActionResult Index(int? page ,string search)
        {
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            int pageSize = 8;
            var lstSanpham = from sp in db.SanPhams select sp;
            if (!String.IsNullOrEmpty(search))
            {
                lstSanpham = lstSanpham.Where(x => x.TenSanPham.Contains(search));
            }
            PagedList<SanPham> lst = new PagedList<SanPham>(lstSanpham, pageNumber, pageSize);
            ViewBag.page = lst.PageCount;
            return View(lst);

        }
   
        public IActionResult ChiTietSanPham(String maSp)
        {
            var anhSanPham = db.HinhAnhSps.Where(x => x.MaSanPham == maSp).ToList();
            ViewBag.anhSanPham = anhSanPham;
            List<String> sizeSP = (from ctsp in db.ChiTietSanPhams
                          join kt in db.KichThuocs on ctsp.MaKichThuoc equals kt.MaKichThuoc
                          where ctsp.MaSanPham == maSp
                          select kt.TenKichThuoc
                          ).Distinct().ToList();
            ViewData["SizeSP"]=sizeSP;
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
 
        public IActionResult Shop(int? page,string search)
        {
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            int pageSize = 6;
            var lstSanpham = from sp in db.SanPhams select sp;
            if (!String.IsNullOrEmpty(search))
            {
                lstSanpham = lstSanpham.Where(x => x.TenSanPham.Contains(search));
            }
            PagedList<SanPham> lst = new PagedList<SanPham>(lstSanpham, pageNumber, pageSize);
            ViewBag.page = lst.PageCount;
            return View(lst);
        }
        [Authentication]
        public IActionResult SanPhamTheoLoai(string MaLoai, int? page)
        {
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            int pageSize = 6;
            var lstSanpham = db.SanPhams.AsNoTracking().Where(x => x.MaLoaiSp == MaLoai).OrderBy(x => x.TenSanPham);
            PagedList<SanPham> lst = new PagedList<SanPham>(lstSanpham, pageNumber, pageSize);
            ViewBag.MaLoaiSp = MaLoai;
            return View(lst);
        }


        public IActionResult AddByAPI()
        {
            ViewBag.MaSanPham = new SelectList(db.SanPhams, "MaSanPham", "MaSanPham");
            ViewBag.MaLoaiSp = new SelectList(db.LoaiSps, "MaLoaiSp", "TenLoaiSp");
            ViewBag.MaDoiTuongMh = new SelectList(db.DoiTuongMhs, "MaDoiTuongMh", "TenDoiTuongMh");
            return View();
        }

        public IActionResult DeleteByAPI()
        {
            ViewBag.SP = db.SanPhams.ToList();
            return View();
        }

        public IActionResult UpdateByAPI()
        {
            ViewBag.MaSanPham = new SelectList(db.SanPhams, "MaSanPham", "MaSanPham");
            ViewBag.MaLoaiSp = new SelectList(db.LoaiSps, "MaLoaiSp", "TenLoaiSp");
            ViewBag.MaDoiTuongMh = new SelectList(db.DoiTuongMhs, "MaDoiTuongMh", "TenDoiTuongMh");
            return View();
        }


        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
        List<CartItem> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }
        /// Thêm sản phẩm vào cart

        [Authentication]
        public IActionResult AddToCart(string maSanPham)
        {

            var sanpham = db.SanPhams
                .Where(p => p.MaSanPham == maSanPham)
                .FirstOrDefault();
            if (sanpham == null)
                return NotFound("Không có sản phẩm");

            // Xử lý đưa vào Cart ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.sanpham.MaSanPham == maSanPham);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItem() { quantity = 1, sanpham = sanpham });
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            // Chuyển đến trang hiện thị Cart
            return RedirectToAction(nameof(Cart));
        }
        /// xóa item trong cart

        [Authentication]
        public IActionResult RemoveCart(string maSanPham)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.sanpham.MaSanPham == maSanPham);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }
        /// Cập nhật
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        [Authentication]
        public IActionResult UpdateCart(string maSanPham, int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.sanpham.MaSanPham == maSanPham);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;
            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }
        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            return View(GetCartItems());
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



        public IActionResult Vote(SanPham sp)
        {
            if (sp != null)
            {
                SanPham sanp = db.SanPhams.SingleOrDefault(x => x.MaSanPham == sp.MaSanPham);
                double vote = (double)sanp.Vote;
                int soluotVote = (int)sanp.SoLuotVot;
                if (sp.Vote != null)
                {
                    double voteNew = (vote * soluotVote + double.Parse(sp.Vote.ToString())) / (soluotVote + 1);
                    int soluotVoteNew = soluotVote + 1;
                    sanp.Vote = voteNew;
                    sanp.SoLuotVot = soluotVoteNew;
                    db.Entry(sanp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Shop");
        }
    }
}
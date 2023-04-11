using Azure;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nhom3_QLBanGiay.Areas.Admin.ViewModels;
using Nhom3_QLBanGiay.Models;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using X.PagedList;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace Nhom3_QLBanGiay.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        QlbanGiayContext db = new QlbanGiayContext();

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.priceAll =(from hdb in db.HoaDonBans
                               join cthdb in db.ChiTietHoaDonBans on hdb.MaHoaDonBan equals cthdb.MaHoaDonBan
                               select cthdb.DonGiaBan).Sum();

            ViewBag.countHDB = (from hdb in db.HoaDonBans
                                select hdb).Count();

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            ViewBag.countHDBToDay =(from hdb in db.HoaDonBans
                                    where hdb.NgayBan == currentDate
                                    select hdb).Count();

            ViewBag.countUser = (from u in db.Users
                                 where u.Role == 1
                                 select u).Count();
            return View();
        }

        [Route("danhsachsanpham")]
        public IActionResult DanhSachSanPham(int? page, String? SearchText)
        {
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            int pageSize = 9;

            List<SanPham> lstsanpham = new List<SanPham>();
            if (SearchText != null && SearchText != "")
            {
                int number;
                String st = SearchText.Trim();
                bool isNumeric = int.TryParse(st, out number);
                if (isNumeric)
                {
                    lstsanpham = db.SanPhams.Where(x => x.GiaNhap == int.Parse(st) || x.GiaBan == int.Parse(st)).ToList();
                }
                else
                {
                    lstsanpham = db.SanPhams.AsEnumerable()
                        .Where(x => Convert(x.TenSanPham.ToLower()).Contains(Convert(st).ToLower()) || Convert(x.ChatLieu.ToLower()).Contains(Convert(st).ToLower())).ToList();
                }
            }
            else
            {
                lstsanpham = db.SanPhams.ToList();
            }
            PagedList<SanPham> lst = new PagedList<SanPham>(lstsanpham, pageNumber, pageSize);
            ViewBag.SearchText = SearchText;
            ViewBag.PageCount = lst.PageCount;
            return View(lst);
        }

        [Route("themsanpham")]
        public IActionResult ThemSanPham()
        {
            //ViewBag.MaLoaiSp = new SelectList(db.LoaiSps, "MaLoaiSp", "TenLoaiSp");
            List<LoaiSp> lstlsp = db.LoaiSps.ToList();
            ViewBag.MaLoaiSp = lstlsp;
            List<DoiTuongMh> lstdtmh = db.DoiTuongMhs.ToList();
            ViewBag.MaDoiTuongMh = lstdtmh;
            ViewBag.MaSanPham = new SelectList(db.SanPhams, "MaSanPham", "TenSanPham");
            ViewBag.MaMauSac = new SelectList(db.MauSacs, "MaMauSac", "TenMauSac");
            ViewBag.MaKichThuoc = new SelectList(db.KichThuocs, "MaKichThuoc", "TenKichThuoc");
            int c1 = db.MauSacs.Count();
            int c2 = db.KichThuocs.Count();
            ViewBag.Count = c1 * c2;
            //SanPhamViewModel sp = new SanPhamViewModel();
            return View();
        }

        [Route("themsanpham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPham(SanPhamViewModel sp)
        {
            //if (ModelState.IsValid)
            //{
            bool exists = db.SanPhams.Any(s => s.MaSanPham == sp.SanPham.MaSanPham);
            if (exists)
            {
                ViewBag.Msg = "Sản phẩm đã tồn tại trong cơ sở dữ liệu";
            }
            else
            {

                db.SanPhams.Add(sp.SanPham);
                int c = db.SaveChanges();
                if (c > 0)
                {
                    if (sp.ChiTietSanPham != null && sp.ChiTietSanPham.Any())
                    {
                        foreach (var chiTietSanPham in sp.ChiTietSanPham)
                        {
                            if (chiTietSanPham == null)
                            {
                                sp.ChiTietSanPham.Remove(chiTietSanPham);
                            }
                            chiTietSanPham.MaSanPham = sp.SanPham.MaSanPham;
                        }
                        db.AddRange(sp.ChiTietSanPham);
                        int m = db.SaveChanges();
                        if (m > 0)
                        {
                            return RedirectToAction("DanhSachSanPham");
                        }
                    }
                    else
                    {
                        db.SanPhams.Remove(sp.SanPham);
                        db.SaveChanges();
                        ViewBag.Msg = "Cần nhập đủ sản phẩm và chi tiết sản phẩm";
                    }
                }
            }
            //}
            //else
            //{
            //    var errors = new Dictionary<string, string>();
            //    foreach (var key in ModelState.Keys)
            //    {
            //        foreach (var error in ModelState[key].Errors)
            //        {
            //            errors.Add(key, error.ErrorMessage);
            //        }
            //    }
            //    ViewBag.Errors = errors;
            //}
            List<LoaiSp> lstlsp = db.LoaiSps.ToList();
            ViewBag.MaLoaiSp = lstlsp;
            List<DoiTuongMh> lstdtmh = db.DoiTuongMhs.ToList();
            ViewBag.MaDoiTuongMh = lstdtmh;
            ViewBag.MaSanPham = new SelectList(db.SanPhams, "MaSanPham", "TenSanPham");
            ViewBag.MaMauSac = new SelectList(db.MauSacs, "MaMauSac", "TenMauSac");
            ViewBag.MaKichThuoc = new SelectList(db.KichThuocs, "MaKichThuoc", "TenKichThuoc");
            int c1 = db.MauSacs.Count();
            int c2 = db.KichThuocs.Count();
            ViewBag.Count = c1 * c2;
            return View(sp);
        }

        [Route("themctsanpham")]
        public IActionResult ThemCTSanPham(String masp)
        {
            List<ChiTietSanPham> lstct = db.ChiTietSanPhams.Where(x => x.MaSanPham == masp).ToList();
            ViewBag.CT = lstct;
            SanPham sp = db.SanPhams.SingleOrDefault(x=>x.MaSanPham==masp);
            ViewBag.SP = sp;
            List<MauSac> lstms = db.MauSacs.ToList();
            ViewBag.MS = lstms;
            List<KichThuoc> lstkt = db.KichThuocs.ToList();
            ViewBag.KT = lstkt;
            int c1 = db.MauSacs.Count();
            int c2 = db.KichThuocs.Count();
            ViewBag.Count = c1 * c2;
            return View();

        }

        [Route("themctsanpham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemCTSanPham(CTSPViewModel ctsp)
        {
            if (ctsp.ChiTietSanPham == null || !ctsp.ChiTietSanPham.Any())
            {
                ViewBag.Msg = "Hãy chọn ít nhất một chi tiết sản phẩm!";
            }
            else
            {
                db.ChiTietSanPhams.AddRange(ctsp.ChiTietSanPham);
                int c = db.SaveChanges();
                if (c > 0)
                {
                    return RedirectToAction("DanhSachSanPham");

                }
            }
            List<ChiTietSanPham> lstct = db.ChiTietSanPhams.Where(x => x.MaSanPham == ctsp.MaSanPham).ToList();
            ViewBag.CT = lstct;
            SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSanPham == ctsp.MaSanPham);
            ViewBag.SP = sp;
            List<MauSac> lstms = db.MauSacs.ToList();
            ViewBag.MS = lstms;
            List<KichThuoc> lstkt = db.KichThuocs.ToList();
            ViewBag.KT = lstkt;
            int c1 = db.MauSacs.Count();
            int c2 = db.KichThuocs.Count();
            ViewBag.Count = c1 * c2;
            return View(ctsp);
        }

        [Route("suasanpham")]
        [HttpGet]
        public IActionResult SuaSanPham(String masp)
        {
            List<LoaiSp> lstlsp = db.LoaiSps.ToList();
            ViewBag.MaLoaiSp = lstlsp;
            List<DoiTuongMh> lstdtmh = db.DoiTuongMhs.ToList();
            ViewBag.MaDoiTuongMh = lstdtmh;
            List<MauSac> lstms = db.MauSacs.ToList();
            ViewBag.MS = lstms;
            List<KichThuoc> lstkt = db.KichThuocs.ToList();
            ViewBag.KT = lstkt;
            var sp = db.SanPhams.SingleOrDefault(x => x.MaSanPham == masp);
            var lst = db.ChiTietSanPhams.Where(x => x.MaSanPham == masp).ToList();
            SanPhamViewModel spvmd = new SanPhamViewModel
            {
                SanPham = sp,
                ChiTietSanPham = lst
            };
            return View(spvmd);
        }

        [Route("suasanpham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(SanPhamViewModel spc)
        {
            if (spc != null && spc.SanPham != null)
            {
                var sanpham = spc.SanPham;
                db.Entry(sanpham).State = EntityState.Modified;
                int v=db.SaveChanges();
                if (spc.ChiTietSanPham != null && spc.ChiTietSanPham.Any())
                {
                    foreach (var item in spc.ChiTietSanPham)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }
                    int c = db.SaveChanges();
                    if (c > 0)
                    {
                        return RedirectToAction("DanhSachSanPham");
                    }
                }
                if (v > 0)
                {
                    return RedirectToAction("DanhSachSanPham");
                }
            }
            List<LoaiSp> lstlsp = db.LoaiSps.ToList();
            ViewBag.MaLoaiSp = lstlsp;
            List<DoiTuongMh> lstdtmh = db.DoiTuongMhs.ToList();
            ViewBag.MaDoiTuongMh = lstdtmh;
            List<MauSac> lstms = db.MauSacs.ToList();
            ViewBag.MS = lstms;
            List<KichThuoc> lstkt = db.KichThuocs.ToList();
            ViewBag.KT = lstkt;
            var sp = db.SanPhams.SingleOrDefault(x => x.MaSanPham == spc.SanPham.MaSanPham);
            var lst = db.ChiTietSanPhams.Where(x => x.MaSanPham == spc.SanPham.MaSanPham).ToList();
            SanPhamViewModel spvmd = new SanPhamViewModel
            {
                SanPham = sp,
                ChiTietSanPham = lst
            };
            return View(spc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaSanPham(List<SanPham> sp)
        {
            TempData["Msg"] = "";
            if (sp == null || !sp.Any())
            {
                TempData["Msg"] = "Vui lòng chọn ít nhất một sản phẩm để xóa";
                return RedirectToAction("DanhSachSanPham");
            }

            foreach (var s in sp)
            {
                SanPham spt = db.SanPhams.SingleOrDefault(x=>x.MaSanPham==s.MaSanPham);

                if (spt != null)
                {
                    // Xóa tất cả các chi tiết sản phẩm liên quan đến sản phẩm
                    List<ChiTietSanPham> ctsps = db.ChiTietSanPhams.Where(ctsp => ctsp.MaSanPham == s.MaSanPham).ToList();
                    if (ctsps != null && ctsps.Count > 0)
                    {
                        db.ChiTietSanPhams.RemoveRange(ctsps);
                        db.SaveChanges();

                    }

                    db.SanPhams.Remove(spt);
                    int c = db.SaveChanges();

                    if (c > 0)
                    {
                        TempData["Msg"] = "Xóa thành công";
                        return RedirectToAction("DanhSachSanPham");
                    }
                }
            }
            TempData["Msg"] = "Không thể xóa";
            return RedirectToAction("DanhSachSanPham");
        }

        [Route("xoactsanpham")]
        [HttpGet]
        public IActionResult XoaCTSanPham(String masp)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSanPham == masp);
            ViewBag.SP = sp;
            List<MauSac> lstms = db.MauSacs.ToList();
            ViewBag.MS = lstms;
            List<KichThuoc> lstkt = db.KichThuocs.ToList();
            ViewBag.KT = lstkt;
            List<ChiTietSanPham> lst = db.ChiTietSanPhams.Where(x => x.MaSanPham == masp).ToList();
            ViewBag.CT = lst;
            return View();
        }

        [Route("xoactsanpham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaCTSanPham(CTSPViewModel ctsp)
        {
            if (ctsp.ChiTietSanPham == null || !ctsp.ChiTietSanPham.Any())
            {
                ViewBag.Msg = "Hãy chọm ít nhất một chi tiết để xóa!";
            }
            else
            {
                List<ChiTietSanPham> lst = new List<ChiTietSanPham>();
                foreach (var item in ctsp.ChiTietSanPham)
                {
                    lst.Add(db.ChiTietSanPhams.SingleOrDefault(x => x.MaSanPham == item.MaSanPham && x.MaMauSac == item.MaMauSac
                    && x.MaKichThuoc == item.MaKichThuoc));
                }
                db.ChiTietSanPhams.RemoveRange(lst);
                int c = db.SaveChanges();
                if (c > 0)
                {
                    return RedirectToAction("DanhSachSanPham");
                }
            }
            SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSanPham == ctsp.MaSanPham);
            ViewBag.SP = sp;
            List<MauSac> lstms = db.MauSacs.ToList();
            ViewBag.MS = lstms;
            List<KichThuoc> lstkt = db.KichThuocs.ToList();
            ViewBag.KT = lstkt;
            List<ChiTietSanPham> lstct = db.ChiTietSanPhams.Where(x => x.MaSanPham == ctsp.MaSanPham).ToList();
            ViewBag.CT = lstct;
            return View(ctsp);
        }
        public static string Convert(string input)
        {
            var result = Regex.Replace(input, "[àáảãạăắằẳẵặâầấẩẫậ]", "a");
            result = Regex.Replace(result, "[đ]", "d");
            result = Regex.Replace(result, "[èéẻẽẹêềếểễệ]", "e");
            result = Regex.Replace(result, "[ìíỉĩị]", "i");
            result = Regex.Replace(result, "[òóỏõọôồốổỗộơờớởỡợ]", "o");
            result = Regex.Replace(result, "[ùúủũụưừứửữự]", "u");
            result = Regex.Replace(result, "[ỳýỷỹỵ]", "y");
            result = Regex.Replace(result, "[^a-zA-Z0-9]", " ");

            return result;
        }

    }
}

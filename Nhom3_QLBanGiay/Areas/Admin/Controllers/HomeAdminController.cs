using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nhom3_QLBanGiay.Areas.Admin.ViewModels;
using Nhom3_QLBanGiay.Models;
using System.Data.Entity;
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
            return View();
        }

        [Route("danhsachsanpham")]
        public IActionResult DanhSachSanPham(int? page)
        {
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            int pageSize = 9;

            var lstsanpham = db.SanPhams.ToList().OrderBy(x => x.MaSanPham);
            PagedList<SanPham> lst = new PagedList<SanPham>(lstsanpham, pageNumber, pageSize);

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
            var sanpham = spc.SanPham;
            db.Entry(sanpham).State = EntityState.Modified;
            db.SaveChanges();
            foreach (var item in spc.ChiTietSanPham)
            {
                db.Entry(item).State = EntityState.Modified;
            }
            int c=db.SaveChanges();
            if (c > 0)
            {
                return RedirectToAction("DanhSachSanPham");
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
    }
}

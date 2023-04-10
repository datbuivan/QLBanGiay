using Nhom3_QLBanGiay.Models;

namespace Nhom3_QLBanGiay.Areas.Admin.ViewModels
{
    public class SanPhamViewModel
    {
      
        public SanPham? SanPham { get; set; }
        public MauSac? MauSac { get; set; }
        public KichThuoc? KichThuoc { get; set; }
        public List<ChiTietSanPham>? ChiTietSanPham { get; set; }
        
    }
}

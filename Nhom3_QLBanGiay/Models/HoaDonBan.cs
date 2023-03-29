using System;
using System.Collections.Generic;

namespace Nhom3_QLBanGiay.Models;

public partial class HoaDonBan
{
    public string MaHoaDonBan { get; set; } = null!;

    public DateOnly? NgayBan { get; set; }

    public string MaKhachHang { get; set; } = null!;

    public string MaNhanVien { get; set; } = null!;

    public double? TongTien { get; set; }

    public virtual ICollection<ChiTietHoaDonBan> ChiTietHoaDonBans { get; } = new List<ChiTietHoaDonBan>();

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual NhanVien MaNhanVienNavigation { get; set; } = null!;
}

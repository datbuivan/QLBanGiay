using System;
using System.Collections.Generic;

namespace Nhom3_QLBanGiay.Models;

public partial class NhanVien
{
    public string MaNhanVien { get; set; } = null!;

    public string? TenNhanVien { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string UserName { get; set; } = null!;

    public virtual ICollection<HoaDonBan> HoaDonBans { get; } = new List<HoaDonBan>();

    public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; } = new List<HoaDonNhap>();

    public virtual User UserNameNavigation { get; set; } = null!;
}

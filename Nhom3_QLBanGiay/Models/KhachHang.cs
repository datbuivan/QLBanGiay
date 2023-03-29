using System;
using System.Collections.Generic;

namespace Nhom3_QLBanGiay.Models;

public partial class KhachHang
{
    public string MaKhachHang { get; set; } = null!;

    public string? TenKhachHang { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string UserName { get; set; } = null!;

    public virtual ICollection<HoaDonBan> HoaDonBans { get; } = new List<HoaDonBan>();

    public virtual User UserNameNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Nhom3_QLBanGiay.Models;

public partial class SanPham
{
    public string MaSanPham { get; set; } = null!;

    public string? TenSanPham { get; set; }

    public string? ChatLieu { get; set; }

    public double? GiaNhap { get; set; }

    public double? GiaBan { get; set; }

    public string? HinhAnhAvatar { get; set; }

    public string MaLoaiSp { get; set; } = null!;

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; } = new List<ChiTietSanPham>();

    public virtual LoaiSp MaLoaiSpNavigation { get; set; } = null!;
}

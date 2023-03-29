using System;
using System.Collections.Generic;

namespace Nhom3_QLBanGiay.Models;

public partial class MauSac
{
    public string MaMauSac { get; set; } = null!;

    public string? TenMauSac { get; set; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; } = new List<ChiTietSanPham>();
}

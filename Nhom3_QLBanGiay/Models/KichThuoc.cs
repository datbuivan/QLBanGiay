using System;
using System.Collections.Generic;

namespace Nhom3_QLBanGiay.Models;

public partial class KichThuoc
{
    public string MaKichThuoc { get; set; } = null!;

    public string? TenKichThuoc { get; set; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; } = new List<ChiTietSanPham>();
}

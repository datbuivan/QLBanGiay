using System;
using System.Collections.Generic;

namespace Nhom3_QLBanGiay.Models;

public partial class LoaiSp
{
    public string MaLoaiSp { get; set; } = null!;

    public string? TenLoaiSp { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; } = new List<SanPham>();

    public virtual ICollection<DoiTuongMh> MaDoiTuongMhs { get; } = new List<DoiTuongMh>();
}

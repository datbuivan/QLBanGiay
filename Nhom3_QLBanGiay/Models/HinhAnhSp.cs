using System;
using System.Collections.Generic;

namespace Nhom3_QLBanGiay.Models;

public partial class HinhAnhSp
{
    public string TenFileAnh { get; set; } = null!;

    public string? MaSanPham { get; set; }

    public int? ViTri { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }
}

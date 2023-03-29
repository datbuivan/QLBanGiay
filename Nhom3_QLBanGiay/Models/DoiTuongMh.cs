using System;
using System.Collections.Generic;

namespace Nhom3_QLBanGiay.Models;

public partial class DoiTuongMh
{
    public string MaDoiTuongMh { get; set; } = null!;

    public string? TenDoiTuongMh { get; set; }

    public virtual ICollection<LoaiSp> MaLoaiSps { get; } = new List<LoaiSp>();
}

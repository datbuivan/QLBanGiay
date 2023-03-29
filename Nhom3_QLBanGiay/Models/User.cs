using System;
using System.Collections.Generic;

namespace Nhom3_QLBanGiay.Models;

public partial class User
{
    public string UserName { get; set; } = null!;

    public string? PassWord { get; set; }

    public int Role { get; set; }

    public virtual ICollection<KhachHang> KhachHangs { get; } = new List<KhachHang>();

    public virtual ICollection<NhanVien> NhanViens { get; } = new List<NhanVien>();
}

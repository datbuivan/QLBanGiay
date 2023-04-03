using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nhom3_QLBanGiay.Models;

public partial class User
{
    [RegularExpression(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "Email nhập không đúng định dạng !")]
    public string UserName { get; set; } = null!;

    [RegularExpression(@"[A-Za-z0-9].{5,}", ErrorMessage = "Mật khẩu ít nhất 5 ký tự !")]
    public string? PassWord { get; set; }

    public int Role { get; set; }

    public virtual ICollection<KhachHang> KhachHangs { get; } = new List<KhachHang>();

    public virtual ICollection<NhanVien> NhanViens { get; } = new List<NhanVien>();
}

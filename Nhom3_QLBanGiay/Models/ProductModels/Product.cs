namespace Nhom3_QLBanGiay.Models.ProductModels
{
    public class Product
    {
        public string MaSanPham { get; set; } = null!;

        public string? TenSanPham { get; set; }
        public double? GiaBan { get; set; }
        public string? HinhAnhAvatar { get; set; }

        public string MaLoaiSp { get; set; } = null!;
    }
}

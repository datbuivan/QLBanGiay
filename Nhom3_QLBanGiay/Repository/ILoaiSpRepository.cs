using Nhom3_QLBanGiay.Models;

namespace Nhom3_QLBanGiay.Repository
{
    public interface ILoaiSpRepository
    {
        LoaiSp Add(LoaiSp loaisp);
        LoaiSp Update(string maloai);
        LoaiSp Delete(string maloai);
        LoaiSp GetLoaiSp(string MaLoai);
        IEnumerable<LoaiSp> GetAllLoaiSp();
    }
}

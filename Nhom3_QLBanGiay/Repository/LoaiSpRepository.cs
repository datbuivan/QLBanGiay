using Nhom3_QLBanGiay.Models;

namespace Nhom3_QLBanGiay.Repository
{
    public class LoaiSpRepository : ILoaiSpRepository
    {
        private readonly QlbanGiayContext _context;
        public LoaiSpRepository(QlbanGiayContext context)
        {
            _context= context;
        }
        public LoaiSp Add(LoaiSp loaisp)
        {
            throw new NotImplementedException();
        }

        public LoaiSp Delete(string maloai)
        {
            throw new NotImplementedException();
        }

        public DoiTuong GetAllLoaiSp()
        {
            return new DoiTuong
            {
                LoaiSp = _context.LoaiSps.ToList(),
                DoiTuongMh = _context.DoiTuongMhs.ToList()
            };
        }

        public LoaiSp GetLoaiSp(string MaLoai)
        {
            throw new NotImplementedException();
        }

        public LoaiSp Update(string maloai)
        {
            throw new NotImplementedException();
        }
    }
}

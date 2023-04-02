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

        public IEnumerable<LoaiSp> GetAllLoaiSp()
        {
            return _context.LoaiSps;
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

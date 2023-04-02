using Microsoft.AspNetCore.Mvc;
using Nhom3_QLBanGiay.Repository;

namespace Lab2.ViewComponents
{
    public class LoaiSpMenuViewComponent :ViewComponent
    {
        private readonly ILoaiSpRepository _loaiSPRepository;

        public LoaiSpMenuViewComponent(ILoaiSpRepository loaiSPRepository)
        {
            _loaiSPRepository = loaiSPRepository;
        }

        public IViewComponentResult Invoke()
        {
            var loaisps = _loaiSPRepository.GetAllLoaiSp().OrderBy(x => x.TenLoaiSp);
            return View(loaisps);
        }
    }
}

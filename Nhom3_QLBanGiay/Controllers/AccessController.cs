using Microsoft.AspNetCore.Mvc;
using Nhom3_QLBanGiay.Models;

namespace Nhom3_QLBanGiay.Controllers
{
    public class AccessController : Controller
    {
        QlbanGiayContext db = new QlbanGiayContext();

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        [HttpPost]
        public IActionResult Login(User user)
        {
            if (HttpContext.Session.GetString("UseHome") == null)
            {
                var u = db.Users.Where(x => x.UserName == user.UserName && x.PassWord == user.PassWord).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("UserName", u.UserName.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "access");
        }
    }
}

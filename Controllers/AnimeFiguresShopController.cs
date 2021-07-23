using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAnime.Models;
using PagedList;
using PagedList.Mvc;


namespace WebAnime.Controllers
{
    public class AnimeFiguresShopController : Controller
    {
        dbwebmohinhDataContext data = new dbwebmohinhDataContext();

        private List<MoHinh> Mohinhmoi(int count)
        {
            return data.MoHinhs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);


            var mohinhmoi = Mohinhmoi(15);
            return View(mohinhmoi.ToPagedList(pageNum, pageSize));
        }

        public ActionResult DanhMuc()
        {
            var danhmuc = from dm in data.DanhMucs select dm;
            return PartialView(danhmuc);
        }

        public ActionResult ChuDe()
        {
            var chude = from cd in data.ChuDes select cd;
            return PartialView(chude);
        }

        public ActionResult SPTheochude(int id)
        {
            var mohinh = from m in data.MoHinhs where m.MaCD == id select m;
            return View(mohinh);
        }

        public ActionResult SPTheodanhmuc(int id)
        {
            var mohinh = from m in data.MoHinhs where m.MaDM == id select m;
            return View(mohinh);
        }

        public ActionResult Details(int id)
        {
            var mohinh = from m in data.MoHinhs
                         where m.Mamohinh == id
                       select m;
            return View(mohinh.Single());
        }

    }
}  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAnime.Models;
using System.IO;
using PagedList;
using PagedList.Mvc;


namespace WebAnime.Controllers
{
    public class AdminController : Controller
    {
        dbwebmohinhDataContext db = new dbwebmohinhDataContext();
        public ActionResult Index()
        {
            {
                if (Session["Taikhoanadmin"] == null)
                    return RedirectToAction("Login", "Admin");
                else
                    return View(db.MoHinhs.ToList());
            }
        }

        //Mô hình
        public ActionResult Mohinh(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.MoHinhs.ToList().OrderBy(n => n.Mamohinh).ToPagedList(pageNumber, pageSize));
        }

        

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {

                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiMohinh()
        {

            ViewBag.MaDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM");
            ViewBag.MaCD = new SelectList(db.ChuDes.ToList().OrderBy(n => n.Tenchude), "MaCD", "Tenchude");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiMohinh(MoHinh mohinh, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM");
            ViewBag.MaCD = new SelectList(db.ChuDes.ToList().OrderBy(n => n.Tenchude), "MaCD", "Tenchude");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    mohinh.Anhbia = fileName;
                    db.MoHinhs.InsertOnSubmit(mohinh);
                    db.SubmitChanges();
                }
                return RedirectToAction("MoHinh");
            }
        }

        public ActionResult Chitietmohinh(int id)
        {

            MoHinh mohinh = db.MoHinhs.SingleOrDefault(n => n.Mamohinh == id);
            ViewBag.Mamohinh = mohinh.Mamohinh;
            if (mohinh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mohinh);
        }

        [HttpGet]
        public ActionResult Xoamohinh(int id)
        {
            MoHinh mohinh = db.MoHinhs.SingleOrDefault(n => n.Mamohinh == id);
            ViewBag.Mamohinh = mohinh.Mamohinh;
            if (mohinh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mohinh);
        }
        [HttpPost, ActionName("Xoamohinh")]
        public ActionResult Xacnhanxoa(int id)
        {
            MoHinh mohinh = db.MoHinhs.SingleOrDefault(n => n.Mamohinh == id);
            ViewBag.Mamohinh = mohinh.Mamohinh;
            if (mohinh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.MoHinhs.DeleteOnSubmit(mohinh);
            db.SubmitChanges();
            return RedirectToAction("MoHinh");
        }
        [HttpGet]
        public ActionResult Suamohinh(int id)
        {
            MoHinh mohinh = db.MoHinhs.SingleOrDefault(n => n.Mamohinh == id);

            if (mohinh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.ChuDes.ToList().OrderBy(n => n.Tenchude), "MaCD", "TenChude", mohinh.MaCD);
            ViewBag.MaDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM", mohinh.MaDM);
            return View(mohinh);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suamohinh(MoHinh mohinh, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.ChuDes.ToList().OrderBy(n => n.Tenchude), "MaCD", "TenChude", mohinh.MaCD);
            ViewBag.MaDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM", mohinh.MaDM);
            MoHinh mh = db.MoHinhs.FirstOrDefault(n => n.Mamohinh == mohinh.Mamohinh);
            mh.Tenmohinh = mohinh.Tenmohinh;
            mh.Giaban = mohinh.Giaban;
            mh.Soluongton = mohinh.Soluongton;
            mh.Ngaycapnhat = mohinh.Ngaycapnhat;
            mh.Mota = mohinh.Mota;
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else 
            { 
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/Figures"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    mohinh.Anhbia = fileName;
                }
                mh.Anhbia = mohinh.Anhbia;
                UpdateModel(mohinh);
                db.SubmitChanges();
            }
            return RedirectToAction("MoHinh");
        }

        //Chủ Đề

        public ActionResult ChuDe(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(db.ChuDes.ToList());
        }

        [HttpGet]
        public ActionResult Themmoichude()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            return View();
        }
        [HttpPost]
        public ActionResult Themmoichude(ChuDe chude)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                db.ChuDes.InsertOnSubmit(chude);
                db.SubmitChanges();
                return RedirectToAction( "ChuDe", "Admin");
            }
        }

        public ActionResult Chitietchude(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var chude = from cd in db.ChuDes where cd.MaCD == id select cd;
                return View(chude.SingleOrDefault());
            }
        }

        public ActionResult Xoachude(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var chude = from cd in db.ChuDes where cd.MaCD == id select cd;
                return View(chude.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Xoachude")]
        public ActionResult Xacnhanxoachude(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ChuDe chude = db.ChuDes.SingleOrDefault(n => n.MaCD == id);
                db.ChuDes.DeleteOnSubmit(chude);
                db.SubmitChanges();
                return RedirectToAction("ChuDe", "Admin");
            }
        }

        public ActionResult Suachude(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var chude = from cd in db.ChuDes where cd.MaCD == id select cd;
                return View(chude.SingleOrDefault());
            }
        }

        [HttpPost, ActionName("Suachude")]
        public ActionResult Xacnhansuachude(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ChuDe chude = db.ChuDes.SingleOrDefault(n => n.MaCD == id);

                UpdateModel(chude);
                db.SubmitChanges();
                return RedirectToAction("ChuDe", "Admin");
            }
        }

        //Danh Mục

        public ActionResult DanhMuc()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(db.DanhMucs.ToList());
        }


        [HttpGet]
        public ActionResult Themmoidanhmuc()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpPost]
        public ActionResult Themmoidanhmuc(DanhMuc danhmuc)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                db.DanhMucs.InsertOnSubmit(danhmuc);
                db.SubmitChanges();
                return RedirectToAction("DanhMuc", "Admin");
            }
        }

        public ActionResult Chitietdanhmuc(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var danhmuc = from dm in db.DanhMucs where dm.MaDM == id select dm;
                return View(danhmuc.SingleOrDefault());
            }
        }

        public ActionResult Xoadanhmuc(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var danhmuc = from dm in db.DanhMucs where dm.MaDM == id select dm;
                return View(danhmuc.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Xoadanhmuc")]
        public ActionResult Xacnhanxoadanhmuc(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                DanhMuc danhmuc = db.DanhMucs.SingleOrDefault(n => n.MaDM == id);
                db.DanhMucs.DeleteOnSubmit(danhmuc);
                db.SubmitChanges();
                return RedirectToAction("DanhMuc", "Admin");
            }
        }

        public ActionResult Suadanhmuc(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var danhmuc = from dm in db.DanhMucs where dm.MaDM == id select dm;
                return View(danhmuc.SingleOrDefault());
            }
        }

        [HttpPost, ActionName("Suadanhmuc")]
        public ActionResult Xacnhansuadanhmuc(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                DanhMuc danhmuc = db.DanhMucs.SingleOrDefault(n => n.MaDM == id);

                UpdateModel(danhmuc);
                db.SubmitChanges();
                return RedirectToAction("DanhMuc", "Admin");
            }
        }
        //KhachHang
        public ActionResult KhachHang()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(db.KhachHangs.ToList());
        }

        [HttpGet]
        public ActionResult Themmoikhachhang()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpPost]
        public ActionResult Themmoikhachhang(KhachHang khachhang)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                db.KhachHangs.InsertOnSubmit(khachhang);
                db.SubmitChanges();
                return RedirectToAction("KhachHang", "Admin");
            }
        }

        public ActionResult Chitietkhachhang(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var khachhang = from kh in db.KhachHangs where kh.MaKH == id select kh;
                return View(khachhang.SingleOrDefault());
            }
        }

        public ActionResult Xoakhachhang(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var khachhang = from kh in db.KhachHangs where kh.MaKH == id select kh;
                return View(khachhang.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Xoakhachhang")]
        public ActionResult Xacnhanxoakhachhangc(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                KhachHang khachhang = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);
                db.KhachHangs.DeleteOnSubmit(khachhang);
                db.SubmitChanges();
                return RedirectToAction("KhachHang", "Admin");
            }
        }

        public ActionResult Suakhachhang(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var khachhang = from kh in db.KhachHangs where kh.MaKH == id select kh;
                return View(khachhang.SingleOrDefault());
            }
        }

        [HttpPost, ActionName("Suakhachhang")]
        public ActionResult Xacnhansuakhachhang(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                KhachHang khachhang = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);

                UpdateModel(khachhang);
                db.SubmitChanges();
                return RedirectToAction("KhachHang", "Admin");
            }
        }
    }

}
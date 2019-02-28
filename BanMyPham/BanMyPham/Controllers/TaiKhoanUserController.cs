using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanMyPham.Models;

namespace BanMyPham.Controllers
{
    public class TaiKhoanUserController : Controller
    {
        BanMyPhamEntities db = new BanMyPhamEntities();
        // GET: TaiKhoanUser
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult UserBar()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(string username, string pass)
        {
            var khachhang = db.KhachHang.SingleOrDefault(n => n.TaiKhoan == username && n.MatKhau == pass);

            if (khachhang == null)
            {
                return View();
            }
            else
            {
                Session["TaiKhoanUser"] = khachhang.TaiKhoan.ToString();
                Session["TenKhachHang"] = khachhang.TenKhachHang;

                if (Session["isTrongGioHang"] != null && Session["isTrongGioHang"].ToString() == "1")
                {
                    Session["isTrongGioHang"] = 0;
                    return RedirectToAction("XemGioHang", "NguoiDung");
                }
                else
                {
                    return RedirectToAction("TrangChu", "NguoiDung");
                }
            }
        }

        public ActionResult DangXuat()
        {
            Session.Remove("TaiKhoanUser");
            Session.Remove("TenKhachHang");
            Session.Remove("GioHang");
            Session.Remove("isTrongGioHang");
            return RedirectToAction("TrangChu", "NguoiDung");
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy([Bind(Include = "MaKhachHang,TenKhachHang,DiaChi,SoDienThoai,GioiTinh,TaiKhoan,MatKhau,Email")] KhachHang khachHang)
        {
            var kh = db.KhachHang.SingleOrDefault(n => n.TaiKhoan == khachHang.TaiKhoan);
            if (kh != null)
            {
                ViewBag.thongbao = "Tài khoản đã tồn tại";
                return View();
            }
            else if (ModelState.IsValid)
            {
                khachHang.MaKhachHang = 1;
                db.KhachHang.Add(khachHang);
                db.SaveChanges();
                //Session["TaiKhoanUser"] = khachHang.TaiKhoan.ToString();
                //Session["TenKhachHang"] = khachHang.TenKhachHang;
                return RedirectToAction("DangNhap", "TaiKhoanUser");
            }
            return RedirectToAction("DangKy", "TaiKhoanUser");
        }
    }
}
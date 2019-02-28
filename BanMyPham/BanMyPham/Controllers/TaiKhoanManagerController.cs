using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanMyPham.Models;
using System.Net;
using System.Data.Entity;

namespace BanMyPham.Controllers
{
    
    public class TaiKhoanManagerController : Controller
    {
        BanMyPhamEntities db = new BanMyPhamEntities();
        // GET: TaiKhoanManager
        public ActionResult Index()
        {
            if(Session["TaiKhoanUser"]==null)
            {
                return RedirectToAction("DangNhap", "TaiKhoanUser");
            }
            string taikhoankhachhang = Session["TaiKhoanUser"].ToString();
            var kh = db.KhachHang.SingleOrDefault(n => n.TaiKhoan == taikhoankhachhang);
            return View(kh);
        }

        public PartialViewResult DonHangKhachHang ()
        {
            string taikhoankhachhang = Session["TaiKhoanUser"].ToString();
            KhachHang kh = db.KhachHang.SingleOrDefault(n => n.TaiKhoan == taikhoankhachhang);
            var dh = db.DonHang.Include(a => a.ChiTietDonHang.Select(b => b.SanPham)).Where(c => c.MaKhachHang == kh.MaKhachHang).ToList();
            return PartialView(dh);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHang.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKhachHang,TenKhachHang,DiaChi,SoDienThoai,GioiTinh,TaiKhoan,MatKhau,Email")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }

    }
}
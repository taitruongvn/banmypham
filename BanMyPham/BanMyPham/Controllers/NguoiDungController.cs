using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanMyPham.Models;
using PagedList;
using PagedList.Mvc;

namespace BanMyPham.Controllers
{
    public class NguoiDungController : Controller
    {
        BanMyPhamEntities db = new BanMyPhamEntities();
        // GET: NguoiDung
        public ActionResult TrangChu()
        {
            Session["isTrongGioHang"] = 0;
            var sanpham = db.SanPham.OrderByDescending(n=>n.MaSanPham).Take(8).ToList();
            return View(sanpham);
        }

        public PartialViewResult SanPhamBanChayPartial()
        {

            var sanpham = (from q in db.SanPham
                       orderby q.ChiTietDonHang.Count() descending
                       select q).Take(8).ToList();

            return PartialView(sanpham);
        }

        public ActionResult Menu()
        {
            var nhomhang = db.NhomHang.ToList();
            return PartialView(nhomhang);
        }
        public ActionResult SanPhamTheoNhomHang(int iMaNhomHang, int? page)
        {
            

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            var lstSanPhamTheoNhomHang = db.SanPham.Where(n => n.MaNhomHang == iMaNhomHang).OrderBy(m=>m.MaSanPham).ToPagedList(pageNumber, pageSize);

            NhomHang nhomhang = db.NhomHang.Single(m => m.MaNhomHang == iMaNhomHang);
            ViewBag.TenNhomHang = nhomhang.TenNhomHang;
            ViewBag.iMaNhomHang = iMaNhomHang;

            return View(lstSanPhamTheoNhomHang);
        }

        public ActionResult ChiTietSanPham(int iMaSanPham)
        {
            var sanpham = db.SanPham.SingleOrDefault(n => n.MaSanPham == iMaSanPham);
            Session["iMaNhomHang"] = sanpham.MaNhomHang;
            Session["iMaSanPham"] = sanpham.MaSanPham;

            ViewBag.TenNhomHang = db.NhomHang.SingleOrDefault(m => m.MaNhomHang == sanpham.MaNhomHang).TenNhomHang.ToString();

            return View(sanpham);
        }

        public ActionResult SanPhamLienQuan()
        {
            int iMaNhomHang = (int)Session["iMaNhomHang"];
            var sanpham = db.SanPham.Where(n => n.MaNhomHang == iMaNhomHang).Take(12).ToList();
            return PartialView(sanpham);
        }

        //GioHang

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;

            if (Session["GioHang"] == null)
            {
                Session["GioHang"] = new List<GioHang>();
                lstGioHang = Session["GioHang"] as List<GioHang>;
            }
            return lstGioHang;
        }

        public ActionResult ThemVaoGioHang(int sMaSanPham, string sUrl, int iSoLuong)
        {
            List<GioHang> gh = LayGioHang();

            SanPham sanpham = db.SanPham.SingleOrDefault(n => n.MaSanPham == sMaSanPham);

            if (sanpham == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                GioHang sp = gh.SingleOrDefault(m => m.iMaSanPham == sMaSanPham);

                if (sp != null)
                {
                    sp.iSoLuong+=iSoLuong;
                }

                else
                {
                    sp = new GioHang(sMaSanPham,iSoLuong);
                    gh.Add(sp);
                }
            }       
            return Redirect(sUrl);
        }

        public ActionResult XemGioHang()
        {
            var lstGioHang = LayGioHang().ToList();
            if (lstGioHang.Count() == 0)
            {
                return RedirectToAction("TrangChu", "NguoiDung");
            }
            ViewBag.Tongtien = TongTien();
            return View(lstGioHang);
        }

        public ActionResult CapNhatGioHang(int sMaSanPham, FormCollection f)
        {
            int iSoLuong = int.Parse(f["sSoLuong"].ToString());
            SanPham sanpham = db.SanPham.SingleOrDefault(n => n.MaSanPham == sMaSanPham);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> gh = LayGioHang();
            GioHang sp = gh.SingleOrDefault(m => m.iMaSanPham == sMaSanPham);

            if (sp == null)
            {
                return RedirectToAction("XemGioHang");
            }
            else
            {
                sp.iSoLuong = iSoLuong;
                return RedirectToAction("XemGioHang");
            }
        }

        public ActionResult XoaGioHang(int sMaSanPham)
        {
            SanPham sanpham = db.SanPham.SingleOrDefault(n => n.MaSanPham == sMaSanPham);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> gh = LayGioHang();
            GioHang sp = gh.SingleOrDefault(m => m.iMaSanPham == sMaSanPham);

            if (sp == null)
            {
                return RedirectToAction("XemGioHang");
            }
            else
            {
                gh.Remove(sp);
                return RedirectToAction("XemGioHang");
            }
        }

        public decimal TongTien()
        {
            List<GioHang> lstGioHang = LayGioHang();
            decimal Tongtien = 0;
            foreach (var item in lstGioHang)
            {
                Tongtien = Tongtien + item.dThanhTien;
            }

            return Tongtien;
        }

        public int SoLuong()
        {
            List<GioHang> gh = LayGioHang();
            int Soluong = 0;
            foreach (var item in gh)
            {
                Soluong = Soluong + item.iSoLuong;
            }

            return Soluong;
        }

        public PartialViewResult GioHangPartial ()
        {
            ViewBag.Soluong = SoLuong();
            return PartialView();
        }

        public PartialViewResult GioHangMiniPartial()
        {
            List<GioHang> lstGioHang = LayGioHang().ToList();
            ViewBag.Tongtien = TongTien();
            return PartialView(lstGioHang);

        }

        public ActionResult DatHang()
        {
            var lstGioHang = LayGioHang();

            if (lstGioHang.Count() == 0)
            {
                return RedirectToAction("TrangChu", "NguoiDung");
            }

            if (Session["TaiKhoanUser"] == null)
            {
                Session["isTrongGioHang"] = 1;
                return RedirectToAction("DangNhap", "TaiKhoanUser");
            }

            string taikhoanuser = Session["TaiKhoanUser"].ToString();

            KhachHang kh = db.KhachHang.Single(n => n.TaiKhoan == taikhoanuser);

            DonHang dh = new DonHang();

            dh.NgayDatHang = DateTime.Now;
            dh.MaKhachHang = kh.MaKhachHang;
            db.DonHang.Add(dh);
            dh.TinhTrang = false;
            db.SaveChanges();

            DonHang dh2 = db.DonHang.ToList().OrderByDescending(n => n.MaDonHang).FirstOrDefault();

            foreach (var item in lstGioHang)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonHang = dh2.MaDonHang;
                ctdh.MaSanPham = item.iMaSanPham;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = item.dDonGia;
                ctdh.ThanhTien = item.dThanhTien;

                db.ChiTietDonHang.Add(ctdh);

            }
            db.SaveChanges();
            lstGioHang.Clear();
            return RedirectToAction("TrangChu", "NguoiDung");
        }

        public ActionResult SanPhamBanChay (int? page)
        {

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var lstSanPhamBanChay = (from q in db.SanPham
                           orderby q.ChiTietDonHang.Count() descending
                           select q).Take(36).ToList();

            return PartialView(lstSanPhamBanChay.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult SanPhamMoi(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var lstSanPhamMoi = db.SanPham.OrderByDescending(n => n.MaSanPham).Take(36).ToList();
            return View(lstSanPhamMoi.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SanPhamChamSocMat(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);

            var lstSanPhamChamSocMat = db.SanPham.Where(n => n.MaNhomHang == 1 || n.MaNhomHang == 2 || n.MaNhomHang == 3).OrderBy(m=>m.MaSanPham).ToPagedList(pageNumber,pageSize);
            return View(lstSanPhamChamSocMat);
        }

        public ActionResult SanPhamChamSocCoThe(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);

            var lstSanPhamChamSocMat = db.SanPham.Where(n => n.MaNhomHang == 4).OrderBy(m => m.MaSanPham).ToPagedList(pageNumber, pageSize);
            return View(lstSanPhamChamSocMat);
        }

        public ActionResult SanPhamChamSocToc(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);

            var lstSanPhamChamSocMat = db.SanPham.Where(n => n.MaNhomHang == 5).OrderBy(m => m.MaSanPham).ToPagedList(pageNumber, pageSize);
            return View(lstSanPhamChamSocMat);
        }

        public ActionResult TimKiemButtonPartial ()
        {           
            return PartialView();
        }

        public ActionResult TimKiem(string sTenSanPham)
        {
            var lstKetQua = db.SanPham.Where(n => n.TenSanPham.Contains(sTenSanPham)).OrderBy(m => m.MaSanPham).ToList();
            return View(lstKetQua);
        }

        public ActionResult SanPhamGiamGia(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var lstSanPhamGiamGia = db.SanPham.Where(n => n.GiamGia != null && n.GiamGia>0).OrderBy(m=>m.MaSanPham).ToPagedList(pageNumber,pageSize);
            return View(lstSanPhamGiamGia);
        }

        public ActionResult FAQ ()
        {
            return View();
        }
    }
}
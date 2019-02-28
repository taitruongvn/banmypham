using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanMyPham.Models
{
    public class GioHang
    {
        BanMyPhamEntities db = new BanMyPhamEntities();
        public int iMaSanPham { get; set; }
        public string sTenSanPham { get; set; }
        public int iSoLuong { get; set; }
        public decimal dDonGia { get; set; }
        public string sHinhAnh { get; set; }
        public decimal dThanhTien
        {
            get { return dDonGia * iSoLuong; }
        }

        public GioHang(int MaSanPham, int SoLuong)
        {
            iMaSanPham = MaSanPham;
            SanPham sanpham = db.SanPham.Single(n => n.MaSanPham == iMaSanPham);
            sTenSanPham = sanpham.TenSanPham;
            sHinhAnh = sanpham.HinhAnh;
            if (sanpham.GiamGia!=null && sanpham.GiamGia>0)
            {
                dDonGia = (decimal)(sanpham.DonGia-sanpham.GiamGia);
            }
            else
            {
                dDonGia = (decimal)sanpham.DonGia;
            }
            iSoLuong = SoLuong;
        }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BanMyPham.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NhanVien
    {
        public int MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public Nullable<int> SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public Nullable<System.DateTime> NgayVaoLam { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public Nullable<int> MaQuyen { get; set; }
    
        public virtual Quyen Quyen { get; set; }
    }
}

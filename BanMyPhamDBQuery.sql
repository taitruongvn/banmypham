use master
IF EXISTS(select * from sys.databases where name='BanMyPham')
DROP DATABASE BanMyPham
create database BanMyPham

go
use BanMyPham

--Hang Hoa
create table NhomHang
(
	MaNhomHang int,
	TenNhomHang nvarchar (50),

	primary key (MaNhomHang)
);

go
create table NhaCungCap
(
	MaNhaCungCap int,
	TenNhaCungCap nvarchar (40),
	DiaChi nvarchar (100),
	SoDienThoai int,

	primary key (MaNhaCungCap)
);

go
create table SanPham
(
	MaSanPham int,
	TenSanPham nvarchar(100),
	MaNhomHang int,
	MaNhaCungCap int,
	DonGia decimal,
	SoLuong int,
	HinhAnh varchar (100),
	MoTa nvarchar (200),

	primary key (MaSanPham),
	constraint fk_MaNhomHang foreign key (MaNhomHang) references NhomHang (MaNhomHang),
	constraint fk_MaNhaCungCap foreign key (MaNhaCungCap) references NhaCungCap (MaNhaCungCap) 
);

-- Khach Hang
go
create table KhachHang
(
	MaKhachHang int,
	TenKhachHang nvarchar (50),
	DiaChi nvarchar (100),
	SoDienThoai int,
	GioiTinh bit,
	TaiKhoan varchar (30),
	MatKhau varchar (20),
	Email varchar (50),

	primary key (MaKhachHang)
);

-- Don Hang
go
create table DonHang
(
	MaDonHang int,
	MaKhachHang int,
	NgayDatHang datetime,
	NgayGiaoHang datetime,
	TongTien decimal,
	TinhTrang bit,

	primary key (MaDonHang),
	constraint fk_MaKhachHang foreign key (MaKhachHang) references KhachHang (MaKhachHang)
);

go
create table ChiTietDonHang
(
	MaDonHang int,
	MaSanPham int,
	SoLuong int,
	DonGia decimal,
	ThanhTien decimal,

	primary key (MaDonHang, MaSanPham),
	constraint fk_MaDonHang foreign key (MaDonHang) references DonHang (MaDonHang),	
	constraint fk_MaHangHoa foreign key (MaSanPham) references SanPham (MaSanPham)
);

-- Nham Vien
go
create table Quyen
(
	MaQuyen int,
	TenQuyen nvarchar (20),

	primary key (MaQuyen)
);

go
create table NhanVien
(
	MaNhanVien int,
	TenNhanVien nvarchar (50),
	NgaySinh datetime,
	SoDienThoai int,
	DiaChi varchar (50),
	NgayVaoLam datetime,
	TaiKhoan varchar (30),
	MatKhau varchar (20),
	MaQuyen int,

	primary key (MaNhanVien),
	constraint fk_MaQuyen foreign key (MaQuyen) references Quyen (MaQuyen)
);





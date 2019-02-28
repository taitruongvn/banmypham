create TRIGGER trg_TangMaKhachHang
ON  KhachHang
INSTEAD OF INSERT
AS 
BEGIN
    SET NOCOUNT ON;

	declare @MaKhachHangCu int
	select @MaKhachHangCu = (SELECT TOP 1 MaKhachHang FROM KhachHang ORDER BY MaKhachHang DESC)

	if((select COUNT (*)  from KhachHang)!=0)

				INSERT INTO KhachHang([MaKhachHang], TenKhachHang, DiaChi, SoDienThoai, GioiTinh, TaiKhoan,MatKhau,Email)
				SELECT @MaKhachHangCu +1 , TenKhachHang, DiaChi, SoDienThoai, GioiTinh, TaiKhoan,MatKhau,Email
				FROM inserted
	else if((select COUNT (*)  from KhachHang)=0 or (select count (MaKhachHang) from inserted)=0)
				INSERT INTO KhachHang(MaKhachHang, TenKhachHang, DiaChi, SoDienThoai, GioiTinh, TaiKhoan,MatKhau,Email)
				SELECT 1 , TenKhachHang, DiaChi, SoDienThoai, GioiTinh, TaiKhoan,MatKhau,Email FROM inserted
END 
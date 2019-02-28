create trigger trg_TinhTongTien
on ChiTietDonHang
for insert
as
	begin
	declare @MaDonHang varchar(10)
	select  @MaDonHang = MaDonHang from inserted

	
	
	if exists (select MaDonHang from DonHang where MaDonHang = @MaDonHang)
	begin
	update DonHang set TongTien = (select SUM(ThanhTien) from ChiTietDonHang where MaDonHang = @MaDonHang) where MaDonHang =@MaDonHang
	end
go
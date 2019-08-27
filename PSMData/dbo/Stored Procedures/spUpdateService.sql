create procedure [dbo].[spUpdateService]
(
	@NomorNota int,
	@NamaPelanggan nvarchar(50),
	@NoHp varchar(12),
	@TipeHp nvarchar(50),
	@Imei varchar(4),
	@DamageId int,
	@YangBelumDicek nvarchar(50),
	@Kelengkapan varchar(50),
	@Warna varchar(50),
	@KataSandiPola varchar(50),
	@TechnicianId int,
	@StatusServisan varchar(50),
	@TanggalKonfirmasi datetime2,
	@IsiKonfirmasi nvarchar(256),
	@Biaya decimal(19, 4),
	@Discount int,
	@Dp decimal(19, 4),
	@TambahanBiaya decimal(19, 4),
	@HargaSparepart decimal(19, 4),
	@TanggalPengambilan datetime2
)
as
begin
	set nocount on;

	update [dbo].[Service] 
	set NamaPelanggan = isnull(@NamaPelanggan, NamaPelanggan), NoHp = isnull(@NoHp, NoHp), TipeHp = isnull(@TipeHp, TipeHp), Imei = isnull(@Imei, Imei),
		DamageId = isnull(@DamageId, DamageId), YangBelumDicek = isnull(@YangBelumDicek, YangBelumDicek), Kelengkapan = isnull(@Kelengkapan, Kelengkapan),
		Warna = isnull(@Warna, Warna), KataSandiPola = isnull(@KataSandiPola, KataSandiPola), TechnicianId = isnull(@TechnicianId, TechnicianId),
		StatusServisan = isnull(@StatusServisan, StatusServisan), TanggalKonfirmasi = isnull(@TanggalKonfirmasi, TanggalKonfirmasi),
		IsiKonfirmasi = isnull(@IsiKonfirmasi, IsiKonfirmasi), Biaya = isnull(@Biaya, Biaya), Discount = isnull(@Discount, Discount), Dp = isnull(@Dp, Dp),
		TambahanBiaya = isnull(@TambahanBiaya, TambahanBiaya), TotalBiaya = (@Biaya - (@Biaya * (cast(@Discount as decimal(19, 4)) / 100))) + @TambahanBiaya,
		HargaSparepart = isnull(@HargaSparepart, HargaSparepart), Sisa = (@Biaya - (@Biaya * (cast(@Discount as decimal(19, 4)) / 100))) + @TambahanBiaya - @Dp,
		LabaRugi = (@Biaya - (@Biaya * (cast(@Discount as decimal(19, 4)) / 100))) + @TambahanBiaya - @HargaSparepart, TanggalPengambilan = isnull(@TanggalPengambilan, TanggalPengambilan) 
	where NomorNota = @NomorNota;
end;
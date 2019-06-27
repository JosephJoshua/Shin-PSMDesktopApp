create procedure [dbo].[spInsertService]
(
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
	@LabaRugi decimal(19, 4),
	@TanggalPengambilan datetime2
)
as
begin
	set nocount on;

	insert into [dbo].[Service]
	(NomorNota, NamaPelanggan, NoHp, TipeHp, Imei, DamageId, YangBelumDicek, Kelengkapan, Warna, KataSandiPola, TechnicianId, StatusServisan, TanggalKonfirmasi,
	 IsiKonfirmasi, Biaya, Discount, Dp, TambahanBiaya, TotalBiaya, HargaSparepart, Sisa, LabaRugi, TanggalPengambilan)
	values
	(next value for [dbo].[serviceIdSequence], @NamaPelanggan, @NoHp, @TipeHp, @Imei, @DamageId, @YangBelumDicek, @Kelengkapan, @Warna, @KataSandiPola, @TechnicianId,
	 @StatusServisan, @TanggalKonfirmasi, @IsiKonfirmasi, @Biaya, @Discount, @Dp, @TambahanBiaya, 
	 (@Biaya - (@Biaya * (cast(@Discount as decimal(19, 4)) / 100))) + @TambahanBiaya, @HargaSparepart, (@Biaya - (@Biaya * (cast(@Discount as decimal(19, 4)) / 100))) + @TambahanBiaya - @Dp,
	 @LabaRugi, @TanggalPengambilan);
end
create procedure [dbo].[spInsertSparepart]
(
	@NomorNota int,
	@Nama nvarchar(50),
	@Harga decimal(19, 4)
)
as
begin
	set nocount on;

	insert into [dbo].[Sparepart] 
	(Id, NomorNota, Nama, Harga) 
	values 
	(next value for [dbo].[sparepartIdSequence], @NomorNota, @Nama,  @Harga)
end;
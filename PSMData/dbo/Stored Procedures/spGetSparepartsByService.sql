create procedure [dbo].[spGetSparepartsByService]
	@NomorNota int
as
begin
	set nocount on;

	select Id, NomorNota, Nama, Harga 
	from [dbo].[Sparepart]
	where NomorNota = @NomorNota;
end;
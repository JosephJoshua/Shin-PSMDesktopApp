create procedure [dbo].[spGetAllMembers]
as
begin
	set nocount on;

	select [Id], [Nama], [NoHp], [Alamat], [TipeHp1], [TipeHp2], [TipeHp3], [TipeHp4], [TipeHp5]
	from [dbo].[Member]
	order by Id;
end
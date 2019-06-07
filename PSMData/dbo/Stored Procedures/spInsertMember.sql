create procedure [dbo].[spInsertMember]
(
	@Nama nvarchar(50),
	@NoHp varchar(12),
	@Alamat nvarchar(50),
	@TipeHp1 nvarchar(50),
	@TipeHp2 nvarchar(50),
	@TipeHp3 nvarchar(50),
	@TipeHp4 nvarchar(50),
	@TipeHp5 nvarchar(50)
)
as
begin
	set nocount on;

	insert into [dbo].[Member]
	(Id, Nama, NoHp, Alamat, TipeHp1, TipeHp2, TipeHp3, TipeHp4, TipeHp5)
	values
	(next value for [dbo].[memberIdSequence], @Nama, @NoHp, @Alamat, @TipeHp1, @TipeHp2, @TipeHp3, @TipeHp4, @TipeHp5);
end
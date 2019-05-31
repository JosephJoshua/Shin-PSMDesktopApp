create procedure [dbo].[spUpdateMember]
(
	@Id int,
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

	update [dbo].[Member]
	set Nama = isnull(@Nama, Nama), NoHp = isnull(@NoHp, NoHp), Alamat = isnull(@Alamat, Alamat),
		TipeHp1 = isnull(@TipeHp1, TipeHp1), TipeHp2 = isnull(@TipeHp2, TipeHp2), TipeHp3 = isnull(@TipeHp3, TipeHp3),
		TipeHp4 = isnull(@TipeHp4, TipeHp4), TipeHp5 = isnull(@TipeHp5, TipeHp5)
	where Id = @Id;
end;
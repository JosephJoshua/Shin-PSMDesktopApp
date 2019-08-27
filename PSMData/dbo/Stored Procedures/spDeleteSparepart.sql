create procedure [dbo].[spDeleteSparepart]
	@Id int
as
begin
	set nocount on;

	delete from [dbo].[Sparepart] 
	where Id = @Id;
end;
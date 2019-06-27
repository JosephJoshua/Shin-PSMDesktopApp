create procedure [dbo].[spDeleteService]
	@NomorNota int
as
begin
	set nocount on;

	delete from [dbo].[Service]
	where [NomorNota] = @NomorNota;
end
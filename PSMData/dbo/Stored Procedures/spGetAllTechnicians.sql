create procedure [dbo].[spGetAllTechnicians]
as
begin
	set nocount on;

	select [Id], [Nama]
	from [dbo].[Technician]
	order by [Id];
end;
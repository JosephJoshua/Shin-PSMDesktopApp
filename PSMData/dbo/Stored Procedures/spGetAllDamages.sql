create procedure [dbo].[spGetAllDamages]
as
begin
	set nocount on;

	select [Id], [Kerusakan]
	from [dbo].[Damage]
	order by [Id];
end
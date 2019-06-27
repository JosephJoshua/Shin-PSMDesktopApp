create procedure [dbo].[spGetAllServices]
as
begin
	set nocount on;

	select *
	from [dbo].[Service]
	order by [NomorNota];
end
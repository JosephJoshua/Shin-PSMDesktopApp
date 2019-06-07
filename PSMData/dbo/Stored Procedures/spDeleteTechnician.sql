create procedure [dbo].[spDeleteTechnician]
	@Id int
as
begin
	set nocount on;

	delete from [dbo].[Technician]
	where [Id] = @Id;
end;
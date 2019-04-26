create procedure [dbo].[spGetUser]
	@Id nvarchar(128)
as
begin
	set nocount on;

	select [Id], [Username], [EmailAddress], [Role]
	from [dbo].[User]
	where Id = @Id;
end

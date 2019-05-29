create procedure [dbo].[spDeleteMember]
	@Id int
as
begin
	set nocount on;

	delete from [dbo].[Member]
	where [Id] = @Id;
end;
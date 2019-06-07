create procedure [dbo].[spInsertTechnician]
	@Nama nvarchar(50)
as
begin
	set nocount on;

	insert into [dbo].[Technician]
	(Id, Nama)
	values
	(next value for [dbo].[technicianIdSequence], @Nama);
end;
CREATE TABLE [dbo].[User]
(
	[Id] NVARCHAR(128) NOT NULL PRIMARY KEY,
	[Username] NVARCHAR(50) NOT NULL,
	[EmailAddress] NVARCHAR(256) NOT NULL, 
    [Role] VARCHAR(50) NOT NULL CHECK ([Role] IN ('Customer Service', 'Admin'))
)

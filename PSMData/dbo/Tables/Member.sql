﻿CREATE TABLE [dbo].[Member]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Nama] NVARCHAR(50) NOT NULL, 
    [NoHp] VARCHAR(12) NULL, 
    [Alamat] NVARCHAR(50) NULL, 
    [TipeHp1] NVARCHAR(50) NULL, 
    [TipeHp2] NVARCHAR(50) NULL, 
    [TipeHp3] NVARCHAR(50) NULL, 
    [TipeHp4] NVARCHAR(50) NULL, 
    [TipeHp5] NVARCHAR(50) NULL
)

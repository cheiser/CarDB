CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Brand] NVARCHAR(50) NOT NULL, 
    [Year] DATETIME NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL
)

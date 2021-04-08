CREATE TABLE [dbo].[Transaction]
(
	[Id] int identity primary key, 
    [Amount] DECIMAL NOT NULL, 
    [DateOfTransaction] DATETIME NOT NULL, 
    [UserId] NVARCHAR(128) NOT NULL
)
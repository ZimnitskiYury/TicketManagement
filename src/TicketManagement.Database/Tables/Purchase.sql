CREATE TABLE [dbo].[Purchase]
(
	[Id] int identity primary key,
	[UserId] NVARCHAR(128) NOT NULL, 
    [EventSeatId] INT NOT NULL, 
    [TransactionId] INT NULL
 )
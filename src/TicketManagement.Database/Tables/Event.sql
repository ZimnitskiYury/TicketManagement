CREATE TABLE [dbo].[Event]
(
	[Id] int primary key identity,
	[Name] nvarchar(120) NOT NULL,
	[Description] nvarchar(max) NOT NULL,
	[LayoutId] int NOT NULL,
	[DateBegin] datetime NOT NULL,
	[DateEnd] datetime NOT NULL,
	[Category] INT NOT NULL, 
    check (DateEnd is null or DateEnd>DateBegin),
)

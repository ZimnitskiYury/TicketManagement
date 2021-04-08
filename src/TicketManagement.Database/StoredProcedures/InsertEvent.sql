CREATE PROCEDURE [dbo].[Sp_InsertEvent]
@name nvarchar(120), 
@description nvarchar(max), 
@layoutId int, 
@dateBegin datetime, 
@dateEnd datetime,
@category int
AS
Begin
INSERT INTO Event (Name, Description, LayoutId, DateBegin, DateEnd, Category) VALUES (@name, @description, @layoutId, @dateBegin, @dateEnd, @category);
SELECT CAST(scope_identity() as int);
End

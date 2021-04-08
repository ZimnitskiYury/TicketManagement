CREATE PROCEDURE [dbo].[Sp_UpdateEvent] 
@id int,
@name nvarchar(120), 
@description nvarchar(max), 
@layoutId int, 
@dateBegin datetime, 
@dateEnd datetime,
@category int
AS
Begin
Update Event SET Name=@name, Description=@description, LayoutId=@layoutId, DateBegin=@dateBegin, DateEnd=@dateEnd, Category = @category WHERE Id=@id;
SELECT @@ROWCOUNT
End

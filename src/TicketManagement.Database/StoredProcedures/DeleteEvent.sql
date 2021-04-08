CREATE PROCEDURE [dbo].[Sp_DeleteEvent] 
@id int
AS
Begin
DELETE FROM Event WHERE Id=@id;
SELECT @@ROWCOUNT
End
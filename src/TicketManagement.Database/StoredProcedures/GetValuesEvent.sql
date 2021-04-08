CREATE PROCEDURE [dbo].[Sp_GetValuesEvent] 
AS
Begin
SELECT [Id],[Name],[Description],[LayoutId],[DateBegin],[DateEnd], [Category] FROM Event
End
RETURN 0

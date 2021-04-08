CREATE PROCEDURE [dbo].[Sp_GetByIdEvent]
@id int
AS
	SELECT [Id],[Name],[Description],[LayoutId],[DateBegin],[DateEnd], [Category] FROM [dbo].[Event] where Id=@id


CREATE PROCEDURE [dbo].[Sp_FilterSeatsByLayoutId]
	@layout int
AS
	SELECT Seat.Id, Seat.AreaId, Seat.Row, Seat.Number 
	FROM Area INNER JOIN 
	Seat ON Area.Id = Seat.AreaId WHERE (Area.LayoutId = @layout)

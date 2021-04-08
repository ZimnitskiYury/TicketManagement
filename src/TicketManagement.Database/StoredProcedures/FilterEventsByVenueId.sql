CREATE PROCEDURE [dbo].[Sp_FilterEventsByVenueId]
@venue int
AS
SELECT        Event.Id, Event.Name, Event.Description, Event.LayoutId, Event.DateBegin, Event.DateEnd, Event.Category
FROM            Layout INNER JOIN
                         Event ON Layout.Id = Event.LayoutId
Where Layout.VenueId=@venue

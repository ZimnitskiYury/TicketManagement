ALTER TABLE [dbo].[Purchase]  
ADD  CONSTRAINT [FK_dbo.Purchase_dbo.EventSeat] FOREIGN KEY([EventSeatId])
REFERENCES [dbo].[EventSeat] ([Id])
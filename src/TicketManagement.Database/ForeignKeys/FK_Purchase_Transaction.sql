ALTER TABLE [dbo].[Purchase]  
ADD  CONSTRAINT [FK_dbo.Purchase_dbo.Transaction] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transaction] ([Id])
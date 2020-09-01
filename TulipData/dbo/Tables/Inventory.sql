CREATE TABLE [dbo].[Inventory]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ProductId] INT NOT NULL, 
    [Quantity] INT NOT NULL, 
    [PurchasePrice] MONEY NOT NULL, 
    [PurchaseDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    CONSTRAINT [FK_Inventory_Product] FOREIGN KEY (ProductId) REFERENCES Product(Id)
)

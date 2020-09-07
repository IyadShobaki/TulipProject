CREATE PROCEDURE [dbo].[spInventory_Insert]
	@ProductId int,
	@Quantity int,
	@PurchasePrice money
AS
begin
	set nocount on;
	
	insert into dbo.Inventory (ProductId, Quantity, PurchasePrice)
	values (@ProductId, @Quantity, @PurchasePrice);
end

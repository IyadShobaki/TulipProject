using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipDataManager.Library.Internal.DataAccess;
using TulipDataManager.Library.Models;

namespace TulipDataManager.Library.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ProductData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public List<ProductModel> GetProducts()
        {
            //SqlDataAccess sql = new SqlDataAccess();

            var output = _sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TulipData");

            return output;
        }

        public void InsertProductInventory(ProductModel product, InventoryModel inventory)
        {

            //using (SqlDataAccess sql = new SqlDataAccess())
            //{
            try
            {

                _sqlDataAccess.StartTransaction("TulipData");
                int newProductId = _sqlDataAccess.CreateProductTransaction("dbo.spProduct_Insert", product);

                inventory.ProductId = newProductId;

                _sqlDataAccess.SaveDataInTransaction("dbo.spInventory_Insert", inventory);
                _sqlDataAccess.CommitTransaction();
            }
            catch
            {
                _sqlDataAccess.RollbackTransaction();
                throw;
            }
            //}
        }

        //public int InsertProduct(ProductModel product)
        //{
        //    SqlDataAccess sql = new SqlDataAccess();
        //    int newProductId = sql.CreateProduct("dbo.spProduct_Insert", product, "TulipData");

        //    return newProductId;

        //}
        //public void InsertInventory(InventoryModel inventory)
        //{
        //    SqlDataAccess sql = new SqlDataAccess();
        //    sql.SaveData("dbo.spInventory_Insert", inventory, "TulipData");

        //}



        //public void UpdateProductQuantityInStock(UpdatedQtyProductModel updatedQtyProduct)
        public void UpdateProductQuantityInStock(int productId, int newQuantity)
        {
            //SqlDataAccess sql = new SqlDataAccess();
            _sqlDataAccess.SaveData<dynamic>("dbo.spProduct_UpdateQuantity",
                new { Id = productId, @QuantityInStock = newQuantity }, "TulipData");

        }
    }
}

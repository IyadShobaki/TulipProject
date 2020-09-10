using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipDataManager.Library.Internal.DataAccess;
using TulipDataManager.Library.Models;

namespace TulipDataManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TulipData");

            return output;
        }

        public int InsertProduct(ProductModel product)
        {
            SqlDataAccess sql = new SqlDataAccess();
            int newProductId = sql.CreateProduct("dbo.spProduct_Insert", product, "TulipData");

            return newProductId;

        }
        public void InsertInventory(InventoryModel inventory)
        {
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("dbo.spInventory_Insert", inventory, "TulipData");

        }

        public void UpdateProductQuantityInStock(UpdatedQtyProductModel updatedQtyProduct)
        {
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("dbo.spProduct_UpdateQuantity", updatedQtyProduct, "TulipData");

        }
    }
}

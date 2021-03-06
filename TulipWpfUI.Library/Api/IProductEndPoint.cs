﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TulipWpfUI.Library.Models;

namespace TulipWpfUI.Library.Api
{
    public interface IProductEndPoint
    {
        Task<List<ProductModel>> GetAll();
        Task<bool> PostProductInventory(ProductModel product, InventoryModel inventory);

        //Task PostInventoryInfo(InventoryModel inventory);
        //Task<int> PostProductInfo(ProductModel product);
        //Task UpdateProductQuantity(UpdatedQtyProductModel updatedQtyProduct);
        Task UpdateProductQuantity(int productId, int newQuantity);
    }
}
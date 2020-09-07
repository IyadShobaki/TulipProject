using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TulipDataManager.Library.DataAccess;
using TulipDataManager.Library.Models;

namespace TulipDataManager.Controllers
{
    [Authorize]
    public class ProductController : ApiController
    {
        public List<ProductModel> Get()
        {
            ProductData data = new ProductData();
            return data.GetProducts();

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public int Post(ProductModel product)
        {

            ProductData data = new ProductData();
            return data.InsertProduct(product);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/Inventory")]
        public void PostInventory(InventoryModel inventory)
        {
            ProductData data = new ProductData();
            data.InsertInventory(inventory);
        }
    }

}

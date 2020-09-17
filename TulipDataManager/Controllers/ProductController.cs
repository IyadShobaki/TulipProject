using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using TulipDataManager.Library.DataAccess;
using TulipDataManager.Library.Models;

namespace TulipDataManager.Controllers
{
    [Authorize]
    public class ProductController : ApiController
    {
        private readonly IProductData _data;

        public ProductController(IProductData data)
        {
            _data = data;
        }
        [HttpGet]
        public List<ProductModel> Get()
        {
            //ProductData data = new ProductData();
            return _data.GetProducts();

        }

        [HttpPost]
        [Route("api/UpdateProductQuantity")]
        //public void PutProduct(UpdatedQtyProductModel updatedQtyProduct)
        public void PutProduct(int[] product)
        {
            int productId = product[0];
            int newQuantity = product[1];
            //ProductData data = new ProductData();
            _data.UpdateProductQuantityInStock(productId, newQuantity);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post(List<object> paramsList)
        {

            string productString = paramsList[0].ToString();
            string inventoryString = paramsList[1].ToString();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var product = (ProductModel) javaScriptSerializer.Deserialize(productString, typeof(ProductModel));
            var inventory = (InventoryModel) javaScriptSerializer.Deserialize(inventoryString, typeof(InventoryModel));
         

            //ProductData data = new ProductData();
            _data.InsertProductInventory(product, inventory);

        }
        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public int Post(ProductModel product)
        //{

        //    ProductData data = new ProductData();
        //    return data.InsertProduct(product);

        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[Route("api/Inventory")]
        //public void PostInventory(InventoryModel inventory)
        //{
        //    ProductData data = new ProductData();
        //    data.InsertInventory(inventory);
        //}
    }

}

using System.Collections.Generic;
using System.Threading.Tasks;
using TulipWpfUI.Library.Models;

namespace TulipWpfUI.Library.Api
{
    public interface IProductEndPoint
    {
        Task<List<ProductModel>> GetAll();
        Task PostInventoryInfo(InventoryModel inventory);
        Task<int> PostProductInfo(ProductModel product);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using TulipWpfUI.Library.Models;

namespace TulipWpfUI.Library.Api
{
    public interface IOrderEndPoint
    {
        Task DeleteOrder(int orderId);
        Task<bool> PostOrderDetailsInfo(List<OrderDetailModel> orderDetailModels);
        Task<int> PostOrderInfo(OrderModel order);
    }
}
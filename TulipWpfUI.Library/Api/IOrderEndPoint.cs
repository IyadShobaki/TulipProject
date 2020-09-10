using System.Threading.Tasks;
using TulipWpfUI.Library.Models;

namespace TulipWpfUI.Library.Api
{
    public interface IOrderEndPoint
    {
        Task PostOrderDetailInfo(OrderDetailModel orderDetail);
        Task<int> PostOrderInfo(OrderModel order);
    }
}
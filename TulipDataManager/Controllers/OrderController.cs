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
    public class OrderController : ApiController
    {
        [HttpPost]
        public int Post(OrderModel order)
        {

            OrderData data = new OrderData();
            return data.InsertOrder(order);

        }

        [HttpPost]
        [Route("api/OrderDetail")]
        public void PostOrderDetail(OrderDetailModel orderDetail)
        {
            OrderData data = new OrderData();
            data.InsertOrderDetail(orderDetail);
        }
    }
}

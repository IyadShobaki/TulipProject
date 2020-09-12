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

        //[HttpPost]
        //[Route("api/OrderDetail")]
        //public void Post(OrderDetailModel orderDetail)
        //{
        //    OrderData data = new OrderData();
        //    data.InsertOrderDetail(orderDetail);
        //}

        [HttpPost]
        [Route("api/OrderDetails")]
        public void Post(List<OrderDetailModel> orderDetailModels)
        {

            OrderData data = new OrderData();
            data.InsertOrderDetails(orderDetailModels);

        }

        [HttpPost]
        [Route("api/DeleteOrder")]
        public void Post(object orderId)
        {
            string _id = orderId.ToString();
            int id = int.Parse(_id);
            OrderData data = new OrderData();
            data.DeleteOrderById(id);

        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/GetOrdersReport")]
        public List<OrdersReportModel> GetOrdersReport()
        {
            OrderData data = new OrderData();
            return data.GetOrdersReport();
        }
    }
}

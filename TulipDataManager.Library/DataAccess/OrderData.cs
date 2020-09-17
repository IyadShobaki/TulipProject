using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipDataManager.Library.Internal.DataAccess;
using TulipDataManager.Library.Models;

namespace TulipDataManager.Library.DataAccess
{
    public class OrderData : IOrderData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public OrderData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public int InsertOrder(OrderModel order)
        {
            //SqlDataAccess sql = new SqlDataAccess();
            int newOrderId = _sqlDataAccess.CreateOrder("dbo.spOrder_Insert", order, "TulipData");

            return newOrderId;

        }

        //public void InsertOrderDetail(OrderDetailModel orderDetail)
        //{
        //    SqlDataAccess sql = new SqlDataAccess();
        //    sql.SaveData("dbo.spOrderDetail_Insert", orderDetail, "TulipData");

        //}


        public void InsertOrderDetails(List<OrderDetailModel> orderDetailModels)
        {
            //using (SqlDataAccess sql = new SqlDataAccess())
            //{
            try
            {

                _sqlDataAccess.StartTransaction("TulipData");

                foreach (var orderDetailModel in orderDetailModels)
                {
                    _sqlDataAccess.SaveDataInTransaction("dbo.spOrderDetail_Insert", orderDetailModel);
                }

                _sqlDataAccess.CommitTransaction();  // important
            }
            catch
            {
                _sqlDataAccess.RollbackTransaction();
                throw;
            }
            //}

        }

        public void DeleteOrderById(int orderId)
        {
            //SqlDataAccess sql = new SqlDataAccess();
            _sqlDataAccess.SaveData<dynamic>("dbo.spDeleteOrderById", new { Id = orderId }, "TulipData");
        }

        public List<OrdersReportModel> GetOrdersReport()
        {
            //SqlDataAccess sql = new SqlDataAccess();

            var output = _sqlDataAccess.LoadData<OrdersReportModel, dynamic>("dbo.spOrder_OrdersReport",
                new { }, "TulipData");

            return output;
        }
    }
}

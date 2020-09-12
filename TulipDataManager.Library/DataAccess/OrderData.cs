using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipDataManager.Library.Internal.DataAccess;
using TulipDataManager.Library.Models;

namespace TulipDataManager.Library.DataAccess
{
    public class OrderData
    {

        public int InsertOrder(OrderModel order)
        {
            SqlDataAccess sql = new SqlDataAccess();
            int newOrderId = sql.CreateOrder("dbo.spOrder_Insert", order, "TulipData");

            return newOrderId;

        }

        //public void InsertOrderDetail(OrderDetailModel orderDetail)
        //{
        //    SqlDataAccess sql = new SqlDataAccess();
        //    sql.SaveData("dbo.spOrderDetail_Insert", orderDetail, "TulipData");

        //}


        public void InsertOrderDetails(List<OrderDetailModel> orderDetailModels)
        {
            using (SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {

                    sql.StartTransaction("TulipData");

                    foreach (var orderDetailModel in orderDetailModels)
                    {
                        sql.SaveDataInTransaction("dbo.spOrderDetail_Insert", orderDetailModel);
                    }

                    sql.CommitTransaction();  // important
                }
                catch
                {
                    sql.RollbackTransaction();
                    throw;
                }
            }

        }

        public void DeleteOrderById(int orderId)
        {
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData<dynamic>("dbo.spDeleteOrderById", new { Id = orderId } , "TulipData");
        }
    }
}

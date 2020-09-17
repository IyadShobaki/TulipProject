using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipDataManager.Library.Internal.DataAccess;
using TulipDataManager.Library.Models;

namespace TulipDataManager.Library.DataAccess
{
    public class InventoryData : IInventoryData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public InventoryData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public List<InventoryDisplayModel> GetInventory()
        {
            //SqlDataAccess sql = new SqlDataAccess();
            var output = _sqlDataAccess.LoadData<InventoryDisplayModel, dynamic>("spInventory_GetAll",
                new { }, "TulipData");

            return output;
        }
    }
}

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
    public class InventoryController : ApiController
    {
        private readonly IInventoryData _data;

        public InventoryController(IInventoryData data)
        {
            _data = data;
        }
        [Authorize(Roles = "Admin")]
        public List<InventoryDisplayModel> GetInventory()
        {
            //InventoryData data = new InventoryData();

            return _data.GetInventory();
        }
    }
}

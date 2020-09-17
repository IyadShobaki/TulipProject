using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipDataManager.Library.Internal.DataAccess;
using TulipDataManager.Library.Models;

namespace TulipDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public UserData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public List<UserModel> GetUserById(string Id)
        {
            //SqlDataAccess sql = new SqlDataAccess();

            var p = new { Id = Id };

            var output = _sqlDataAccess.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TulipData");

            return output;
        }

        public void InsertUser(UserModel user)
        {
            //SqlDataAccess sql = new SqlDataAccess();
            _sqlDataAccess.SaveData("dbo.spUser_Insert", user, "TulipData");
        }
    }
}

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TulipDataManager.Library.DataAccess;
using TulipDataManager.Library.Models;
using TulipDataManager.Models;

namespace TulipDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        public UserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();

            UserData data = new UserData();

            return data.GetUserById(userId).First();
      
        }
        [HttpPost]
        public void Post(UserModel user)
        {

            UserData data = new UserData();
            data.InsertUser(user);
         
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach (var user in users)
                {
                    UserData data = new UserData();

                    var userInfo =  data.GetUserById(user.Id).First();

                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName
                    };

                    foreach (var r in user.Roles)
                    {
                        u.Roles.Add(r.RoleId, roles.Where(x => x.Id == r.RoleId).First().Name);
                    }
                    output.Add(u);
                }
            }

            return output;
        }

    }
}

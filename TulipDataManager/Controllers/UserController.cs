using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using TulipDataManager.Library.DataAccess;
using TulipDataManager.Library.Models;
using TulipDataManager.Models;

namespace TulipDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IUserData _data;

        public UserController(IUserData data)
        {
            _data = data;
        }
        [HttpGet]
        public UserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();

            //UserData data = new UserData();
            return _data.GetUserById(userId).First();

        }
        [HttpPost]
        public void Post(UserModel user)
        {

            //UserData data = new UserData();
            _data.InsertUser(user);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/GetAllUsers")]
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
                    //UserData data = new UserData();

                    var userInfo = _data.GetUserById(user.Id).First();

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


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                var roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);
                return roles;

            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/AddRole")]
        public void AddARole(UserRolePairModel pairing)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                userManager.AddToRole(pairing.UserId, pairing.RoleName);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/RemoveRole")]
        public void RemoveARole(UserRolePairModel pairing)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                userManager.RemoveFromRole(pairing.UserId, pairing.RoleName);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/CreateRole")]
        public void CreateARole(object roleName)
        {
            string role = (string)roleName;
                 
            using (var context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                roleManager.Create(new IdentityRole { Name = $"{role}" });
            }
        }

    }
}

   
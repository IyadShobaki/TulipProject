using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TulipDataManager.Models;

namespace TulipDataManager.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            // Using Postman to display the user id
            string userId = RequestContext.Principal.Identity.GetUserId();
            //var roles = new Dictionary<string, string>();

            //using (var context = new ApplicationDbContext())
            //{

            //    roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);

            //}

            //string result = "Not Admin";

            //if (roles.Keys.Contains(userId))
            //{
            //    result = "Admin";
            //}

            string result = "Not Admin";
            if (RequestContext.Principal.IsInRole("Admin"))
            {
                result = "Admin";
            }
          
            //return new string[] { "value1", "value2", userId };
            return new string[] {  userId, result };
         
        }

    }
}

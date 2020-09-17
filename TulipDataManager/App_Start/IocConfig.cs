using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using TulipDataManager.Library.DataAccess;
using TulipDataManager.Library.Internal.DataAccess;

namespace TulipDataManager
{
    public class IocConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<InventoryData>().As<IInventoryData>().InstancePerRequest();
            builder.RegisterType<OrderData>().As<IOrderData>().InstancePerRequest();
            builder.RegisterType<UserData>().As<IUserData>().InstancePerRequest();
            builder.RegisterType<ProductData>().As<IProductData>().InstancePerRequest();

            builder.RegisterType<SqlDataAccess>().As<ISqlDataAccess>().InstancePerRequest();

            
            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

           

        }
    }
}


namespace CloudMovie.APIRole
{
    using CloudMovie.APIRole.Config;
    using System;
    using System.Web.Http;
    using System.Web.Mvc;
    //using System.Web.Optimization;
    using System.Web.Routing;
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}
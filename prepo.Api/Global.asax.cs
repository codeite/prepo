using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace prepo.Api
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
          
            var container = IocConfig.ConfigureIoc(GlobalConfiguration.Configuration);
            WebApiConfig.Register(GlobalConfiguration.Configuration, container);
        }
    }
}
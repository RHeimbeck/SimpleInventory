using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Threading;
using InventoryDemo1.Service;

namespace InventoryDemo1
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private Thread workerThread;
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            DataExpirer dataExp = new DataExpirer();
            workerThread = new Thread(dataExp.CheckForExpiredData);
            workerThread.Start();
        }

        protected void Application_End()
        {
            workerThread.Abort();
        }
    }
}
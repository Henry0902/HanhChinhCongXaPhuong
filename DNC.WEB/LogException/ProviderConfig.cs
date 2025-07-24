using System; 
using System.IO; 
using System.Web;

namespace DNC.WEB.LogException
{
    public class ProviderConfig
    {
        /// <summary>
        /// thanhtv
        /// </summary>
        public void Config()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(HttpContext.Current.Server.MapPath("~/Web.config")));
        }
    }
}
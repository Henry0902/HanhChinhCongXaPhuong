
using System.Web.Mvc;

namespace SV.Website.Controllers
{ 
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult _MainMenu()
        {
            return PartialView();
        }
    }
}
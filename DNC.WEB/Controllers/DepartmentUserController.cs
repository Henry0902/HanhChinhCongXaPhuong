using System;
using System.Net;
using System.Web.Mvc;
using DNC.WEB.Models;
using DNC.WEB.Repository;
//using log4net;
//using SV.Website.LogException;

namespace SV.Website.Controllers
{
    //[Authorize]
    //[SessionTimeout]
    public class DepartmentUsersController : Controller
    {
        GenericRepository<DepartmentUsers> _context = null;
        private DepartmentUsersRp _obj = null;
        //
        // GET: /DepartmentUsers/

        [HttpGet]
        public ActionResult DepartmentUsers_GetByUserId(int id)
        {
            _obj = new DepartmentUsersRp();

            return Json(_obj.GetByUserId(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Delete(int id)
        {
            _obj = new DepartmentUsersRp();
            _obj.Delete(id);
        }

        [HttpPost]
        public ActionResult Create(DepartmentUsers model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DepartmentUsers>();
                model.UserId = model.UserId;
                model.DepartmentId = model.DepartmentId; 

                var data = _context.Insert(model);
                _context.Save();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // Hàm Dispose Connection
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
            base.Dispose(disposing);
        }

	}
}
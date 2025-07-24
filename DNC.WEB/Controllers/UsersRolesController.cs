using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using VTLT_DNC.Common;
using DNC.WEB.Models;
using DNC.WEB.Repository;
//using log4net;
//using VTLT_DNC.LogException;

namespace DNC.WEB.Website.Controllers
{
    //[SessionTimeout]
    public class UsersRolesController : Controller
    {
        GenericRepository<UsersRoles> _context = null;
        private UserRoleRp _obj = null;
        private DbConnectContext _db = new DbConnectContext();
        //
        // GET: /UsersRoles/
       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserRole_GetByUserId(int id)
        {
            _obj = new UserRoleRp();

            return Json(_obj.getRolesbyUserID(id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Create(UsersRoles model)
        {
            var session = Session["Users"];
            using (var trans = _db.Database.BeginTransaction())
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context = new GenericRepository<UsersRoles>();
                        var data = _context.Insert(model);
                        _context.Save();
                        trans.Commit();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public void Delete(int id)
        {
            using (var trans = _db.Database.BeginTransaction())
                try
                {
                    _obj = new UserRoleRp();
                    _obj.DeletebyUserId(id);
                }
                catch (Exception)
                {
                    trans.Rollback();
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
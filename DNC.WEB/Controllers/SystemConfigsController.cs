using System;
using System.Net;
using System.Web.Mvc;
using DNC.WEB.Models;
using DNC.WEB.Repository;
using DNC.CM;

namespace DNC.WEB.Controllers
{
    //[SessionTimeout]
    public class SystemConfigsController : Controller
    { 
        GenericRepository<SystemConfigs> _context = null;
        private SystemConfigsRp _obj = null;
        private DbConnectContext _db = new DbConnectContext();
        Ultils logs = new Ultils();
        
        public ActionResult Index()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index?url_redirect=" + Request.Url.AbsoluteUri);
            }
            return View();
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<SystemConfigs>();
                return Json(_context.GetAll(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " thiết lập " +logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "SystemConfig/GetAll");
            }
            return HttpNotFound();
        } 
        [HttpGet]
        public ActionResult GetById(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<SystemConfigs>();
                var data = _context.Get(id);
                if (data != null)
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " thiết lập " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "SystemConfig/GetById");
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Create(SystemConfigs model)
        {
            var session = Session["Users"];
            using (var trans = _db.Database.BeginTransaction())
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<SystemConfigs>();
                    model.CreatedDate = DateTime.Now;
                     model.CreatedBy = 1; 
                    var data = _context.Insert(model);
                    _context.Save();
                    trans.Commit();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " thiết lập " + model.Name + logs.NotifySuccess, (int)ActionType.ADD, "SystemConfigs/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                trans.Rollback();
                logs.SystemLog_Insert_Error((int)session, logs.ActionCreate, " thiết lập " + model.Name + logs.NotifyError, (int)ActionType.ADD, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "SystemConfig/Create");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpPost]
        public ActionResult Update(SystemConfigs model)
        {
            var session = Session["Users"];
            using (var trans = _db.Database.BeginTransaction())
            try
            {
                _context = new GenericRepository<SystemConfigs>();
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = (int)session;
                _context.Update(model);
                _context.Save();
                trans.Commit();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " thiết lập " + model.Name + logs.NotifySuccess, (int)ActionType.UPDATE, "SystemConfigs/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " thiết lập " + model.Name + logs.NotifyError, (int)ActionType.UPDATE, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "SystemConfig/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // Hàm Delete
        public void Delete(int id)
        {
            var session = Session["Users"];
            SystemConfigs obj = _context.Get(id);
            using (var trans = _db.Database.BeginTransaction())

            try
            {
                var info = _db.SystemConfig.Find(id);
                _context = new GenericRepository<SystemConfigs>();
                _context.Delete(id);
                _context.Save();
                trans.Commit();
                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " thiết lập " + obj.Name + logs.NotifySuccess, (int)ActionType.DELETE, "SystemConfigs/Delete");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " thiết lập"+ obj.Name + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "SystemConfigs/Delete");
            }
        }
        public ActionResult GetIndex(string keyword, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new SystemConfigsRp();
                return Json(_obj.SearchAll(keyword, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " thiết lập " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "SystemConfig/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
         
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
using System;
using System.Net;
using System.Web.Mvc;
using DNC.CM;
using DNC.WEB.Models;
using DNC.WEB.Repository;
using DNC.WEB.LogException;
using System.Globalization;

namespace DNC.WEB.Controllers
{
    //[SessionTimeout]
    public class SystemLogsController : Controller
    {
        //
        // GET: /SystemLogs/
        GenericRepository<SystemLogs> _context = null;
        private SystemLogsRp _obj = null;
        private DbConnectContext _db = new DbConnectContext();
        private Ultils logs;
       
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
                _context = new GenericRepository<SystemLogs>();
                return Json(_context.GetAll(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "SystemLogs/GetAll");
            }
            return HttpNotFound();
        }

        [HttpGet]
         
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<SystemLogs>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Create(SystemLogs model)
        {
            var session = Session["Users"];
            using(var trans = _db.Database.BeginTransaction())
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<SystemLogs>();
                    model.UserName = model.UserName;
                    model.ActionName = model.ActionName;
                    model.ActionDate = DateTime.Now;
                    model.IPAddress = model.IPAddress;
                    model.MacAddress = model.MacAddress;
                    model.Type = model.Type;
                    model.Message = model.Message;
                    model.InnerException = model.InnerException;
                    model.UserAgent = model.UserAgent;
                    model.RawURL = model.RawURL;
                    model.Method = model.Method; 
                    _context = new GenericRepository<SystemLogs>();
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " nhật ký hệ thống " + model.ActionName + logs.NotifySuccess, (int)ActionType.ADD, "SystemLogs/Create");
                    trans.Commit();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionCreate, logs.NotifyError, (int)ActionType.ADD, ex.Message,
                              ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "SystemConfigs/Create");
                trans.Rollback();
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }  
        // Hàm Delete
        public void Delete(int id)
        {
            var session = Session["Users"];
            using (var trans = _db.Database.BeginTransaction())
            try
            {
                var info = _db.SystemLogs.Find(id);
                _context = new GenericRepository<SystemLogs>();
                _context.Delete(id);
                _context.Save();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
            }
        }

        // Gọi đến Respo để trả về dữ liệu
        public ActionResult GetIndex(string keyword, int type, int status, string dateFrom, string dateTo, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                if(dateFrom == "")
                {
                    dateFrom = "01/01/1970";
                }
                if(dateTo == "")
                {
                    dateTo = "01/12/2500";
                }
                string sDateFrom = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                string sDateTo = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                _obj = new SystemLogsRp();
                return Json(_obj.SearchAll(keyword, type, status, sDateFrom, sDateTo, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message,
                             ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "SystemConfigs/GetIndex");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpGet]
        public void DeleteByTime(int time)
        {
            var session = Session["Users"];
            using (var trans =_db.Database.BeginTransaction())
            try
            {
                _obj = new SystemLogsRp();
                _obj.DeleteByTime(time);
                trans.Commit();
            }
            catch (Exception ex)
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
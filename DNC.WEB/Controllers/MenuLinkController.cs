using DNC.CM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DNC.WEB.Models;
using DNC.WEB.Repository;

namespace DNC.WEB.Controllers
{
    public class MenuLinkController : Controller
    {
        GenericRepository<Pages> _context = null;
        private DbConnectContext _db = new DbConnectContext();
        Ultils logs = new Ultils();
        //
        // GET: /MenuLink/
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
            _context = new GenericRepository<Pages>();
            return Json(_context.GetAll().OrderBy(x => x.ParentId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllLevel()
        {
            _context = new GenericRepository<Pages>();
            return Json(_context.GetAll().Where(x => x.ParentId == 0), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<Pages>();
            var data = _context.Get(id);
            if(data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
            //return Json(_context.GetAll().Where(x => x.ParentId == 0), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(Pages model)
        {
            var session = Session["Users"];
            using (var trans = _db.Database.BeginTransaction())
                try
                {
                    //if (ModelState.IsValid)
                    //{
                        _context = new GenericRepository<Pages>();
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = (int)session;
                        var data = _context.Insert(model);
                        _context.Save();
                        trans.Commit();
                        logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, "Trang " + model.Name + logs.NotifySuccess, (int)ActionType.ADD, "MenuLink/Create");
                        return Json(data, JsonRequestBehavior.AllowGet);
                    //}
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    logs.SystemLog_Insert_Error((int)session, logs.ActionCreate, "Trang" + model.Name + logs.NotifyError, (int)ActionType.ADD, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "MenuLink/Create");
                }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpPost]
        public ActionResult Update(Pages model)
        {
            var session = Session["Users"];
            using (var trans = _db.Database.BeginTransaction())
                try
                {
                    //if (ModelState.IsValid)
                    //{
                        _context = new GenericRepository<Pages>();
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = (int)session;
                        _context.Update(model);
                        _context.Save();
                        trans.Commit();
                        logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, "Trang " + model.Name + logs.NotifySuccess, (int)ActionType.ADD, "MenuLink/Update");
                        return Json(model, JsonRequestBehavior.AllowGet);
                    //}
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, "Trang" + model.Name + logs.NotifyError, (int)ActionType.ADD, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "MenuLink/Update");
                }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // Hàm Delete
        public void Delete(int id)
        {
            var session = Session["Users"];
            //LinhVuc model = _context.Get(id);
            using (var trans = _db.Database.BeginTransaction())
                try
                {
                    var info = _db.Pages.Find(id);
                    if (info != null)
                    {
                        _context = new GenericRepository<Pages>();
                        _context.Delete(id);
                        _context.Save();
                        trans.Commit();
                        logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " MenuLink " + id + logs.NotifySuccess, (int)ActionType.DELETE, "MenuLink/Delete");
                    }
                 }
                catch (Exception ex)
                {
                    trans.Rollback();
                    logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " MenuLink " + id + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "MenuLink/Delete");
                }

        }
	}
}
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
    public class FunctionController : Controller
    {
        //
        // GET: /Function/

        DbConnectContext _db = new DbConnectContext();
        Ultils logs = new Ultils();
        GenericRepository<PageFunctions> _context = null;

        FunctionRp _obj = new FunctionRp();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll()
        {
            var data = _db.Functions.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetByPageId(int PageId)
        {
            return Json(_obj.GetByPageId(PageId), JsonRequestBehavior.AllowGet);
        }

        // Update PageFunction
        [HttpPost]
        public ActionResult UpdatePageFunction(int PageId, string FunctionId)
        {
            var session = Session["Users"];
            try
            {
                int id = _obj.update(PageId, FunctionId);

                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, "PageFunction " + PageId + logs.NotifySuccess, (int)ActionType.UPDATE, "Function/UpdatePageFunction");

                return Json(id, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionCreate, "PageFunction " + PageId + logs.NotifyError, (int)ActionType.ADD, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "Function/UpdatePageFunction");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // Create PageFunction
        [HttpPost]
        public ActionResult CreatePageFunction(PageFunctions model)
        {
            var session = Session["Users"];
            using (var trans = _db.Database.BeginTransaction())
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context = new GenericRepository<PageFunctions>();
                        var data = _context.Insert(model);
                        _context.Save();
                        trans.Commit();
                        logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " PageFunction " + model.Id + logs.NotifySuccess, (int)ActionType.ADD, "Function/CreatePageFunction");
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    logs.SystemLog_Insert_Error((int)session, logs.ActionCreate, " PageFunction " + model.Id + logs.NotifyError, (int)ActionType.ADD, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "Function/CreatePageFunction");
                }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public void DeleteByPageId(int id)
        {
            _context = new GenericRepository<PageFunctions>();
            var session = Session["Users"];
            try
            {
                _obj.DeleteByPageId(id);
                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " PageFunction " + id + logs.NotifySuccess, (int)ActionType.DELETE, "Function/DeleteByPageId");
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " PageFunction " + id + logs.NotifyError, (int)ActionType.DELETE, ex.Message,
                    ex.InnerException.ToString(), "", "", "Function/DeleteByPageId");
            }
        }
	}
}
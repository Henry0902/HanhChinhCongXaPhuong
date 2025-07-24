using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DNC.CM;
using DNC.WEB.Common;
using DNC.WEB.Models;
using DNC.WEB.Repository;
namespace DNC.WEB.Controllers
{
    public class DmQuanHuyenController : Controller
    {
        //
        // GET: /DmQuanHuyen/

        GenericRepository<DmQuanHuyen> _context = null;
        private DmQuanHuyenRp _obj = null;
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmQuanHuyen>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmQuanHuyen/GetAll");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult GetQuanHuyenByTinhThanh(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmQuanHuyen>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE && x.idTinhThanh == id).OrderBy(x => x.NgayTao), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmQuanHuyen/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string thongtintimkiem, int TrangThai, string order, int pageSize, int pageIndex, int idTinhThanh)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DmQuanHuyenRp();
                return Json(_obj.SearchAll(thongtintimkiem, TrangThai, order, pageSize, pageIndex, idTinhThanh), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " quận huyện " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmQuanHuyen/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<DmQuanHuyen>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Create(DmQuanHuyen model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<DmQuanHuyen>();
                    model.NgayTao = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " quận huyện " + model.TenQuanHuyen + logs.NotifySuccess, (int)ActionType.ADD, "DmQuanHuyen/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " quận huyện " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới quận huyện", "Lỗi tạo mới quận huyện", "", "", "DmQuanHuyen/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult Update(DmQuanHuyen model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmQuanHuyen>();
                model.NgayTao = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " quận huyện " + model.TenQuanHuyen + logs.NotifySuccess, (int)ActionType.UPDATE, "DmQuocTichuAn/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " quận huyện " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật quận huyện", "Lỗi cập nhật quận huyện", "", "", "DmQuanHuyen/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult Delete(int id)
        {
            _obj = new DmQuanHuyenRp();
            var session = Session["Users"];
            int check = _obj.check_delete(id);
            if (check > 0)
            {
                var data = new Delete { Messeger = 1 };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    _context = new GenericRepository<DmQuanHuyen>();
                    var model = _context.Get(id);
                    _context.Delete(id);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " quận huyện " + model.TenQuanHuyen + logs.NotifySuccess, (int)ActionType.DELETE, "DmQuanHuyen/Delete");
                    var data = new Delete { Messeger = 2 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " quận huyện " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DmQuanHuyen/Delete");
                    var data = new Delete { Messeger = 3 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult UpdateStatus(int Id, int Status)
        {
            _obj = new DmQuanHuyenRp();
            try
            {
                _obj.UpdateStatus(Id, Status);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

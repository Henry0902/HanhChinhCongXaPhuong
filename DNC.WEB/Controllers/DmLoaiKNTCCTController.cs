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
    public class DmLoaiKNTCCTController : Controller
    {
        //
        // GET: /DmLoaiKNTCCT/

        GenericRepository<DmLoaiKNTCCT> _context = null;
        private DmLoaiKNTCCTRp _obj = null;
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
                _context = new GenericRepository<DmLoaiKNTCCT>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmLoaiKNTCCT/GetAll");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult GetLoaiKNTCCTByLoaiKNTC(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmLoaiKNTCCT>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE && x.IdLoaiKNTC == id).OrderBy(x => x.NgayTao), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmLoaiKNTC/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string thongtintimkiem, int TrangThai, string order, int pageSize, int pageIndex, int idTinhThanh, int idTinh)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DmLoaiKNTCCTRp();
                return Json(_obj.SearchAll(thongtintimkiem, TrangThai, order, pageSize, pageIndex, idTinhThanh, idTinh), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " loại tckn chi tiết " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmLoaiKNTCCT/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _obj = new DmLoaiKNTCCTRp();
            _context = new GenericRepository<DmLoaiKNTCCT>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(_obj.SearchById(id)[0], JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Create(DmLoaiKNTCCT model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<DmLoaiKNTCCT>();
                    model.NgayTao = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " loại tckn chi tiết " + model.TenLoaiKNTCChiTiet + logs.NotifySuccess, (int)ActionType.ADD, "DmLoaiKNTCCT/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " loại tckn chi tiết " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới loại tckn chi tiết", "Lỗi tạo mới loại tckn chi tiết", "", "", "DmLoaiKNTCCT/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult Update(DmLoaiKNTCCT model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmLoaiKNTCCT>();
                model.NgayTao = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " loại tckn chi tiết " + model.TenLoaiKNTCChiTiet + logs.NotifySuccess, (int)ActionType.UPDATE, "DmLoaiKNTCCT/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " loại tckn chi tiết " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật loại tckn chi tiết", "Lỗi cập nhật loại tckn chi tiết", "", "", "DmLoaiKNTCCT/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public void Delete(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmLoaiKNTCCT>();
                var model = _context.Get(id);
                _context.Delete(id);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " loại tckn chi tiết " + model.TenLoaiKNTCChiTiet + logs.NotifySuccess, (int)ActionType.DELETE, "DmLoaiKNTCCT/Delete");
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " loại tckn chi tiết " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DmLoaiKNTCCT/Delete");
            }
        }

        public ActionResult UpdateStatus(int Id, int Status)
        {
            _obj = new DmLoaiKNTCCTRp();
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
        public ActionResult getQuanHuyen(int id)
        {
            _obj = new DmLoaiKNTCCTRp();
            return Json(_obj.SearchID(id), JsonRequestBehavior.AllowGet);

        }

    }
}

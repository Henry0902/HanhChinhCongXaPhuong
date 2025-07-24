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
    public class DmLoaiDoiTuongController : Controller
    {
        // GET: /DmLoaiDoiTuong/

        GenericRepository<DmLoaiDoiTuong> _context = null;
        private DmLoaiDoiTuongRp _obj = null;
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
                _context = new GenericRepository<DmLoaiDoiTuong>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmQuocTich/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string thongtintimkiem, int TrangThai, string order, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DmLoaiDoiTuongRp();
                return Json(_obj.SearchAll(thongtintimkiem, TrangThai, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " quốc tich " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmQuocTich/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<DmLoaiDoiTuong>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Create(DmLoaiDoiTuong model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<DmLoaiDoiTuong>();
                    model.NgayTao = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " quốc tich " + model.TenDoiTuong + logs.NotifySuccess, (int)ActionType.ADD, "DmQuocTich/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " quốc tich " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới quốc tich", "Lỗi tạo mới quốc tich", "", "", "DmQuocTich/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult Update(DmLoaiDoiTuong model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmLoaiDoiTuong>();
                model.NgayTao = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " quốc tich " + model.TenDoiTuong + logs.NotifySuccess, (int)ActionType.UPDATE, "DmQuocTichuAn/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " quốc tich " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật quốc tich", "Lỗi cập nhật quốc tich", "", "", "DmQuocTich/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public void Delete(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmLoaiDoiTuong>();
                var model = _context.Get(id);
                _context.Delete(id);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " quốc tich " + model.TenDoiTuong + logs.NotifySuccess, (int)ActionType.DELETE, "DmQuocTich/Delete");
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " quốc tich " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DmQuocTich/Delete");
            }
        }

        public ActionResult UpdateStatus(int Id, int Status)
        {
            _obj = new DmLoaiDoiTuongRp();
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

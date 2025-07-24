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
    public class DmHuongXuLyController : Controller
    {
        GenericRepository<DmHuongXuLy> _context = null;
        private DmHuongXuLyRp _obj = null;
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
                _context = new GenericRepository<DmHuongXuLy>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmHuongXuLy/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string thongtintimkiem, int TrangThai, string order, int pageSize, int pageIndex, int Loai)
        {
            try
            {
                _obj = new DmHuongXuLyRp();
                return Json(_obj.SearchAll(thongtintimkiem, TrangThai, order, pageSize, pageIndex, Loai), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error(-1, logs.ActionLoadData, "Lỗi lấy dữ liệu DmHuongXuLy", (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmHuongXuLy/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<DmHuongXuLy>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Create(DmHuongXuLy model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<DmHuongXuLy>();
                    // Check if the name already exists
                    var existingEntity = _context.GetAll().FirstOrDefault(x => x.TenHuongXuLy == model.TenHuongXuLy);
                    if (existingEntity != null)
                    {
                        return Json(new { success = false, message = "Tên hướng xử lý đã tồn tại." }, JsonRequestBehavior.AllowGet);
                    }
                    model.MaNguoiTao = (int)session;
                    model.NgayTao = DateTime.Now;
                    model.ThoiDiemCapNhat = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, "DmHuongXuLy " + model.TenHuongXuLy + logs.NotifySuccess, (int)ActionType.ADD, "DmHuongXuLy/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, "DmHuongXuLy " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới DmHuongXuLy", "Lỗi tạo mới DmHuongXuLy", "", "", "DmHuongXuLy/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Update(DmHuongXuLy model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmHuongXuLy>();
                model.MaNguoiThayDoi = (int)session;
                model.ThoiDiemCapNhat = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, "DmHuongXuLy " + model.TenHuongXuLy + logs.NotifySuccess, (int)ActionType.UPDATE, "DmHuongXuLy/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, "DmHuongXuLy " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật DmHuongXuLy", "Lỗi cập nhật DmHuongXuLy", "", "", "DmHuongXuLy/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult Delete(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmHuongXuLy>();
                var model = _context.Get(id);
                _context.Delete(id);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, "DmHuongXuLy " + model.TenHuongXuLy + logs.NotifySuccess, (int)ActionType.DELETE, "DmHuongXuLy/Delete");
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, "DmHuongXuLy " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DmHuongXuLy/Delete");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult UpdateStatus(int Id, int Status)
        {
            _obj = new DmHuongXuLyRp();
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

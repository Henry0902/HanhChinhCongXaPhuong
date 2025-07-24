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
using Microsoft.Ajax.Utilities;

namespace DNC.WEB.Controllers
{
    public class DmPhienTiepCongDanController : Controller
    {
        GenericRepository<DmPhienTiepCongDan> _context = null;
        private DmPhienTiepCongDanRp _obj = null;
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll()
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmPhienTiepCongDan>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmPhienTiepCongDan/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string thongtintimkiem, int TrangThai, int IdDonVi, int KieuTiep, int Loai, string order, int pageSize, int pageIndex)
        {
            try
            {
                _obj = new DmPhienTiepCongDanRp();
                return Json(_obj.SearchAll(thongtintimkiem, TrangThai, IdDonVi, KieuTiep, Loai, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error(-1, logs.ActionLoadData, "Lỗi lấy dữ liệu phiên tiếp công dân", (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmPhienTiepCongDan/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<DmPhienTiepCongDan>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        //lấy ra phiên tiếp công dân theo IdDonVi và Phiên tiếp công dân thuộc tất cả đơn vị

        [HttpGet]
        public ActionResult GetPhienTCDByDonViIncludingAll(int IdDonVi)
        {
            _obj = new DmPhienTiepCongDanRp();
            return Json(_obj.GetPhienTCDByDonViIncludingAll(IdDonVi), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Create(DmPhienTiepCongDan model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<DmPhienTiepCongDan>();
                    // Check if the name already exists
                    var existingEntity = _context.GetAll().FirstOrDefault(x => x.TenPhienTCD == model.TenPhienTCD);
                    if (existingEntity != null)
                    {
                        return Json(new { success = false, message = "Tên phiên tiếp công dân đã tồn tại." }, JsonRequestBehavior.AllowGet);
                    }
                    model.MaNguoiTao = (int)session;
                    model.NgayTao = DateTime.Now;
                    model.ThoiDiemCapNhat = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " phiên tiếp công dân " + model.TenPhienTCD + logs.NotifySuccess, (int)ActionType.ADD, "DmPhienTiepCongDan/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " phiên tiếp công dân " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới phiên tiếp công dân", "Lỗi tạo mới phiên tiếp công dân", "", "", "DmPhienTiepCongDan/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Update(DmPhienTiepCongDan model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmPhienTiepCongDan>();
                model.MaNguoiThayDoi = (int)session;
                model.ThoiDiemCapNhat = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " phiên tiếp công dân " + model.TenPhienTCD + logs.NotifySuccess, (int)ActionType.UPDATE, "DmPhienTiepCongDan/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " phiên tiếp công dân " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật phiên tiếp công dân", "Lỗi cập nhật phiên tiếp công dân", "", "", "DmPhienTiepCongDan/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult Delete(int id)
        {
            _obj = new DmPhienTiepCongDanRp();
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmPhienTiepCongDan>();
                var model = _context.Get(id);
                _context.Delete(id);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " phiên tiếp công dân " + model.TenPhienTCD + logs.NotifySuccess, (int)ActionType.DELETE, "DmPhienTiepCongDan/Delete");
                var data = new Delete { Messeger = 2 };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " phiên tiếp công dân " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DmPhienTiepCongDan/Delete");
                var data = new Delete { Messeger = 3 };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateStatus(int Id, int Status)
        {
            _obj = new DmPhienTiepCongDanRp();
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

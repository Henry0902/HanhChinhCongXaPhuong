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
    public class DmDiaDiemTiepDanController : Controller
    {
        //
        // GET: /DmDiaDiemTiepDan/Index

        GenericRepository<DmDiaDiemTiepDan> _context = null;
        private DmDiaDiemTiepDanRp _obj = null;
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        public ActionResult GetAll()
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmDiaDiemTiepDan>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmDiaDiemTiepDan/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string thongtintimkiem, int IdDonVi, string DiaChi, int TrangThai, string order, int pageSize, int pageIndex)
        {
            try
            {
                
                _obj = new DmDiaDiemTiepDanRp();

                return Json(_obj.SearchAll(thongtintimkiem, IdDonVi, DiaChi, TrangThai, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error(-1, logs.ActionLoadData, "Lỗi lấy dữ liệu phiên tiếp dân", (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmDiaDiemTiepDan/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<DmDiaDiemTiepDan>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

       


        [HttpPost]
        public ActionResult Create(DmDiaDiemTiepDan model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<DmDiaDiemTiepDan>();
                    // Check if the DiaChi already exists
                    var existingEntity = _context.GetAll().FirstOrDefault(x => x.DiaChi == model.DiaChi);
                    if (existingEntity != null)
                    {
                        return Json(new { success = false, message = "Địa chỉ đã tồn tại." }, JsonRequestBehavior.AllowGet);
                    }
                    //model.Creator = (int)session;
                    model.NgayTao = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " địa điểm tiếp dân " + model.Ten + logs.NotifySuccess, (int)ActionType.ADD, "DmDiaDiemTiepDan/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " địa điểm tiếp dân " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới địa điểm tiếp dân", "Lỗi tạo mới địa điểm tiếp dân", "", "", "DmDiaDiemTiepDan/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult Update(DmDiaDiemTiepDan model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmDiaDiemTiepDan>();
                //model.Creator = (int)session;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " địa điểm tiếp dân " + model.Ten + logs.NotifySuccess, (int)ActionType.UPDATE, "DmDiaDiemTiepDan/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " địa điểm tiếp dân " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi tạo mới địa điểm tiếp dân", "Lỗi tạo mới địa điểm tiếp dân", "", "", "DmDiaDiemTiepDan/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult Delete(int id )
        {
            _obj = new DmDiaDiemTiepDanRp();
            var session = Session["Users"];
                try
                {
                    _context = new GenericRepository<DmDiaDiemTiepDan>();
                    var model = _context.Get(id);
                    _context.Delete(id);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " địa điểm tiếp dân " + model.Ten + logs.NotifySuccess, (int)ActionType.DELETE, "DmDiaDiemTiepDan/Delete");
                    var data = new Delete { Messeger = 2 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " địa điểm tiếp dân " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DmDiaDiemTiepDan/Delete");
                    var data = new Delete { Messeger = 3 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
        }

        public ActionResult UpdateStatus(int Id, int Status,string DiaChi)
        {
            _obj = new DmDiaDiemTiepDanRp();
            try
            {
                _obj.UpdateStatus(Id, Status, DiaChi);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

    }
}

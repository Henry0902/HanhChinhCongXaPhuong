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
    public class DmLoaiKNTCController : Controller
    {
        //
        // GET: /DmLoaiKNTC/

        GenericRepository<DmLoaiKNTC> _context = null;
        private DmLoaiKNTCRp _obj = null;
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
                _context = new GenericRepository<DmLoaiKNTC>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmLoaiKNTC/GetAll");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult GetLoaiKNTCByLoaiDonThu(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmLoaiKNTC>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE && x.IdLoaiDonThu == id).OrderBy(x => x.NgayTao), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmLoaiKNTC/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string thongtintimkiem,string Code, int TrangThai, string order, int pageSize, int pageIndex, int IdLoaiDonThu,int IdNguonDonThu,int Loai)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DmLoaiKNTCRp();
                return Json(_obj.SearchAll(thongtintimkiem,Code, TrangThai, order, pageSize, pageIndex, IdLoaiDonThu, IdNguonDonThu, Loai), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " loại kntc " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmLoaiKNTC/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        //Lấy danh sách loại KNTC theo nguồn hồ sơ
        //[HttpGet]
        //public ActionResult GetLoaiKNTCByNguonDonThu(int id)
        //{
        //    var session = Session["Users"];
        //    try
        //    {
        //        _context = new GenericRepository<DmLoaiKNTC>();
        //        return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE && x.IdNguonDonThu == id).OrderBy(x => x.NgayTao), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)

        //    {
        //        logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmLoaiKNTC/GetAll");
        //    }
        //    return HttpNotFound();
        //}

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<DmLoaiKNTC>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Create(DmLoaiKNTC model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<DmLoaiKNTC>();
                    model.NgayTao = DateTime.Now;
                    model.ThoiDiemCapNhat = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " loại kntc " + model.TenLoaiKNTC + logs.NotifySuccess, (int)ActionType.ADD, "DmLoaiKNTC/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " loại kntc " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới loại kntc", "Lỗi tạo mới loại kntc", "", "", "DmLoaiKNTC/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult Update(DmLoaiKNTC model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmLoaiKNTC>();
                model.NgayTao = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " loại kntc " + model.TenLoaiKNTC + logs.NotifySuccess, (int)ActionType.UPDATE, "DmLoaiKNTC/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " loại kntc " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật loại kntc", "Lỗi cập nhật loại kntc", "", "", "DmLoaiKNTC/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult Delete(int id)
        {
            _obj = new DmLoaiKNTCRp();
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
                    _context = new GenericRepository<DmLoaiKNTC>();
                    var model = _context.Get(id);
                    _context.Delete(id);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " loại kntc " + model.TenLoaiKNTC + logs.NotifySuccess, (int)ActionType.DELETE, "DmLoaiKNTC/Delete");
                    var data = new Delete { Messeger = 2 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " loại kntc " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DmLoaiKNTC/Delete");
                    var data = new Delete { Messeger = 3 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult UpdateStatus(int Id, int Status)
        {
            _obj = new DmLoaiKNTCRp();
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

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
    public class DmLoaiDonThuController : Controller
    {
        //
        // GET: /DmLoaiDonThu/

        GenericRepository<DmLoaiDonThu> _context = null;
        private DmLoaiDonThuRp _obj = null;
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
                _context = new GenericRepository<DmLoaiDonThu>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmLoaiDonThu/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string thongtintimkiem, int TrangThai, string order, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DmLoaiDonThuRp();
                return Json(_obj.SearchAll(thongtintimkiem, TrangThai, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " hồ sơ " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmLoaiDonThu/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<DmLoaiDonThu>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Create(DmLoaiDonThu model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<DmLoaiDonThu>();
                    model.NgayTao = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " hồ sơ " + model.TenLoaiDonThu + logs.NotifySuccess, (int)ActionType.ADD, "DmLoaiDonThu/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " hồ sơ " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới hồ sơ", "Lỗi tạo mới hồ sơ", "", "", "DmLoaiDonThu/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult Update(DmLoaiDonThu model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmLoaiDonThu>();
                model.NgayTao = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " hồ sơ " + model.TenLoaiDonThu + logs.NotifySuccess, (int)ActionType.UPDATE, "DmLoaiDonThu/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " hồ sơ " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật hồ sơ", "Lỗi cập nhật hồ sơ", "", "", "DmLoaiDonThu/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult Delete(int id)
        {
            _obj = new DmLoaiDonThuRp();
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
                    _context = new GenericRepository<DmLoaiDonThu>();
                    var model = _context.Get(id);
                    _context.Delete(id);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " hồ sơ " + model.TenLoaiDonThu + logs.NotifySuccess, (int)ActionType.DELETE, "DmLoaiDonThu/Delete");
                    var data = new Delete { Messeger = 2 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " hồ sơ " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DmLoaiDonThu/Delete");
                    var data = new Delete { Messeger = 3 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult UpdateStatus(int Id, int Status)
        {
            _obj = new DmLoaiDonThuRp();
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
        public ActionResult getAllDropDown()
        {
            GenericRepository<Users> _context3 = new GenericRepository<Users>();
            var data = _context3.Get((int)Session["Users"]);
            var id = data.DepartmentId;
            _obj = new DmLoaiDonThuRp();
            return Json(_obj.getAllDepartmentLevel(id), JsonRequestBehavior.AllowGet);
        }

    }
}

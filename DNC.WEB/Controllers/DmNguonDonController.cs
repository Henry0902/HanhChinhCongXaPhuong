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
    public class DmNguonDonController : Controller
    {

        //Get : /DmNguonDon/Index

        GenericRepository<DmNguonDon> _context = null;
        private DmNguonDonRp _obj = null;
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
                _context = new GenericRepository<DmNguonDon>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmNguonDon/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string thongtintimkiem, int TrangThai, string order, int pageSize, int pageIndex, int Loai)
        {
            try
            {
                _obj = new DmNguonDonRp();
                return Json(_obj.SearchAll(thongtintimkiem, TrangThai, order, pageSize, pageIndex,Loai), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error(-1, logs.ActionLoadData, "Lỗi lấy dữ liệu nguồn hồ sơ", (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmNguonDon/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }



        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<DmNguonDon>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }




        [HttpPost]
        public ActionResult Create(DmNguonDon model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<DmNguonDon>();
                    // Check if the TenNguonDon already exists
                    var existingEntity = _context.GetAll().FirstOrDefault(x => x.TenNguonDon == model.TenNguonDon);
                    if (existingEntity != null)
                    {
                        return Json(new { success = false, message = "Nguồn hồ sơ đã tồn tại." }, JsonRequestBehavior.AllowGet);
                    }
                    model.MaNguoiTao = (int)session;
                    model.NgayTao = DateTime.Now;
                    model.ThoiDiemCapNhat = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " nguồn hồ sơ " + model.TenNguonDon + logs.NotifySuccess, (int)ActionType.ADD, "DmNguonDon/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " nguồn hồ sơ " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới nguồn hồ sơ", "Lỗi tạo mới nguồn hồ sơ", "", "", "DmNguonDon/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult Update(DmNguonDon model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmNguonDon>();
                model.MaNguoiThayDoi = (int)session;
                model.ThoiDiemCapNhat = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " nguồn hồ sơ " + model.TenNguonDon + logs.NotifySuccess, (int)ActionType.UPDATE, "DmNguonDon/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " nguồn hồ sơ " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật nguồn hồ sơ", "Lỗi cập nhật nguồn hồ sơ", "", "", "DmNguonDon/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult Delete(int id)
        {
            _obj = new DmNguonDonRp();
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
                    _context = new GenericRepository<DmNguonDon>();
                    var model = _context.Get(id);
                    _context.Delete(id);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " nguồn hồ sơ " + model.TenNguonDon + logs.NotifySuccess, (int)ActionType.DELETE, "DmNguonDon/Delete");
                    var data = new Delete { Messeger = 2 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " nguồn hồ sơ " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DmNguonDon/Delete");
                    var data = new Delete { Messeger = 3 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult UpdateStatus(int Id, int Status)
        {
            _obj = new DmNguonDonRp();
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

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
    public class DmTinhThanhController : Controller
    {
        // GET: /DmTinhThanh/

        GenericRepository<DmTinhThanh> _context = null;
        private DmTinhThanhRp _obj = null;
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
                _context = new GenericRepository<DmTinhThanh>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmTinhThanh/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string thongtintimkiem, int TrangThai, string order, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DmTinhThanhRp();
                return Json(_obj.SearchAll(thongtintimkiem, TrangThai, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " tỉnh thành " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmTinhThanh/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<DmTinhThanh>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Create(DmTinhThanh model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<DmTinhThanh>();
                    model.NgayTao = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " tỉnh thành " + model.TenTinhThanh + logs.NotifySuccess, (int)ActionType.ADD, "DmTinhThanh/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " tỉnh thành " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới tỉnh thành", "Lỗi tạo mới tỉnh thành", "", "", "DmTinhThanh/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult Update(DmTinhThanh model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmTinhThanh>();
                model.NgayTao = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " tỉnh thành " + model.TenTinhThanh + logs.NotifySuccess, (int)ActionType.UPDATE, "DmQuocTichuAn/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " tỉnh thành " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật tỉnh thành", "Lỗi cập nhật tỉnh thành", "", "", "DmTinhThanh/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult Delete(int id)
        {
            _obj = new DmTinhThanhRp();
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

                    _context = new GenericRepository<DmTinhThanh>();
                    var model = _context.Get(id);
                    _context.Delete(id);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " tỉnh thành " + model.TenTinhThanh + logs.NotifySuccess, (int)ActionType.DELETE, "DmTinhThanh/Delete");
                    var data = new Delete { Messeger = 2 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " tỉnh thành " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DmTinhThanh/Delete");
                    var data = new Delete { Messeger = 3 };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }
            

        public ActionResult UpdateStatus(int Id, int Status)
        {
            _obj = new DmTinhThanhRp();
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
            //var data = _context3.Get((int)Session["Users"]);
            //if (data != null)
            //{
                
            //}
            //var id = data.DepartmentId;
            _obj = new DmTinhThanhRp();
            //return Json(_obj.getAllDepartmentLevel(id), JsonRequestBehavior.AllowGet);
            return Json(_obj.getAllDepartmentLevel(null), JsonRequestBehavior.AllowGet);
        }

    }
}

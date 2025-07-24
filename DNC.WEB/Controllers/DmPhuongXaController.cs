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
    public class DmPhuongXaController : Controller
    {
        //
        // GET: /DmPhuongXa/

        GenericRepository<DmPhuongXa> _context = null;
        private DmPhuongXaRp _obj = null;
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
                _context = new GenericRepository<DmPhuongXa>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmPhuongXa/GetAll");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult GetPhuongXaByQuanHuyen(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmPhuongXa>();
                return Json(_context.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE && x.idQuanHuyen == id).OrderBy(x => x.NgayTao), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmPhuongXa/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string thongtintimkiem, int TrangThai, string order, int pageSize, int pageIndex, int idTinhThanh, int idTinh)
        {
            //if(idTinh == -1)
            //{
            //    idTinhThanh = -1;
            //}
            var session = Session["Users"];
            try
            {
                _obj = new DmPhuongXaRp();
                return Json(_obj.SearchAll(thongtintimkiem, TrangThai, order, pageSize, pageIndex, idTinhThanh, idTinh), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " phường xã " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmPhuongXa/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _obj = new DmPhuongXaRp();
            _context = new GenericRepository<DmPhuongXa>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(_obj.SearchById(id)[0], JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Create(DmPhuongXa model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<DmPhuongXa>();
                    model.NgayTao = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " phường xã " + model.TenPhuongXa + logs.NotifySuccess, (int)ActionType.ADD, "DmPhuongXa/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " phường xã " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới phường xã", "Lỗi tạo mới phường xã", "", "", "DmPhuongXa/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult Update(DmPhuongXa model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmPhuongXa>();
                model.NgayTao = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " phường xã " + model.TenPhuongXa + logs.NotifySuccess, (int)ActionType.UPDATE, "DmQuocTichuAn/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " phường xã " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật phường xã", "Lỗi cập nhật phường xã", "", "", "DmPhuongXa/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public void Delete(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DmPhuongXa>();
                var model = _context.Get(id);
                _context.Delete(id);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " phường xã " + model.TenPhuongXa + logs.NotifySuccess, (int)ActionType.DELETE, "DmPhuongXa/Delete");
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " phường xã " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DmPhuongXa/Delete");
            }
        }

        public ActionResult UpdateStatus(int Id, int Status)
        {
            _obj = new DmPhuongXaRp();
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
            _obj = new DmPhuongXaRp();
            return Json(_obj.SearchID(id), JsonRequestBehavior.AllowGet);
           
        }

    }
}

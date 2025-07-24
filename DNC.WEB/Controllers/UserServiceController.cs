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
    public class UserServiceController : Controller
    {
        //
        // GET: /UserService/
        private GenericRepository<UserService> _context = null;
        private UserServiceRp _obj = new UserServiceRp();
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetIndex(string strSearch, string trang_thai, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add(Constants.ITEMS, _obj.SearchAll(strSearch, trang_thai, pageSize, pageIndex));
                dic.Add(Constants.PAGE_SIZE, pageSize);
                dic.Add(Constants.PAGE_INDEX, pageIndex);
                dic.Add(Constants.TOTAL_RECORDS, _obj.GetTotal(strSearch, trang_thai));
                return Json(dic, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " UserService " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "UserService/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<UserService>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Create(UserService model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    
                    UserService uService =
                        db.UserService.Where(x => x.userservice_name == model.userservice_name).OrderBy(x => x.ngay_tao).FirstOrDefault();
                    if (uService != null)
                    {
                        return Json(model, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _context = new GenericRepository<UserService>();
                        model.userservice_name = model.userservice_name.ToLower();
                        model.ngay_tao = DateTime.Now;
                        model.user_created_id = (int)session;
                        var data = _context.Insert(model);
                        _context.Save();
                        logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " UserService " + model.userservice_name + logs.NotifySuccess, (int)ActionType.ADD, "UserService/Create");
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " UserService " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới UserService", "Lỗi tạo mới UserService", "", "", "UserService/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult Update(UserService model)
        {
            var session = Session["Users"];
            try
            {
                UserService uService =
                        db.UserService.Where(x => x.userservice_id == model.userservice_id).OrderBy(x => x.ngay_tao).FirstOrDefault();
                if (uService != null)
                {
                    model.userservice_name = uService.userservice_name;
                    _context = new GenericRepository<UserService>();
                    _context.Update(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int) session, logs.ActionUpdate,
                        " UserService " + model.userservice_name + logs.NotifySuccess, (int) ActionType.UPDATE,
                        "UserService/Update");
                    
                }
                return Json(model, JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " UserService " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật UserService", "Lỗi cập nhật UserService", "", "", "UserService/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public void Delete(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<UserService>();
                _context.Delete(id);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " UserService " + id + logs.NotifySuccess, (int)ActionType.DELETE, "UserService/Delete");
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " UserService " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "UserService/Delete");
            }
        }

        public ActionResult UpdateStatus(int userServiceId, int status)
        {
            try
            {
                _obj.UpdateStatus(userServiceId, status);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Json(db.UserService.Where(x => x.trang_thai == Constants.STATUS_ACTIVE).OrderBy(x => x.ngay_tao).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetByName(string name)
        {
            return Json(db.UserService.Where(x => x.userservice_name == name).OrderBy(x => x.ngay_tao).FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }
    }
}

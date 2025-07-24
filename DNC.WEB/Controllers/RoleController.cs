using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using DNC.CM;
using DNC.WEB.Models;
using DNC.WEB.Repository;

namespace DNC.WEB.Controllers
{
    //[SessionTimeout]
    public class RolesController : Controller
    {
        GenericRepository<Roles> _context = null;
        private RolesRp _obj = null;
        private DbConnectContext _db = new DbConnectContext();
        Ultils logs = new Ultils();
        public ActionResult Index()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index?url_redirect=" + Request.Url.AbsoluteUri);
            }
            return View();
        }
        // Lấy về thông tin nhóm quyền 
        [HttpGet]
        public ActionResult GetById(int id)
        {
            try
            {
                _obj = new RolesRp();
                return Json(_obj.getRolebyId(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<Roles>();
                return Json(_context.GetAll(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "Roles/GetAll");
            }
            return HttpNotFound();
        } 
        [HttpGet]
        public ActionResult getAllcheckbox()
        {
            try
            {
                _obj = new RolesRp();
                return Json(_obj.getAllRole(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Create(Roles model)
        {
            var session = Session["Users"];
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    _context = new GenericRepository<Roles>();
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = (int)session;
                    model.UpdatedDate = DateTime.Now;
                    model.UpdatedBy = (int)session;
                    model.Status = model.Status;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " nhóm quyền " + model.Name + logs.NotifySuccess, (int)ActionType.ADD, "Roles/Create");

                    trans.Commit();
                    return Json(data, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    logs.SystemLog_Insert_Error((int)session, logs.ActionCreate, " nhóm quyền " + model.Name + logs.NotifyError, (int)ActionType.ADD, ex.Message,
                              ex.InnerException.ToString(), "", "", "Roles/Create");
                    trans.Rollback();
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }
        [HttpPost]
        public ActionResult Update(RoleInfo model)
        {
            var session = Session["Users"];

            try
            {
                _obj = new RolesRp();
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = (int)session;
                _obj.RolesUpdate(model);
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " nhóm quyền " + model.Name + logs.NotifySuccess, (int)ActionType.UPDATE, "Roles/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " nhóm quyền " + model.Name + logs.NotifyError, (int)ActionType.UPDATE, ex.Message,
                         ex.InnerException.ToString(), "", "", "Roles/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // Hàm Delete
        public bool Delete(int id)
        {
            _context = new GenericRepository<Roles>();
            var session = Session["Users"];
            _db = new DbConnectContext();
            _obj = new RolesRp();
            Roles model = _context.Get(id);

            try
            {
                if (!_obj.deleteURandPageRole(id, (int)session, model.Name))
                {
                    return false;
                }

                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " nhóm quyền " + model.Name + logs.NotifySuccess, (int)ActionType.DELETE, "Roles/Delete");
                return true;

            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " nhóm quyền " + model.Name + logs.NotifyError, (int)ActionType.DELETE, ex.Message,
                        ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "Roles/Delete");

                return false;
            }

        }
        // Gọi đến Respo để trả về dữ liệu
        public ActionResult GetIndex(int pageIndex, int pageSize, string keyword, int status)
        {
            _obj = new RolesRp();
            return Json(_obj.SearchAll(pageIndex, pageSize, keyword, status), JsonRequestBehavior.AllowGet);
        }

        // Đếm số người dùng và số chức năng của nhóm quyền
        public ActionResult countUserPagebyRoles(int id)
        {
            try
            {
                _obj = new RolesRp();
                var data = _obj.countUsersPage(id);
                return Json(_obj.countUsersPage(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // Kiểm tra tên nhóm quyền 
        public ActionResult checkRoleName(string name)
        {
            try
            {
                _obj = new RolesRp();
                int count = _obj.checkRoleName(name);
                return Json(count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        ///<summary>
        ///Hàm lấy về các Function đã có sẵn trong Database
        ///</summary>
        public ActionResult getFunction()
        {
            GenericRepository<Functions> obj = new GenericRepository<Functions>();
            return Json(obj.GetAll(), JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        ///Hàm lấy về các dữ liệu trong bảng pagefuntionrole theo RoleId
        /// </summary>
        public ActionResult getPageFuncRole(int id)
        {
            _obj = new RolesRp();
            return Json(_obj.getPageFunctionbyRole(id), JsonRequestBehavior.AllowGet);
        }


        ///<summary>
        ///Xử lý dữ liệu trước khi insert vào bảng phân quyền
        ///</summary>
        ///
        [HttpPost]
        public ActionResult addPageFunction(List<PageFunctionRolesAdd> data, int roleId)
        {
            try
            {
                bool result;
                List<string> listvalue = new List<string>();
                _obj = new RolesRp();
                if (data == null)
                {
                    result = _obj.createPagesFunctionRole("", roleId);
                }
                else
                {
                    foreach (PageFunctionRolesAdd Obj in data)
                    {
                        string value = "( " + Obj.PageId + ", " + Obj.FunctionId + ", " + roleId + ")";
                        listvalue.Add(value);
                    }
                    string values = String.Join(",", listvalue);
                    result = _obj.createPagesFunctionRole(values, roleId);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        // Hàm lấy về Role cho Checkbox theo Phòng ban 
        [HttpGet]
        public ActionResult getRolebyDepId(int id)
        {
            try
            {
                _obj = new RolesRp();
                return Json(_obj.getRolebyDepartment(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        //Lấy về các nhóm quyền mà người dùng đang có hiển thị trong Info
        [HttpGet]
        public ActionResult GetRoleByUserId(int id)
        {
            try
            {
                if(id == 0)
                {
                   id = (int)Session["Users"];
                }
                _obj = new RolesRp();
                return Json(_obj.GetRoleByUserId(id), JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return null;
            }
        }
         
        // Hàm Dispose Connection
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
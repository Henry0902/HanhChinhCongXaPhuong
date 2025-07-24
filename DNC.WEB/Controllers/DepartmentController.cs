using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using DNC.CM;
using DNC.WEB.Models;
using DNC.WEB.Repository;

namespace DNC.WEB.Controllers
{
  
    //[SessionTimeout]
    public class DepartmentsController : Controller
    {
        //
        // GET: /Departments/
        GenericRepository<Departments> _context = null;
        private DepartmentsRp _obj = null;
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
        [HttpGet]
        public ActionResult GetAll()
        {
            _context = new GenericRepository<Departments>();
            return Json(_context.GetAll().OrderBy(x => x.ParentId), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllUse()
        {
            _context = new GenericRepository<Departments>();
            return Json(_context.GetAll().Where(x => x.StatusUse == true).OrderBy(x => x.ParentId), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllforUsers()
        {
            _context = new GenericRepository<Departments>();
            return Json(_context.GetAll().Where(x=>x.Status == true).OrderBy(x => x.ParentId), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllforUsersinfo(int Id)
        {
            _obj = new DepartmentsRp();
            return Json(_obj.getDepartmentUsersExits(Id), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<Departments>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Create(Departments model,List<DepartmentRoles> lstDepRoles)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DepartmentsRp();
                model.Code = model.Code.ToUpper();
                model.Name = model.Name;
                model.ParentId = model.ParentId;
                model.Descriptions = model.Descriptions;
                model.Levels = model.Levels;
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = (int)session;
                model.Status = model.Status;
                model.StatusUse = model.StatusUse;
                var data = _obj.createDepartment(model, lstDepRoles);
                Departments result = new Departments();
                result.Id = data;
                result.ParentId = model.ParentId;
                logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " cơ cấu tổ chức " + model.Name + logs.NotifySuccess, (int)ActionType.ADD, "Departments/Create");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionCreate, " cơ cấu tổ chức " + model.Name + logs.NotifyError, (int)ActionType.ADD, ex.Message,
                    ex.InnerException.ToString(), "", "", "Departments/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult Update(Departments model, List<DepartmentRoles> lstDepRoles)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DepartmentsRp();
                model.Id = model.Id;
                model.Code = model.Code.ToUpper();
                model.Name = model.Name;
                model.ParentId = model.ParentId;
                model.Descriptions = model.Descriptions;
                model.Levels = model.Levels;
                model.Status = model.Status;
                model.StatusUse = model.StatusUse;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = (int)session;
                int id = _obj.updateDepartment(model, lstDepRoles);
                if (id != -1)
                {
                    logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " cơ cấu tổ chức " + model.Name + logs.NotifySuccess, (int)ActionType.UPDATE, "Departments/Update");
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception ex)
            {

                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " cơ cấu tổ chức " + model.Name + logs.NotifyError, (int)ActionType.UPDATE, ex.Message,
                    ex.InnerException.ToString(), "", "", "Departments/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public void Delete(int id)
        {
            _context = new GenericRepository<Departments>();
            var session = Session["Users"];
            _db = new DbConnectContext();
            var obj = _context.Get(id);
            try
            {
                var info = _db.Departments.Find(id);
                _obj = new DepartmentsRp();
                _obj.department_Delete(id);
                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " cơ cấu tổ chức " + obj.Name + logs.NotifySuccess, (int)ActionType.DELETE, "Departments/Delete");
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " cơ cấu tổ chức" + obj.Name + logs.NotifyError, (int)ActionType.DELETE, ex.Message,
                    ex.InnerException.ToString(), "", "", "Departments/Delete");
            }
        }
        //Check Child
        [HttpGet]
        public ActionResult CheckChirlDep(int uid , int DepId)
        {
            try
            {
                _obj = new DepartmentsRp();
                return Json(_obj.CheckDepHaschirl(uid, DepId), JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // Count Chirld
        [HttpGet]
        public ActionResult countChirl(int depID)
        {
            try
            {
                _obj = new DepartmentsRp();
                return Json(_obj.CountChirl(depID), JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult getAllDropDown_Search()
        {
            _obj = new DepartmentsRp();
            return Json(_obj.getAllDepartmentLevel(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getAllDropDown()
        {
            _obj = new DepartmentsRp();
            if (Request.Cookies["SuperUser"].Value == "1")
            {
                return Json(_obj.getAllDepartmentLevel(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(_obj.getAllDepartmentLevel().Where(x => x.Id == int.Parse(Request.Cookies["DeparmentId"].Value)), JsonRequestBehavior.AllowGet);
            }
        } 
        [HttpGet]
        public ActionResult getDepartmentByUserID(int UserID)
        {
            _obj = new DepartmentsRp();
            return Json(_obj.getDepartmentsByUserID(UserID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDepartmentsByLevel()
        {
            var result = _db.Departments.Where(x => x.Levels == 1 || x.Levels == 2)
                            .Select(m => new DepartmentDDL
                            {
                                Id = m.Id,
                                Name = m.Name,
                                Code = m.Code,
                            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Kiểm tra mã phòng ban
        [HttpGet]
        public ActionResult checkDepartmentCode(string code)
        {
            _obj = new DepartmentsRp();
            return Json(_obj.checkDepartmentCode(code.ToUpper()), JsonRequestBehavior.AllowGet);
        }
         
        // Kiểm tra tên phòng ban
        [HttpGet]
        public ActionResult checkDepartmentName(string name)
        {
            _obj = new DepartmentsRp();
            return Json(_obj.checkDepartmentName(name), JsonRequestBehavior.AllowGet);
        } 
        [HttpGet]
        public JsonResult countContent(int id)
        {
            try
            {
                _obj = new DepartmentsRp();
                return Json(_obj.Dep_CountContent(id), JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return null;
            }
        }
        [HttpGet]
        public ActionResult getDepbyUserNotin(int id)
        {
            _obj = new DepartmentsRp();
            return Json(_obj.getDepartmentUsernotin(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getListEdit(int Id)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DepartmentsRp();
                return Json(_obj.GetListEdit(Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " cơ cấu tổ chức " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "Departments/getListEdit");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // lay ve phong ban ma nguoi dung dang truc thuoc

        [HttpGet]
        public ActionResult getDepartmentbyUsers(int id)
        {
            try {
                _obj = new DepartmentsRp();
                if(id == 0)
                {
                    id = (int)Session["Users"];
                }
                return Json(_obj.GetDepartmentUserInfo(id), JsonRequestBehavior.AllowGet);

            }
            catch(Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpGet]
        public ActionResult GetAllLevelByParentId(int parentId)
        {
            try
            {
                _obj = new DepartmentsRp();
                return Json(_obj.GetAllLevelByParentId(parentId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        //dungnc - lấy danh sách đơn vị sắp xếp theo cấp đơn vị cha, con
        [HttpGet]
        public ActionResult GetAllOrderByLevel()
        {
            try
            {
                _obj = new DepartmentsRp();
                return Json(_obj.GetAllOrderByLevel(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        //dungnc - lấy danh sách đơn vị trực thuộc theo id cha
        [HttpGet]
        public ActionResult getAllDropDown_ByParentId(int parentId)
        {
            _obj = new DepartmentsRp();
            return Json(_obj.getAllDropDown_ByParentId(parentId), JsonRequestBehavior.AllowGet);
        }

        //Lấy danh sách đơn vị theo id cha nhưng level 1 
        [HttpGet]
        public ActionResult getDropDownLevel1_ByParentId(int parentId)
        {
            _obj = new DepartmentsRp();
            return Json(_obj.getDropDownLevel1_ByParentId(parentId), JsonRequestBehavior.AllowGet);
        }

        //dungnc - lấy danh sách đơn vị trực thuộc theo id cha và không có ký tự - 
        [HttpGet]
        public ActionResult getAllDropDown_ByParentId_NoSpecialCharacter(int parentId)
        {
            _obj = new DepartmentsRp();
            return Json(_obj.getAllDropDown_ByParentId_NoSpecialCharacter(parentId), JsonRequestBehavior.AllowGet);
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
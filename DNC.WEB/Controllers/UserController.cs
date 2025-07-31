using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using DNC.WEB.Models;
using DNC.WEB.Repository;
using DNC.CM;
using DNC.WEB.Common;
using System.Globalization;
using System.IO;
using System.Net.Mime;
using DNC.WEB.Services;
using System.Data.Entity.Migrations;

namespace DNC.WEB.Controllers
{

    //[SessionTimeout]
    public class UsersController : Controller
    {
        GenericRepository<Users> _context = null;
        private UsersRp _obj = null;
        private DbConnectContext _db = new DbConnectContext();
        private EmailService _emailService;
        //Ultils logs = new Ultils();
        public ActionResult Index()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index?url_redirect=" + Request.Url.AbsoluteUri);
            }
            _obj = new UsersRp();
            return View();
        }

        public ActionResult RegisterAccount()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GenerateCaptcha()
        {
            var random = new Random();
            int num1 = random.Next(1, 10);
            int num2 = random.Next(1, 10);

            return Json(new { question = num1.ToString() + " + " + num2.ToString(), answer = num1 + num2 }, JsonRequestBehavior.AllowGet);
        }

        public void DeleteUserPassword(int id)
        {
            var result = _db.UsersQuenMatKhau.Find(id);
            if (result != null)
            {
                _db.UsersQuenMatKhau.Remove(result);
                _db.SaveChanges();
            }
        }

        [HttpPost]
        public ActionResult SendOTP(string mobile)
        {
            try
            {
                _emailService = new EmailService();
                var user = _db.Users.FirstOrDefault(x => x.Mobile == mobile);

                if (user != null) {
                    Random random = new Random();
                    var otp = random.Next(0, 999999).ToString("D6");

                    var userPass = _db.UsersQuenMatKhau.FirstOrDefault(x => x.IdCongDan == user.Id);
                    if (userPass != null)
                    {
                        //Kiểm tra thời gian giữa lần gửi OTP đầu tiên và hiện tại
                        var currentTime = DateTime.Now;
                        var timeSinceFirstSend = currentTime - userPass.OtpGuiLanDau;

                        //Nếu thời gian từ lần gửi đầu tiên lớn hơn 1 giờ, reset số lần gửi
                        if (timeSinceFirstSend.TotalHours > 1)
                        {
                            userPass.OtpLanGui = 1;
                            userPass.OtpGuiLanDau = currentTime;
                        }
                        else
                        {
                            //Nếu số lần gửi đạt ngưỡng 5 lần, khoá số điện thoại
                            if (userPass.OtpLanGui >= 5)
                            {
                                _db.UsersQuenMatKhau.AddOrUpdate(userPass);
                                _db.SaveChanges();
                                return Json(new DataResponse("Đã gửi quá 5 lần, vui lòng chờ 60 phút để thực hiện lại yêu cầu.", userPass, 400), JsonRequestBehavior.AllowGet);
                            }

                            userPass.OtpLanGui += 1;
                        }

                        userPass.Otp = otp;
                        userPass.OtpLanNhap = 0;
                        userPass.OtpTrangThai = 0;
                        userPass.OtpThoiGianTao = currentTime;
                        userPass.OtpHsd = currentTime.AddSeconds(60);

                        _db.UsersQuenMatKhau.AddOrUpdate(userPass);
                        _db.SaveChanges();
                    }
                    else
                    {
                        var dateNow = DateTime.Now;
                        userPass = new UsersQuenMatKhau()
                        {
                            IdCongDan = user.Id,
                            Otp = otp,
                            OtpThoiGianTao = dateNow,
                            OtpHsd = dateNow.AddSeconds(60),
                            OtpGuiLanDau = dateNow,
                            OtpTrangThai = 0,
                            OtpLanNhap = 0,
                            OtpLanGui = 1
                        };

                        _db.UsersQuenMatKhau.Add(userPass);
                        _db.SaveChanges();
                    }

                    _emailService.SendEmail(user.Email, "OTP lấy lại mật khẩu", "Mã OTP của bạn là: " + otp);
                    return Json(new DataResponse("OTP", userPass, 200), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new DataResponse("Không tìm thấy tài khoản: " + mobile, null, 400), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult CheckOtp(int id, string otp)
        {
            var result = _db.UsersQuenMatKhau.Find(id);
            if (result == null)
            {
                return Json(new DataResponse("Không tìm thấy bản ghi nào", null, 400), JsonRequestBehavior.AllowGet);
            }

            if (result.OtpLanNhap == 5)
            {
                result.OtpTrangThai = 2;
                _db.UsersQuenMatKhau.AddOrUpdate(result);
                _db.SaveChanges();
                return Json(new DataResponse("Bạn đã nhập sai quá 5 lần. Vui lòng gửi lại OTP mới.", null, 400, "01"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var now = DateTime.Now;
                var count = _db.UsersQuenMatKhau.Count(x => x.Otp == otp && now >= x.OtpThoiGianTao && now <= x.OtpHsd && x.OtpTrangThai == 0);
                var isCheck = count > 0 ? true : false;
                result.OtpLanNhap += 1;
                _db.UsersQuenMatKhau.AddOrUpdate(result);
                _db.SaveChanges();
                return Json(new DataResponse(isCheck ? "OTP hợp lệ" : ("Nhập sai mã OTP, bạn còn " + (5 - result.OtpLanNhap).ToString() + " lần nhập"), isCheck, isCheck ? 200 : 400), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ValidateCaptcha(CaptchaModel model)
        {
            try
            {
                if (model.UserAnswer == model.CorrectAnswer)
                {
                    var user = _db.Users.FirstOrDefault(x => x.Mobile == model.PhoneNumber);
                    if (user != null)
                    {
                        return Json(new { success = true, message = "Thông tin tài khoản", userInfo = user }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Số điện thoại chưa được đăng ký" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Captcha không chính xác. Vui lòng thử lại!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            _context = new GenericRepository<Users>();
            return Json(_context.GetAll().Where(x => x.IsDeleted == false), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<Users>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        //lấy tên user theo id
        [HttpGet]
        public ActionResult GetDisplayNameById(int id)
        {
            var user = _db.Users.Where(x => x.Id == id)
                                .Select(x => new { x.DisplayName })
                                .FirstOrDefault();
            if (user != null)
                return Json(user, JsonRequestBehavior.AllowGet);
            return HttpNotFound();
        }

        [HttpGet]
        public Users GetUserInfoById(int id)
        {
            _context = new GenericRepository<Users>();
            var data = _context.Get(id);
            if (data != null)
            {
                return data;
            }
            return null;
        }

        [HttpPost]
        public ActionResult Create(Users model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<Users>();
                    model.UserName = model.UserName;
                    if (model.Password != null && model.IsSuper != 2)
                    {
                        model.Password = Encryptor.MD5Hash(model.Password);
                    }
                    //model.DisplayName = model.DisplayName;
                    //model.Avatar = model.Avatar;
                    //model.Mobile = model.Mobile;
                    //model.Email = model.Email;
                    //model.Gender = model.Gender;
                    //model.DateOfBirth = model.DateOfBirth;
                    model.CreatedDate = DateTime.Now;
                    model.IsLocked = false;
                    model.IsDeleted = false;
                    _context = new GenericRepository<Users>();
                    var data = _context.Insert(model);
                    _context.Save();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult UpdateStatusCititzen(int id, int status, string note)
        {
            var session = Session["Users"];
            _obj = new UsersRp();
            try
            {
                _context = new GenericRepository<Users>();
                var entity = _context.Get(id);
                if (entity != null)
                {
                    entity.Status = status;
                    entity.UpdatedDate = DateTime.Now;
                    _context.Update(entity);
                    _context.Save();

                    var historyEntity = new UsersHistory()
                    {
                        IdCongDan = entity.Id,
                        IdNguoiDuyet = (int)session,
                        NgayDuyet = DateTime.Now,
                        GhiChu = note
                    };

                    _db.UsersHistories.Add(historyEntity);
                    _db.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateProfile(UpdateProfileRequest request)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<Users>();
                var entity = _context.Get(request.Id);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy bản ghi nào");
                }

                string filePath = null;
                if (!string.IsNullOrEmpty(request.AvatarFile))
                {
                    string year = DateTime.Now.Year.ToString();
                    string month = DateTime.Now.Month.ToString();
                    string day = DateTime.Now.Day.ToString();

                    if (!Directory.Exists(Server.MapPath(Constants.PATH_FOLDER_FILESUPLOAD + year)))
                    {
                        Directory.CreateDirectory(Server.MapPath(Constants.PATH_FOLDER_FILESUPLOAD + year));
                    }

                    if (!Directory.Exists(Server.MapPath(Constants.PATH_FOLDER_FILESUPLOAD + year + "/" + month)))
                    {
                        Directory.CreateDirectory(Server.MapPath(Constants.PATH_FOLDER_FILESUPLOAD + year + "/" + month));
                    }

                    if (!Directory.Exists(Server.MapPath(Constants.PATH_FOLDER_FILESUPLOAD + year + "/" + month + "/" + day)))
                    {
                        Directory.CreateDirectory(Server.MapPath(Constants.PATH_FOLDER_FILESUPLOAD + year + "/" + month + "/" + day));
                    }
                    filePath = Constants.PATH_FOLDER_FILESUPLOAD + year + "/" + month + "/" + day + "/" + request.FileName;

                    var path = Path.Combine(Server.MapPath(filePath));

                    if (!string.IsNullOrEmpty(request.AvatarFile))
                    {
                        CommonFunc.SaveBase64ToFile(request.AvatarFile, path);
                    }
                }

                entity.DisplayName = request.DisplayName;
                entity.Gender = request.Gender;

                if (!string.IsNullOrEmpty(request.DateOfBirth))
                {
                    entity.DateOfBirth = DateTime.ParseExact(request.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                entity.IdCard = request.IdCard;

                if (!string.IsNullOrEmpty(request.DateOfBirth))
                {
                    entity.IssuanceDate = DateTime.ParseExact(request.IssuanceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(filePath))
                {
                    entity.Avatar = filePath;
                }
                
                entity.IssuanceAgency = request.IssuanceAgency;
                entity.ProvinceId = request.ProvinceId;
                entity.DistrictId = request.DistrictId;
                entity.CommuneId = request.CommuneId;
                entity.SpecifiedAddress = request.SpecifiedAddress;
                entity.EthnicId = request.EthnicId;
                entity.NationalityId = request.NationalityId;
                entity.Status = request.Status;
                entity.UpdatedDate = DateTime.Now;
                _context.Update(entity);
                _context.Save();

                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
        [HttpPost]
        public ActionResult Update(Users model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<Users>();
                model.CreatedDate = model.CreatedDate;
                model.UpdatedDate = DateTime.Now; 
                model.IsLocked = model.IsLocked;
                model.DateOfBirth = model.DateOfBirth;
                model.Password = model.Password;
                _context.Update(model);
                _context.Save();
              //  logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " người dùng " + model.UserName + logs.NotifySuccess, (int)ActionType.UPDATE, "Users/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
              //  logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " người dùng " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật người dùng", "Lỗi cập nhật người dùng", "", "", "Users/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Updates()
        {
            AccountRp accRP = new AccountRp();
            _obj = new UsersRp();
            Users result = JsonConvert.DeserializeObject<Users>(Request.Form["model"]);
            List<UsersRoles> lst = JsonConvert.DeserializeObject<List<UsersRoles>>(Request.Form["lstUR"]);
            int DepID = JsonConvert.DeserializeObject<int>(Request.Form["DepId"]);
            var session = Session["Users"];
            try
            {
                string pathFile = accRP.SystemConfigs_GetAll_ByKey("USERPATHAVATAR").Value;
                _context = new GenericRepository<Users>();
                result.UpdatedDate = DateTime.Now;
                string values = "";
                if (lst != null)
                {
                    List<string> listvalue = new List<string>();
                    foreach (UsersRoles Obj in lst)
                    {
                        string value = "(" + Obj.DerpartmentId + "," + Obj.UserId + "," + Obj.RoleId + ")";
                        listvalue.Add(value);
                    }
                    values = String.Join(",", listvalue);
                }

                bool update = _obj.updateUserinfoAdmin(result, values, DepID);

                if (update == false)
                {
                  //  logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " người dùng " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi Cập nhật người dùng", "Lỗi Cập nhật người dùng", "", "", "Users/Update");
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }
               // logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " người dùng " + result.UserName + logs.NotifySuccess, (int)ActionType.UPDATE, "Users/Update");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
              //  logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " người dùng " + logs.NotifyError, (int)ActionType.UPDATE, ex.Message, ex.InnerException.ToString(), "", "", "Users/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // Hàm Delete
        public void Delete(int id)
        {
            _db = new DbConnectContext();
            var session = Session["Users"];
            try
            {

                _context = new GenericRepository<Users>();
                var result = _context.Get(id);

                _context.Delete(id);
                _context.Save();
                //logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " người dùng " + result.UserName + logs.NotifySuccess, (int)ActionType.DELETE, "Users/Delete");
            }
            catch (Exception ex)
            {
                //logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " người dùng " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "Users/Delete");
            }
        }

        // Gọi đến Respository để trả về dữ liệu
        public ActionResult GetIndex(int pageIndex, int pageSize, string name, int islocked, int isdeleted, int roleID)
        {
            _obj = new UsersRp();
            return Json(_obj.SearchAll(pageIndex, pageSize, name, islocked, isdeleted, roleID), JsonRequestBehavior.AllowGet);
        }

        //get user by role id
        public ActionResult GetAllByRole(int roleId)
        {
            _obj = new UsersRp();
            var data = _obj.GetUsersByRole(roleId); // Sử dụng stored procedure
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //lấy ra tên cán bộ tiếp nhận
        //public ActionResult GetTenCanBoTiepNhan(int IdNguoiTao)
        //{
        //    _obj = new UsersRp();
        //    var data = _obj.GetById(IdNguoiTao); // Sử dụng stored procedure
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult GetById(int id)
        //{
        //    _obj = new UsersRp();
        //    var data = _obj.GetById(id);
        //    if (data != null)
        //    {
        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }
        //    return HttpNotFound();
        //}

        [HttpPost]
        public ActionResult getsuggestUser(string keyword)
        {
            try
            {
                _obj = new UsersRp();
                List<UsersSuggest> data = _obj.getSuggestUsers(keyword);


                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        } 
        public ActionResult updateStatus(int Id, bool Status)
        {
            _obj = new UsersRp();
            try
            {
                int session = (int)Session["Users"];
                Users info = GetUserInfoById(Id);
                _obj.updateStatus(Id, Status);
               // logs.SystemLog_Insert_Success(session, logs.ActionUpdate, " Cập nhật thông tin trạng thái người dùng " + info.DisplayName + logs.NotifySuccess, (int)ActionType.UPDATE, "Users/updateStatus");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        } 
        public ActionResult GetDepartment(int Id)
        {
            _obj = new UsersRp();
            int session = (int)Session["Users"];
            try
            {
                Users info = GetUserInfoById(Id);
               // logs.SystemLog_Insert_Success(session, logs.ActionLoadData, " phòng ban của người dùng " + info.DisplayName + logs.NotifySuccess, (int)ActionType.GET, "Users/GetDepartment");
                return Json(_obj.getDepartmentUser(Id), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
              //  logs.SystemLog_Insert_Error(session, logs.ActionLoadData, " phòng ban của người dùng " + logs.NotifyError, (int)ActionType.UPDATE, ex.Message, ex.InnerException.ToString(), "", "", "Users/GetDepartment");
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        } 
        public ActionResult UpdateInfo()
        {
            _obj = new UsersRp();
            int session = (int)Session["Users"];
            try
            {
                Users result = JsonConvert.DeserializeObject<Users>(Request.Form["model"]);

                AccountRp accRP = new AccountRp();
             //   logs.SystemLog_Insert_Success(session, logs.ActionUpdate, " thông tin người dùng " + result.DisplayName + logs.NotifySuccess, (int)ActionType.GET, "Users/UpdateInfo");
                return Json(_obj.UpdateUsersInfo(result), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
               // logs.SystemLog_Insert_Error(session, logs.ActionUpdate, " thông tin người dùng " + logs.NotifyError, (int)ActionType.UPDATE, ex.Message, ex.InnerException.ToString(), "", "", "Users/UpdateInfo");
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ChangePassword(Users model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<Users>();
                if (model.Password != null)
                {
                    model.Password = Encryptor.MD5Hash(model.Password);
                }
                _context.Update(model);
                _context.Save();
                 
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public bool CheckPassword(string pass)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<Users>();
                Users xmodel = new Users();
                xmodel = _context.Get((int)session);
                string oldpass = Encryptor.MD5Hash(pass);
                if (oldpass == xmodel.Password)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
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

        [HttpGet]
        public ActionResult GetAllByDeparmenttId(int departmentId)
        {
            return Json(_db.Users.Where(x => x.DepartmentId == departmentId).OrderBy(x => x.UserName).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Net;
using System.Web.Mvc;
using DNC.WEB.Models;
using DNC.WEB.Repository;
using DNC.CM;
using System.Globalization;
using System.Linq;

namespace DNC.WEB.Controllers
{
    public class UsersTempController : Controller
    {
        GenericRepository<UsersTemp> _context = null;
        private UsersRp _obj = null;
        private UsersTempRp _objUserTemp = null;
        private DbConnectContext _db = new DbConnectContext();

        public UsersTempController()
        {
            _obj = new UsersRp();
            _objUserTemp = new UsersTempRp();
            _context = new GenericRepository<UsersTemp>();
        }

        [HttpPost]
        public ActionResult Create(CreateUserTempRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listCheck = _obj.CheckExisted(model.Mobile, model.Email, model.IdCard);
                    if (listCheck.Any())
                    {
                        return Json(new DataResponse("Số điện thoại hoặc email hoặc cccd đã tồn tại", listCheck, 400), JsonRequestBehavior.AllowGet);
                    }

                    var entity = new UsersTemp();
                    entity.UserName = model.Mobile;
                    if (model.Password != null)
                    {
                        entity.Password = Encryptor.MD5Hash(model.Password);
                    }
                    entity.DisplayName = model.DisplayName;
                    entity.Gender = model.Gender == 0 ? true : false;
                    entity.Mobile = model.Mobile;
                    entity.Email = model.Email;

                    if (!string.IsNullOrEmpty(model.DateOfBirth))
                    {
                        entity.DateOfBirth = DateTime.ParseExact(model.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    
                    entity.IsLocked = false;
                    entity.IsDeleted = false;
                    entity.IsSuper = false;
                    entity.IdCard = model.IdCard;

                    if (!string.IsNullOrEmpty(model.DateOfBirth))
                    {
                        entity.IssuanceDate = DateTime.ParseExact(model.IssuanceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                   
                    entity.IssuanceAgency = model.IssuanceAgency;
                    entity.EthnicId = model.EthnicId;
                    entity.NationalityId = model.NationalityId;
                    entity.SpecifiedAddress = model.SpecifiedAddress;
                    entity.ProvinceId = model.ProvinceId;
                    entity.DistrictId = model.DistrictId;
                    entity.CommuneId = model.CommuneId;

                    entity.Otp = GenerateOtp();
                    var dateNow = DateTime.Now;
                    entity.OtpStartTime = dateNow;
                    entity.OtpEndTime = dateNow.AddSeconds(60);
                    entity.OtpStatus = 0;
                    entity.TimesEntryOtp = 0;
                    entity.TimesSendOtp = 1;
                    entity.OtpFirstSend = dateNow;
                    entity.CreatedDate = DateTime.Now;

                    var data = _context.Insert(entity);
                    _context.Save();
                    return Json(new DataResponse("Thêm mới tài khoản thành công", data, 200), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult SendOTP(int id)
        {
            try
            {
                var userTemp = _context.Get(id);
                if (userTemp == null)
                {
                    return Json(new DataResponse("Tài khoản không tồn tại", userTemp, 400), JsonRequestBehavior.AllowGet);
                }

                //Kiểm tra thời gian giữa lần gửi OTP đầu tiên và hiện tại
                var currentTime = DateTime.Now;
                var timeSinceFirstSend = currentTime - userTemp.OtpFirstSend;

                //Nếu thời gian từ lần gửi đầu tiên lớn hơn 1 giờ, reset số lần gửi
                if (timeSinceFirstSend.TotalHours > 1)
                {
                    userTemp.TimesSendOtp = 1;
                    userTemp.OtpFirstSend = currentTime;
                }
                else
                {
                    //Nếu số lần gửi đạt ngưỡng 5 lần, khoá số điện thoại
                    if (userTemp.TimesSendOtp >= 5)
                    {
                        userTemp.IsLocked = true;
                        _context.Update(userTemp);
                        _context.Save();
                        _context.Dispose();
                        return Json(new DataResponse("Đã gửi quá 5 lần, vui lòng chờ 60 phút để thực hiện lại yêu cầu.", userTemp, 400), JsonRequestBehavior.AllowGet);
                    }

                    userTemp.TimesSendOtp += 1;
                }

                userTemp.Otp = GenerateOtp();
                userTemp.OtpStatus = 0;
                userTemp.OtpStartTime = currentTime;
                userTemp.OtpEndTime = currentTime.AddSeconds(60);
                userTemp.TimesEntryOtp = 0;

                _context.Update(userTemp);
                _context.Save();
                _context.Dispose();
                return Json(new DataResponse("Gửi OTP thành công", userTemp, 200), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var result = _context.Get(id);
                if (result != null)
                {
                    _context.Delete(id);
                    _context.Save();
                }else
                {
                    throw new Exception("Can not found UsersTemp by id" + id);
                }
            }
            catch (Exception ex)
            {
            }
        }

        [HttpPost]
        public ActionResult DeleteUserExisted(string phoneNumber, string email, string idCard)
        {
            try
            {
                var userTemp = _db.UsersTemps.Where(x => x.Mobile == phoneNumber || x.Email.ToLower() == email.ToLower() || x.IdCard.ToLower() == idCard.ToLower()).FirstOrDefault();
                if (userTemp != null)
                {
                    _db.UsersTemps.Remove(userTemp);
                    _db.SaveChanges();
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult CheckOtp(int id, string otp)
        {
            var result = _context.Get(id);
            if (result == null)
            {
                return Json(new DataResponse("Không tìm thấy bản ghi nào", null, 400), JsonRequestBehavior.AllowGet);
            }

            if (result.TimesEntryOtp == 5)
            {
                result.OtpStatus = 2;
                _context.Update(result);
                _context.Save();
                return Json(new DataResponse("Bạn đã nhập sai quá 5 lần. Vui lòng gửi lại OTP mới.", null, 400, "01"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var isCheck = _objUserTemp.CheckOtp(otp);
                result.TimesEntryOtp += 1;
                _context.Update(result);
                _context.Save();
                return Json(new DataResponse(isCheck ? "OTP hợp lệ" : ("Nhập sai mã OTP, bạn còn " + (5 - result.TimesEntryOtp).ToString() + " lần nhập"), isCheck, isCheck ? 200 : 400), JsonRequestBehavior.AllowGet);
            }
        }

        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(0, 999999).ToString("D6");
        }
    }
}
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DNC.WEB.Models;
using DNC.WEB.Repository;
using DNC.CM;
using System.Globalization;
using DNC.WEB.Common;

namespace DNC.WEB.Controllers
{
    public class XuLyDonThuController : Controller
    {
        GenericRepository<XuLyDonThu> _context = null;
        private XuLyDonThuRp _obj = null;
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(XuLyDonThu model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<XuLyDonThu>();
                    model.NgayTao = DateTime.Now;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " Xử lý hồ sơ " + model.IdDonThu + logs.NotifySuccess, (int)ActionType.ADD, "XuLyDonThu/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " Xử lý hồ sơ " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo bước xử lý hồ sơ", "Lỗi tạo bước xử lý hồ sơ", "", "", "XuLyDonThu/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Update(XuLyDonThu model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<XuLyDonThu>();
                model.NgayTao = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " Xử lý hồ sơ " + model.IdDonThu + logs.NotifySuccess, (int)ActionType.UPDATE, "XuLyDonThu/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " Xử lý hồ sơ " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật Đơn Thư", "Lỗi cập nhật Đơn Thư", "", "", "DonThu/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult GetByIdTiepCongDan(string id)
        {
            var session = Session["Users"];
            try
            {
                _obj = new XuLyDonThuRp();
                return Json(_obj.GetByIdTiepCongDan(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Xử lý hồ sơ" + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "XuLyDonThu/GetByIdTiepCongDan");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult GetByIdDonThu(string id)
        {
            var session = Session["Users"];
            try
            {
                _obj = new XuLyDonThuRp();
                return Json(_obj.GetByIdDonThu(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Xử lý hồ sơ" + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "XuLyDonThu/GetByIdTiepCongDan");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult GetViewByIdDonThu(string id)
        {
            var session = Session["Users"];
            try
            {
                _obj = new XuLyDonThuRp();
                var XuLyDonThu = new List<XuLyDonThuSearchView>();
                //var data = _obj.GetViewByIdDonThu(id);
                var data = _obj.GetViewByIdDonThuGoc(id);
                foreach (var i in data)
                {
                    var File = db.FileUpload.Where(x => x.file_type == Constants.FileType_XuLy && x.objects_id == i.Id).OrderBy(x => x.ngay_tao).ToList();
                    i.FileUpload = new List<FileUpload>();
                    i.FileUpload.AddRange(File);
                    XuLyDonThu.Add(i);
                }

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Xử lý hồ sơ" + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "XuLyDonThu/GetByIdTiepCongDan");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult GetViewThuLyGiaiQuyetByIdDonThu(string id)
        {
            var session = Session["Users"];
            try
            {
                _obj = new XuLyDonThuRp();
                var XuLyDonThu = new List<XuLyDonThuSearchView>();
                var data = _obj.GetViewByIdDonThu(id).Where(x => x.IdTrangThai == Constants.TrangThaiThuLyGiaiQuyet).OrderBy(x => x.Id);
                foreach (var i in data)
                {
                    var File = db.FileUpload.Where(x => x.file_type == Constants.FileType_XuLy && x.objects_id == i.Id).OrderBy(x => x.ngay_tao).ToList();
                    i.FileUpload = new List<FileUpload>();
                    i.FileUpload.AddRange(File);
                    XuLyDonThu.Add(i);
                }

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Xử lý hồ sơ" + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "XuLyDonThu/GetByIdTiepCongDan");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

    }
}

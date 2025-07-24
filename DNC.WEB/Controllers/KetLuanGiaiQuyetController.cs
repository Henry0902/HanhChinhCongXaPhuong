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
    public class KetLuanGiaiQuyetController : Controller
    {
        GenericRepository<KetLuanGiaiQuyet> _context = null;
      //  private DuAnRp _obj = null;
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<KetLuanGiaiQuyet>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        public ActionResult GetInfoViewByID(int id)
        {
            var session = Session["Users"];
            try
            {
                KetLuanGiaiQuyet KetLuan = db.KetLuanGiaiQuyet.Where(x => x.IdDonThu == id).FirstOrDefault();
                KetLuanGiaiQuyetInfoView KetLuanView = new KetLuanGiaiQuyetInfoView();
                KetLuanView.IdDonThu = id;
                if (KetLuan != null)
                {
                    KetLuanView.Id = KetLuan.Id;
                    KetLuanView.NgayBanHanh = KetLuan.NgayBanHanh;
                    KetLuanView.IdLoaiKetQua = KetLuan.IdLoaiKetQua;
                    KetLuanView.TenLoaiKetQua = db.DmLoaiKetQua.Where(x => x.Id == KetLuan.IdLoaiKetQua).First().TenLoaiKetQua;
                    KetLuanView.NoiDung = KetLuan.NoiDung;
                    KetLuanView.NNSoTien = KetLuan.NNSoTien;
                    KetLuanView.NNSoDat = KetLuan.NNSoDat;
                    KetLuanView.NNSoDatSX = KetLuan.NNSoDatSX;
                    KetLuanView.CNSoTien = KetLuan.CNSoTien;
                    KetLuanView.CNSoDat = KetLuan.CNSoDat;
                    KetLuanView.CNSoDatSX = KetLuan.CNSoDatSX;
                    KetLuanView.NgayTao = KetLuan.NgayTao;

                    var File = db.FileUpload.Where(x => x.file_type == Constants.FileType_KetLuan && x.objects_id == KetLuan.Id).OrderBy(x => x.ngay_tao).ToList();
                    KetLuanView.FileUpload = new List<FileUpload>();
                    KetLuanView.FileUpload.AddRange(File);
                }
                return Json(KetLuanView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Kết luận giải quyết " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "KetLuanGiaiQuyet/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Create(KetLuanGiaiQuyet model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<KetLuanGiaiQuyet>();
                    model.NgayTao = DateTime.Now;
                    var data = _context.Insert(model);
                    data.IdDonThuGoc = data.IdDonThu;
                    _context.Save();
                    
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " Kết luận giải quyết " + model.NoiDung + logs.NotifySuccess, (int)ActionType.ADD, "KetLuanGiaiQuyet/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " Kết luận giải quyết " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới dự án", "Lỗi tạo mới dự án", "", "", "KetLuanGiaiQuyet/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult Update(KetLuanGiaiQuyet model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<KetLuanGiaiQuyet>();
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " Kết luận giải quyết " + model.NoiDung + logs.NotifySuccess, (int)ActionType.UPDATE, "KetLuanGiaiQuyet/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " Kết luận giải quyết " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật dự án", "Lỗi cập nhật dự án", "", "", "KetLuanGiaiQuyet/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public void Delete(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<KetLuanGiaiQuyet>();
                var model = _context.Get(id);
                _context.Delete(id);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " Kết luận giải quyết " + model.NoiDung + logs.NotifySuccess, (int)ActionType.DELETE, "DuAn/KetLuanGiaiQuyet");
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " Kết luận giải quyết " + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), "", "", "DuAn/KetLuanGiaiQuyet");
            }
        }

    }
}

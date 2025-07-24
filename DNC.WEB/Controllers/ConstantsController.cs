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
    public class ConstantsController : Controller
    {
        GenericRepository<DmHuongXuLy> _contextDmHuongXuLy = null;
        GenericRepository<DmTrangThai> _contextDmTrangThai = null;
        GenericRepository<DmNguonDon> _contextDmNguonDon = null;
        GenericRepository<DmPhienTiepCongDan> _contextDmPhienTiepCongDan = null;
        GenericRepository<DmCoQuan> _contextDmCoQuan = null;
        GenericRepository<DmLoaiKetQua> _contextDmLoaiKetQua = null;
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetDmHuongXuLy()
        {
            var session = Session["Users"];
            try
            {
                _contextDmHuongXuLy = new GenericRepository<DmHuongXuLy>();
                return Json(_contextDmHuongXuLy.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmQuocTich/GetAll");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult GetDmTrangThai()
        {
            var session = Session["Users"];
            try
            {
                _contextDmTrangThai = new GenericRepository<DmTrangThai>();
                return Json(_contextDmTrangThai.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmTrangThai/GetAll");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult GetDmNguonDon()
        {
            var session = Session["Users"];
            try
            {
                _contextDmNguonDon = new GenericRepository<DmNguonDon>();
                return Json(_contextDmNguonDon.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE && x.Id != 0).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmNguonDon/GetAll");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult GetDmPhienTiepCongDan()
        {
            var session = Session["Users"];
            try
            {
                _contextDmPhienTiepCongDan = new GenericRepository<DmPhienTiepCongDan>();
                return Json(_contextDmPhienTiepCongDan.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE && x.Id != 0).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmNguonDon/GetAll");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult GetDmCoQuan()
        {
            var session = Session["Users"];
            try
            {
                _contextDmCoQuan = new GenericRepository<DmCoQuan>();
                return Json(_contextDmCoQuan.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE && x.Id != 0).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmNguonDon/GetAll");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult GetDmLoaiKetQua()
        {
            var session = Session["Users"];
            try
            {
                _contextDmLoaiKetQua = new GenericRepository<DmLoaiKetQua>();
                return Json(_contextDmLoaiKetQua.GetAll().Where(x => x.TrangThai == Constants.STATUS_ACTIVE && x.Id != 0).OrderBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DmNguonDon/GetAll");
            }
            return HttpNotFound();
        }  
    }
}

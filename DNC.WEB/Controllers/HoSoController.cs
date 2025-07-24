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
    public class HoSoController : Controller
    {
        GenericRepository<HoSo> _context = null;
        private HoSoRp _obj = null;
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Timkiem()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<HoSo>();
                return Json(_context.GetAll(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "HoSo/GetAll");
            }
            return HttpNotFound();
        }

        public ActionResult GetIndex(string nam, string kho_id, string kegia_id, string hopcap_id, string donvi_id, string loaihs_id, string domat_id, string ttvl_id,
                            string ngay_baoquan_from, string ngay_baoquan_to, string ngay_luutru_from, string ngay_luutru_to,
                            string Keyword, string hoso_type, string trang_thai, string order, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new HoSoRp();
                if (ngay_baoquan_from != "")
                {
                    ngay_baoquan_from = DateTime.ParseExact(ngay_baoquan_from, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (ngay_baoquan_to != "")
                {
                    ngay_baoquan_to = DateTime.ParseExact(ngay_baoquan_to, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (ngay_luutru_from != "")
                {
                    ngay_luutru_from = DateTime.ParseExact(ngay_luutru_from, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (ngay_luutru_to != "")
                {
                    ngay_luutru_to = DateTime.ParseExact(ngay_luutru_to, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                //return Json(_obj.SearchAll(nam, kho_id, kegia_id, hopcap_id, donvi_id, loaihs_id, domat_id, ttvl_id, ngay_baoquan_from, ngay_baoquan_to,
                //    ngay_luutru_from, ngay_luutru_to, Keyword, hoso_type, trang_thai, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
                return null;
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " hồ sơ " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "HoSo/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<HoSo>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        public ActionResult GetByHopCapId(int hopcap_id)
        {
            return Json(db.HoSo.Where(x => x.trang_thai == Constants.STATUS_ACTIVE && x.hopcap_id == hopcap_id).OrderBy(x => x.ngay_tao).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetByHoso_ma(string hoso_ma)
        {
            return Json(db.HoSo.Where(x => x.trang_thai == Constants.STATUS_ACTIVE && x.hoso_ma.ToLower() == hoso_ma.ToLower()).OrderBy(x => x.ngay_tao).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(HoSo model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _context = new GenericRepository<HoSo>();
                    model.ngay_tao = DateTime.Now;
                    model.trang_thai = Constants.STATUS_ACTIVE;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " hồ sơ " + model.hoso_ten + logs.NotifySuccess, (int)ActionType.ADD, "HoSo/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " hồ sơ " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới hồ sơ", "Lỗi tạo mới hồ sơ", "", "", "HoSo/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Update(HoSo model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<HoSo>();
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " hồ sơ " + model.hoso_ten + logs.NotifySuccess, (int)ActionType.UPDATE, "HoSo/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " hồ sơ " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật hồ sơ", "Lỗi cập nhật hồ sơ", "", "", "HoSo/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult updateStatus(int Id, bool Status)
        {
            _obj = new HoSoRp();
            try
            {
                _obj.updateStatus(Id, Status);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        } 
    }
}

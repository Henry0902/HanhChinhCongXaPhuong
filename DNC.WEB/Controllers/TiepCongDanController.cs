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
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using DNC.WEB.Dto;

namespace DNC.WEB.Controllers
{
    public class TiepCongDanController : Controller
    {
        GenericRepository<VuViec> _contextVuViec = null;
        GenericRepository<TiepCongDan> _context = null;
        private TiepCongDanRp _obj = null;
        private VuViecRp _objVuViec = null;
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TiepCongDan()
        {
            return View();
        }

        public ActionResult SoTiepDan()
        {
            return View();
        }

        public async Task<ActionResult> GetDocuments(int iDisplayStart, string sSearch)
        {
            try
            {
                string apiUrl = "https://ubndlaocai.vnptioffice.vn/Ajax/GetFileFromPortalHandler.ashx" +
                        "?key=54b66a2c-4078-4ad1-afeb-0624296029736a239215-c25e-41ce-9838-9fabe47ca7ea" +
                        "&requestType=getListDocumentPaging&sEmail=contact-ubnd&iDisplayStart=" + iDisplayStart + "&sSearch=" + sSearch + "&sDocumentType=0";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(20); // Timeout 20s tránh treo request

                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    var settings = new JsonSerializerSettings
                    {
                        MaxDepth = 1024,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<DocumentResponse>(content, settings);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public ActionResult GetIndex(string IdDonVi, string KieuTiepDan, string IdPhienTCD, string IdDoiTuong, string KeyWord, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa, 
            string IdLoaiVu, string IdLoaiVuKNTC, string IdLoaiVuKNTCChiTiet, string NoiDung, string TuNgay, string DenNgay, string order, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new TiepCongDanRp();
                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                return Json(_obj.SearchAll(IdDonVi, KieuTiepDan, IdPhienTCD, IdDoiTuong, KeyWord, IdTinhThanh, IdQuanHuyen, IdPhuongXa,
                                IdLoaiVu, IdLoaiVuKNTC, IdLoaiVuKNTCChiTiet, NoiDung, TuNgay, DenNgay, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Sổ tiếp dân " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "SoTiepDan/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult GetInfoByID(string id)
        {
            var session = Session["Users"];
            try
            {
                _obj = new TiepCongDanRp();
                return Json(_obj.GetInfoByID(id).First(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Sổ tiếp dân " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "SoTiepDan/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult GetInfoViewByID(string id)
        {
            var session = Session["Users"];
            try
            {
                _obj = new TiepCongDanRp();
                var TiepCongDan = _obj.GetInfoViewByID(id).First();
                if (TiepCongDan.IdDonThu != null)
                {
                    var File = db.FileUpload.Where(x => x.file_type == Constants.FileType_DonThu && x.objects_id == TiepCongDan.IdDonThu).OrderBy(x => x.ngay_tao).ToList();
                    TiepCongDan.FileUpload = new List<FileUpload>();
                    TiepCongDan.FileUpload.AddRange(File);
                }

                return Json(TiepCongDan, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Đơn thư " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DonThu/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Create(TiepCongDan model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    //Insert vu viec
                    _contextVuViec = new GenericRepository<VuViec>();
                    VuViec caseModel = new VuViec();
                    caseModel.NoiDungVuViec = model.NoiDungTiepDan;
                    caseModel.LoaiVuViecId = model.IdLoaiVu;
                    caseModel.LinhVucId = model.IdLoaiVuKNTC;
                    caseModel.NguoiTaoId = model.IdNguoiTiep;
                    caseModel.NgayTao = DateTime.Now;
                    var caseData = _contextVuViec.Insert(caseModel);
                    _contextVuViec.Save();

                    if(model.KieuTiepDan == 0){
                        model.NoiDungTiepDan = "";
                    }
                    _context = new GenericRepository<TiepCongDan>();
                    model.NgayTao = DateTime.Now;
                    model.VuViecId = caseData.Id;
                    var data = _context.Insert(model);
                    _context.Save();
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " Tiếp công dân " + model.HoTen + logs.NotifySuccess, (int)ActionType.ADD, "TiepCongDan/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " Tiếp công dân " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới Tiếp công dân", "Lỗi tạo mới Tiếp công dân", "", "", "TiepCongDan/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Update(TiepCongDan model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<TiepCongDan>();
                model.NgayTao = DateTime.Now;
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " Tiếp công dân " + model.HoTen + logs.NotifySuccess, (int)ActionType.UPDATE, "TiepCongDan/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " Tiếp công dân " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật Tiếp công dân", "Lỗi cập nhật Tiếp công dân", "", "", "TiepCongDan/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }                      
    }
}

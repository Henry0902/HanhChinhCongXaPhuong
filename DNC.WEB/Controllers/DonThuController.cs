
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
    public class DonThuController : Controller
    {
        GenericRepository<DonThu> _context = null;
        private DonThuRp _obj = null;
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        GenericRepository<VuViec> _contextVuViec = null;
        private VuViecRp _objVuViec = null;

        public ActionResult Index()
        {
            return View();
        }        

        public ActionResult TiepNhanDonThu()
        {
            return View();
        }

        public ActionResult DonThuTiepNhan()
        {
            return View();
        }

        public ActionResult DonThuDuyetXuLy()
        {
            return View();
        }

        public ActionResult DonThuGiaiQuyet()
        {
            return View();
        }

        public ActionResult DonThuKetLuan()
        {
            return View();
        }

        public ActionResult DonThuDuyetGiaiQuyet()
        {
            return View();
        }

        public ActionResult DonThuTraKetQua()
        {
            return View();
        }

        public ActionResult DonThuHuy()
        {
            return View();
        }

        public ActionResult TraCuuDonThu()
        {
            return View();
        }

        public ActionResult GetInfoByID(string id)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DonThuRp();
                return Json(_obj.GetInfoByID(id).First(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Đơn thư " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DonThu/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        //public ActionResult GetInfoViewByID(string id)
        //{
        //    var session = Session["Users"];
        //    try
        //    {
        //        _obj = new DonThuRp();
        //        var DonThu = _obj.GetInfoViewByID(id).First();
        //        var File = db.FileUpload.Where(x => x.file_type == Constants.FileType_DonThu && x.objects_id == DonThu.Id).OrderBy(x => x.ngay_tao).ToList();
        //        DonThu.FileUpload = new List<FileUpload>();
        //        DonThu.FileUpload.AddRange(File);
        //        return Json(DonThu, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Đơn thư " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DonThu/GetIndex");
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //}

        public ActionResult GetInfoViewByID(string id)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DonThuRp();
                var DonThu = _obj.GetInfoViewByID(id).First();
                var File = db.FileUpload.Where(x => x.file_type == Constants.FileType_DonThu && x.IdDonThuGoc == DonThu.IdDonThuGoc).OrderBy(x => x.ngay_tao).ToList();
                DonThu.FileUpload = new List<FileUpload>();
                DonThu.FileUpload.AddRange(File);
                return Json(DonThu, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Đơn thư " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DonThu/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult GetTiepNhan(string IdTrangThai, string IdDonViNhap, string IdNguonDon, string TuNgay, string DenNgay, string Keyword, string order, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DonThuRp();
                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                return Json(_obj.SearchTiepNhan(IdTrangThai, IdDonViNhap, IdNguonDon, TuNgay, DenNgay, Keyword, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Đơn thư " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DonThu/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult GetXuLy(string IdTrangThai, string IdDonViXuLy, string IdNguonDon, string TuNgay, string DenNgay, string Keyword, string order, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DonThuRp();
                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                return Json(_obj.SearchXuLy(IdTrangThai, IdDonViXuLy, IdNguonDon, TuNgay, DenNgay, Keyword, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Đơn thư " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DonThu/GetIndex");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult SearchAll(string IdDonVi, string IdNguonDon, string IdDoiTuong, string KeyWord, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa, string IdLoaiDonThu, string IdLoaiKNTC, string IdLoaiKNTCChiTiet
            , string IdDonThuXuLy, string IdTrangThai, string IdHuongXuLy, string NoiDung, string TuNgay, string DenNgay, string order, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DonThuRp();
                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                return Json(_obj.SearchAll(IdDonVi, IdNguonDon, IdDoiTuong, KeyWord, IdTinhThanh, IdQuanHuyen, IdPhuongXa, IdLoaiDonThu, IdLoaiKNTC, IdLoaiKNTCChiTiet
                             , IdDonThuXuLy, IdTrangThai, IdHuongXuLy, NoiDung, TuNgay, DenNgay, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Đơn thư " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DonThu/SearchAll");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult SearchAllHuy(string IdDonVi, string IdNguonDon, string IdDoiTuong, string KeyWord, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa, string IdLoaiDonThu, string IdLoaiKNTC, string IdLoaiKNTCChiTiet
            , string IdDonThuXuLy, string IdTrangThai, string IdHuongXuLy, string NoiDung, string TuNgay, string DenNgay, string IsDelete, string order, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DonThuRp();
                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                return Json(_obj.SearchAllHuy(IdDonVi, IdNguonDon, IdDoiTuong, KeyWord, IdTinhThanh, IdQuanHuyen, IdPhuongXa, IdLoaiDonThu, IdLoaiKNTC, IdLoaiKNTCChiTiet
                             , IdDonThuXuLy, IdTrangThai, IdHuongXuLy, NoiDung, TuNgay, DenNgay, IsDelete, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Đơn thư " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DonThu/SearchAll");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult SearchQuaHan(string IdDonVi, string IdNguonDon, string IdDoiTuong, string KeyWord, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa, string IdLoaiDonThu, string IdLoaiKNTC, string IdLoaiKNTCChiTiet
            , string IdDonThuXuLy, string IdTrangThai, string IdHuongXuLy, string NoiDung, string TuNgay, string DenNgay, string order, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DonThuRp();
                return Json(_obj.SearchQuaHan(order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Đơn thư " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DonThu/SearchAll");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult SearchCheck(string HoTen, string CMTND, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa, string NoiDung, string order, int pageSize, int pageIndex)
        {
            var session = Session["Users"];
            try
            {
                _obj = new DonThuRp();
                return Json(_obj.SearchCheck(HoTen, CMTND, IdTinhThanh, IdQuanHuyen, IdPhuongXa, NoiDung, order, pageSize, pageIndex), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionLoadData, " Đơn thư " + logs.NotifyError, (int)ActionType.GET, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "DonThu/SearchCheck");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Create(DonThu model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    _obj = new DonThuRp();
                    _context = new GenericRepository<DonThu>();
                    model.NgayTao = DateTime.Now;
                    model.IsDelete = 0; // Trạng thái bình thường
                    var data = _context.Insert(model);
                    _context.Save();
                    if (data.Id > 0)
                    {
                        _obj.UpdateDonThuGoc(data.Id);
                    }
                    data.IdDonThuGoc = data.Id;
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " Đơn thư " + model.HoTen + logs.NotifySuccess, (int)ActionType.ADD, "DonThu/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " Đơn thư " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới Đơn Thư", "Lỗi tạo mới Đơn Thư", "", "", "DonThu/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult CreateDonThu(DonThu model)
        {
            var session = Session["Users"];
            try
            {
                if (ModelState.IsValid)
                {
                    //Insert vu viec
                    _contextVuViec = new GenericRepository<VuViec>();
                    VuViec caseModel = new VuViec();
                    caseModel.NoiDungVuViec = model.NoiDungDonThu;
                    caseModel.LoaiVuViecId = model.IdLoaiDonThu;
                    caseModel.LinhVucId = model.IdLoaiKNTC;
                    caseModel.NguoiTaoId = model.IdNguoiNhap;
                    caseModel.NgayTao = DateTime.Now;
                    var caseData = _contextVuViec.Insert(caseModel);
                    _contextVuViec.Save();

                    _obj = new DonThuRp();
                    _context = new GenericRepository<DonThu>();
                    model.NgayTao = DateTime.Now;
                    model.IsDelete = 0; // Trạng thái bình thường
                    model.VuViecId = caseData.Id;
                    var data = _context.Insert(model);
                    _context.Save();
                    if (data.Id > 0)
                    {
                        _obj.UpdateDonThuGoc(data.Id);
                    }
                    data.IdDonThuGoc = data.Id;
                    logs.SystemLog_Insert_Success((int)session, logs.ActionCreate, " Đơn thư " + model.HoTen + logs.NotifySuccess, (int)ActionType.ADD, "DonThu/Create");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " Đơn thư " + logs.NotifyError, (int)ActionType.ADD, "Lỗi tạo mới Đơn Thư", "Lỗi tạo mới Đơn Thư", "", "", "DonThu/Create");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Update(DonThu model)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<DonThu>();
                model.NgayTao = DateTime.Now;
                model.IsDelete = 0; // Trạng thái bình thường
                _context.Update(model);
                _context.Save();
                logs.SystemLog_Insert_Success((int)session, logs.ActionUpdate, " Đơn Thư " + model.HoTen + logs.NotifySuccess, (int)ActionType.UPDATE, "DonThu/Update");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionUpdate, " Đơn Thư " + logs.NotifyError, (int)ActionType.UPDATE, "Lỗi cập nhật Đơn Thư", "Lỗi cập nhật Đơn Thư", "", "", "DonThu/Update");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult UpdateHuongXuLy(int Id, int IdHuongXuLy, int IdDonViXuLy, int IdDonViXacMinh, int IdDonViTiepNhan, int IdCanBoXuLy,int IdNguoiTao, string NgayThoiHanThuLy, int IdTrangThai)
        {
            _obj = new DonThuRp();
            Dictionary<string, object> jsonResult = new Dictionary<string, object>();
            DonThu model = new DonThu();
            try
            {
                if (NgayThoiHanThuLy != "")
                {
                    NgayThoiHanThuLy = DateTime.ParseExact(NgayThoiHanThuLy, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                }
                var result = _obj.UpdateHuongXuLy(Id, IdHuongXuLy, IdDonViXuLy, IdDonViXacMinh, IdDonViTiepNhan, IdCanBoXuLy, IdNguoiTao, NgayThoiHanThuLy, IdTrangThai);
                if (result)
                {
                    jsonResult.Add(Constants.STATUS, true);
                    if (IdHuongXuLy == Constants.HXL_CHUYEN_DON && IdTrangThai == Constants.STATUS_DUYET_CHUYEN_DON)
                    {
                        _context = new GenericRepository<DonThu>();
                        var data = _context.Get(Id);
                        if(data.Id > 0){
                            jsonResult.Add(Constants.CODE, IdHuongXuLy);
                            model.IdTiepCongDan = data.IdTiepCongDan;
                            model.IdDonThuXuLy = data.IdDonThuXuLy;
                            //Chuyển đơn thi IdNguonDon = 6
                            model.IdNguonDon = 6; 
                            model.SoVanBan = data.SoVanBan;
                            model.NgayNhap = data.NgayNhap;
                            model.IdDoiTuong = data.IdDoiTuong;
                            model.SoNguoi = data.SoNguoi;
                            model.HoTen = data.HoTen;
                            model.SoGiayTo = data.SoGiayTo;
                            model.GioiTinh = data.GioiTinh;
                            model.NgayCap = data.NgayCap;
                            model.NoiCap = data.NoiCap;
                            model.DiaChi = data.DiaChi;
                            model.IdPhuongXa = data.IdPhuongXa;
                            model.IdQuanHuyen = data.IdQuanHuyen;
                            model.IdTinhThanh = data.IdTinhThanh;
                            model.IdQuocTich = data.IdQuocTich;
                            model.IdDanToc = data.IdDanToc;
                            model.IdLoaiDonThu = data.IdLoaiDonThu;
                            model.IdLoaiKNTC = data.IdLoaiKNTC;
                            model.IdLoaiKNTCChiTiet = data.IdLoaiKNTCChiTiet;
                            //model.LanGiaiQuyet = data.LanGiaiQuyet;
                            model.LanTiepNhan = data.LanTiepNhan;
                            model.Pre_Status = data.Pre_Status;
                            model.NoiDungDonThu = data.NoiDungDonThu;
                            model.IdDonViNhap = IdDonViTiepNhan;
                            model.IdNguoiNhap = data.IdNguoiNhap;
                            model.NgayThoiHanThuLy = data.NgayThoiHanThuLy;
                            model.IdTrangThai = 2;
                            model.NgayTao = DateTime.Now;
                            model.IsDelete = 0; // Trạng thái bình thường
                            model.VuViecId = data.VuViecId;
                            model.IdDonThuGoc = data.IdDonThuGoc;

                            var dataResult = _context.Insert(model);
                            _context.Save();
                            jsonResult.Add(Constants.DATA, dataResult);
                            return Json(jsonResult, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            model.Id = 0;
                            jsonResult.Add(Constants.DATA, model);
                        }
                    }
                    else
                    {
                        model.Id = 0;
                        jsonResult.Add(Constants.DATA, model);
                        
                    }

                    return Json(jsonResult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.Id = 0;
                    jsonResult.Add(Constants.DATA, model);
                    jsonResult.Add(Constants.STATUS, false);
                    return Json(jsonResult, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception)
            {
                model.Id = 0;
                jsonResult.Add(Constants.DATA, model);
                jsonResult.Add(Constants.STATUS, false);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateHuongPheDuyetXuLy(int Id, int IdHuongXuLy, int IdDonViXuLy, int IdDonViXacMinh, int IdDonViTiepNhan, string NgayThoiHanThuLy, int IdTrangThai)
        {
            _obj = new DonThuRp();
            Dictionary<string, object> jsonResult = new Dictionary<string, object>();
            DonThu model = new DonThu();
            try
            {
                if (NgayThoiHanThuLy != "")
                {
                    NgayThoiHanThuLy = DateTime.ParseExact(NgayThoiHanThuLy, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                }
                var result = _obj.UpdateHuongPheDuyetXuLy(Id, IdHuongXuLy, IdDonViXuLy, IdDonViXacMinh, IdDonViTiepNhan, NgayThoiHanThuLy, IdTrangThai);
                if (result)
                {
                    jsonResult.Add(Constants.STATUS, true);
                    if (IdHuongXuLy == Constants.HXL_CHUYEN_DON && IdTrangThai == Constants.STATUS_DUYET_CHUYEN_DON)
                    {
                        _context = new GenericRepository<DonThu>();
                        var data = _context.Get(Id);
                        if (data.Id > 0)
                        {
                            jsonResult.Add(Constants.CODE, IdHuongXuLy);
                            model.IdTiepCongDan = data.IdTiepCongDan;
                            model.IdDonThuXuLy = data.IdDonThuXuLy;
                            model.IdNguonDon = data.IdNguonDon;
                            model.SoVanBan = data.SoVanBan;
                            model.NgayNhap = data.NgayNhap;
                            model.IdDoiTuong = data.IdDoiTuong;
                            model.SoNguoi = data.SoNguoi;
                            model.HoTen = data.HoTen;
                            model.SoGiayTo = data.SoGiayTo;
                            model.GioiTinh = data.GioiTinh;
                            model.NgayCap = data.NgayCap;
                            model.NoiCap = data.NoiCap;
                            model.DiaChi = data.DiaChi;
                            model.IdPhuongXa = data.IdPhuongXa;
                            model.IdQuanHuyen = data.IdQuanHuyen;
                            model.IdTinhThanh = data.IdTinhThanh;
                            model.IdQuocTich = data.IdQuocTich;
                            model.IdDanToc = data.IdDanToc;
                            model.IdLoaiDonThu = data.IdLoaiDonThu;
                            model.IdLoaiKNTC = data.IdLoaiKNTC;
                            model.IdLoaiKNTCChiTiet = data.IdLoaiKNTCChiTiet;
                            //model.LanGiaiQuyet = data.LanGiaiQuyet;
                            model.LanTiepNhan = data.LanTiepNhan;
                            model.Pre_Status = data.Pre_Status;
                            model.NoiDungDonThu = data.NoiDungDonThu;
                            model.IdDonViNhap = IdDonViTiepNhan;
                            model.IdNguoiNhap = data.IdNguoiNhap;
                            model.NgayThoiHanThuLy = data.NgayThoiHanThuLy;
                            model.IdTrangThai = 2;
                            model.NgayTao = DateTime.Now;
                            model.IsDelete = 0; // Trạng thái bình thường
                            model.VuViecId = data.VuViecId;
                            model.IdDonThuGoc = data.IdDonThuGoc;

                            var dataResult = _context.Insert(model);
                            _context.Save();

                            jsonResult.Add(Constants.DATA, dataResult);
                            return Json(jsonResult, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            model.Id = 0;
                            jsonResult.Add(Constants.DATA, model);
                        }
                    }
                    else
                    {
                        model.Id = 0;
                        jsonResult.Add(Constants.DATA, model);

                    }

                    return Json(jsonResult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.Id = 0;
                    jsonResult.Add(Constants.DATA, model);
                    jsonResult.Add(Constants.STATUS, false);
                    return Json(jsonResult, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                model.Id = 0;
                jsonResult.Add(Constants.DATA, model);
                jsonResult.Add(Constants.STATUS, false);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateTrangThai(int Id, int IdTrangThai)
        {
            _obj = new DonThuRp();
            try
            {
                _obj.UpdateTrangThai(Id, IdTrangThai);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UpdateIsDelete(int Id, int IsDelete)
        {
            _obj = new DonThuRp();
            try
            {
                _obj.UpdateIsDelete(Id, IsDelete);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

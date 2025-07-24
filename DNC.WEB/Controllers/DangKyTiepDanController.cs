using System;
using System.Net;
using System.Web.Mvc;
using DNC.WEB.Models;
using DNC.WEB.Repository;
using DNC.WEB.Common;
using System.IO;
using System.Globalization;
using System.Linq;

namespace DNC.WEB.Controllers
{
    public class DangKyTiepDanController : Controller
    {
        GenericRepository<DangKyTiepDan> _context = null;
        GenericRepository<DangKyTiepDanFile> _contextFile = null;
        DbConnectContext db = new DbConnectContext();
        private DangKyTiepDanRp _obj = null;

        public DangKyTiepDanController()
        {
            _context = new GenericRepository<DangKyTiepDan>();
            _contextFile = new GenericRepository<DangKyTiepDanFile>();
            _obj = new DangKyTiepDanRp();
        }

        public ActionResult RegisterList()
        {
            return View();
        }

        public ActionResult RegisterListAdmin()
        {
            return View();
        }

        public ActionResult RegisterAdd()
        {
            return View();
        }

        public ActionResult RegisterEdit(int id)
        {
            return View();
        }

        public ActionResult RegisterDetail(int id)
        {
            return View();
        }

        public ActionResult RegisterDetailAdmin(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            var data = _obj.GetByIdWithFiles(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        public ActionResult GetListRegister(int pageIndex, int pageSize, string content, int status, int department, string registerDate, int? idCongDan)
        {
            DateTime tempDate;
            DateTime? parsedDate = DateTime.TryParseExact(registerDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate) ? tempDate : (DateTime?)null;
            var result = _obj.SearchAll(pageIndex, pageSize, status, content, department, parsedDate, idCongDan);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(CreateOrUpdateDangKyTiepDanRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = new DangKyTiepDan();
                    entity.IdCongDan = request.IdCongDan;
                    entity.IdDonViTiepDan = request.IdDonViTiepDan;
                    entity.MaDangKy = _obj.GeneratedCode(request.IdDonViTiepDan.ToString(), DateTime.Now);

                    if (!string.IsNullOrEmpty(request.NgayDangKy))
                    {
                        entity.NgayDangKy = DateTime.ParseExact(request.NgayDangKy, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                    entity.ChuDe = request.ChuDe;
                    entity.TrangThai = request.TrangThai;
                    entity.NgayTao = DateTime.Now;

                    var data = _context.Insert(entity);
                    _context.Save();
                    return Json(new DataResponse("Thêm mới đăng ký tiếp dân thành công", data, 200), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, int status, string content, DateTime? appointment, int? idRecepion)
        {
            var session = Session["Users"];
            try
            {
                var entity = db.DangKyTiepDan.Find(id);
                if (entity != null)
                {
                    entity.TrangThai = status;
                    entity.NgayCapNhat = DateTime.Now;

                    if (status == 2)
                    {
                        entity.IdNguoiXuLy = (int)session;
                        entity.NgayTiepNhan = DateTime.Now;
                    }
                    else if (status == 3 || status == 4)
                    {
                        entity.NgayXuLy = DateTime.Now;
                        entity.KetQuaXuLy = content;
                        if (status == 3)
                        {
                            entity.ThoiGianHen = appointment;
                            entity.NoiTiep = idRecepion;
                        }
                    }
                    else if (status == 5)
                    {
                        entity.NgayHuy = DateTime.Now;
                        entity.LyDoHuy = content;
                    }

                    _context.Update(entity);
                    _context.Save();
                    return Json(entity, JsonRequestBehavior.AllowGet);
                }

                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Update(int id, CreateOrUpdateDangKyTiepDanRequest request)
        {
            var session = Session["Users"];
            try
            {
                if (request.FileDelete != null && request.FileDelete.Any())
                {
                    foreach (var file in request.FileDelete)
                    {
                        _contextFile.Delete(file);
                    }
                }

                var entity = db.DangKyTiepDan.Find(id);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy bản ghi nào");
                }

                if (request.TrangThai == 1)
                {
                    entity.NgayGui = DateTime.Now;
                }

                entity.TrangThai = request.TrangThai;
                if (entity.IdDonViTiepDan != request.IdDonViTiepDan)
                {
                    entity.IdDonViTiepDan = request.IdDonViTiepDan;
                    entity.MaDangKy = _obj.GeneratedCode(request.IdDonViTiepDan.ToString(), DateTime.Now);
                }
                entity.ChuDe = request.ChuDe;

                if (!string.IsNullOrEmpty(request.NgayDangKy))
                {
                    entity.NgayDangKy = DateTime.ParseExact(request.NgayDangKy, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(request.LyDoHuy))
                {
                    entity.LyDoHuy = request.LyDoHuy;
                    entity.NgayHuy = DateTime.Now;
                }

                entity.NgayCapNhat = DateTime.Now;
                _context.Update(entity);

                _contextFile.Save();
                _context.Save();

                return Json(entity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult UploadFile(int idDangKy)
        {
            DangKyTiepDanFile fileUpload = null;
            try
            {
                if (Request.Files != null)
                {
                    string pathFile = "";
                    string fileNameOriginal = ""; //Ten file goc
                    string fileExtension = "";//Duoi mo rong: .xslx, .pdf, ...vv
                    foreach (string files in Request.Files)
                    {
                        var file = Request.Files[files];
                        string year = DateTime.Now.Year.ToString();
                        string month = DateTime.Now.Month.ToString();
                        string day = DateTime.Now.Day.ToString();
                        string hour = DateTime.Now.Hour.ToString();
                        string minute = DateTime.Now.Minute.ToString();
                        string second = DateTime.Now.Second.ToString();
                        string millisecond = DateTime.Now.Millisecond.ToString();
                        string time = hour + "_" + minute + "_" + second + "_" + millisecond;

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
                        pathFile = Constants.PATH_FOLDER_FILESUPLOAD + year + "/" + month + "/" + day;
                        fileNameOriginal = file.FileName;
                        fileExtension = "." + fileNameOriginal.Split('.')[1];
                        FileInfo fi = new FileInfo(fileNameOriginal);
                        fileExtension = fi.Extension;

                        string name = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;
                        string fileNameSave = name + "_" + fileNameOriginal.Replace(" ", "_");

                        if (System.IO.File.Exists(Server.MapPath(pathFile + "/" + fileNameSave)))
                        {
                            file.SaveAs(Path.Combine(Server.MapPath(pathFile), fileNameSave));
                        }
                        else
                        {
                            file.SaveAs(Path.Combine(Server.MapPath(pathFile), fileNameSave));
                        }

                        fileUpload = new DangKyTiepDanFile()
                        {
                            IdDangKy = idDangKy,
                            TenTep = fileNameSave,
                            Url = pathFile + "/" + fileNameSave,
                            Path = pathFile,
                        };

                        db.DangKyTiepDanFile.Add(fileUpload);
                        db.SaveChanges();
                    }
                }
                return Json(fileUpload, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(fileUpload, JsonRequestBehavior.AllowGet);
            }
        }

        public void Delete(int id)
        {
            var session = Session["Users"];
            try
            {
                if (_context.Get(id) != null)
                {
                    _context.Delete(id);
                    _context.Save();

                    var listFile = db.DangKyTiepDanFile.Where(x => x.IdDangKy == id).ToList();
                    if (listFile.Any())
                    {
                        foreach (var file in listFile)
                        {
                            db.DangKyTiepDanFile.Remove(file);
                        }
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
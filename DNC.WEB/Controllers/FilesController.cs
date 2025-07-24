using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DNC.CM;
using DNC.WEB.Common;
using DNC.WEB.Models;
using DNC.WEB.Repository;

namespace DNC.WEB.Controllers
{
    public class FilesController : Controller
    {
        //
        // GET: /Files/
        private GenericRepository<FileUpload> _context = null;
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateFile(int objectId, int type, int idDonThuGoc)
        {
            FileUpload fileUpload = null;
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
                        
                        fileUpload = new FileUpload()
                        {
                            objects_id = objectId,
                            file_name = Guid.NewGuid() + Path.GetExtension(fileNameOriginal),
                            file_extension = fileExtension,
                            file_url = pathFile + "/" + fileNameSave,
                            file_original = fileNameOriginal,
                            folder_path = pathFile,
                            trang_thai = Constants.STATUS_ACTIVE,
                            file_type = type,
                            user_created_id = 0,
                            ngay_tao = DateTime.Now,
                            IdDonThuGoc = idDonThuGoc
                        };
                        db.FileUpload.Add(fileUpload);
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

        public ActionResult GetAllFileByObjectId(int Id, int FileType)
        {
            return Json(
                    db.FileUpload.Where(x => x.file_type == FileType && x.objects_id == Id).OrderBy(x => x.ngay_tao).ToList()
                    , JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFiles(int id)
        {
            var session = Session["Users"];
            try
            {
                _context = new GenericRepository<FileUpload>();
                FileUpload file = db.FileUpload.FirstOrDefault(x => x.Id == id);
                if (file != null && file.Id == id)
                {
                    string path = Server.MapPath(file.file_url);
                    CommonFunc.DeleteFile(path, 3);
                    _context.Delete(id);
                    _context.Save();
                }
                
                logs.SystemLog_Insert_Success((int)session, logs.ActionDelete, " files " + id + logs.NotifySuccess, (int)ActionType.DELETE, "Files/DeleteFiles");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.SystemLog_Insert_Error((int)session, logs.ActionDelete, " files " + id + logs.NotifyError, (int)ActionType.DELETE, ex.Message, ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "Files/DeleteFiles");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

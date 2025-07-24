using System;
using System.Net;
using System.Linq;
using System.Web.Mvc;
using DNC.WEB.Models;
using DNC.WEB.Repository;
using System.IO;
using Aspose.Words;

namespace DNC.WEB.Controllers
{
    public class VanBanMauController : Controller
    {
        //
        // GET: /VanBanMau/
        GenericRepository<DonThu> _context = null;
        private DonThuRp _obj = null;
        DbConnectContext db = new DbConnectContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrintMau14(string Id)
        {
            var session = Session["Users"];
            try
            {
                string tempfile = Server.MapPath("~/Exports/Templates/Mau14.doc");
                string filename = "Mau14" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".doc";
                string foldername = "/Exports/Files/";

                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }

                string savefile = Server.MapPath(foldername) + filename;

                Document doc = new Document(tempfile);

                _obj = new DonThuRp();
                DonThuInfoPrint donthu = _obj.GetInfoPrintByID(Id).First();

                doc.MailMerge.UseNonMergeFields = true;
                doc.MailMerge.Execute(
                    new string[] { "TenLoaiDonThu", "NgayNhap", "HoTen", "DiaChi", "NoiDungDonThu" },
                    new object[] { donthu.TenLoaiDonThu, donthu.NgayNhap, donthu.HoTen, donthu.DiaChi, donthu.NoiDungDonThu });

                doc.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }        

        public ActionResult PrintMau21(string Id)
        {
            var session = Session["Users"];
            try
            {
                string tempfile = Server.MapPath("~/Exports/Templates/Mau21.doc");
                string filename = "Mau21" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".doc";
                string foldername = "/Exports/Files/";

                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }

                string savefile = Server.MapPath(foldername) + filename;

                Document doc = new Document(tempfile);

                _obj = new DonThuRp();
                DonThuInfoPrint donthu = _obj.GetInfoPrintByID(Id).First();

                doc.MailMerge.UseNonMergeFields = true;
                doc.MailMerge.Execute(
                    new string[] { "NgayNhap", "HoTen", "NoiDungDonThu" },
                    new object[] { donthu.NgayNhap, donthu.HoTen, donthu.NoiDungDonThu });

                doc.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult PrintMau22(string Id)
        {
            var session = Session["Users"];
            try
            {
                string tempfile = Server.MapPath("~/Exports/Templates/Mau22.doc");
                string filename = "Mau22" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".doc";
                string foldername = "/Exports/Files/";

                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }

                string savefile = Server.MapPath(foldername) + filename;

                Document doc = new Document(tempfile);

                _obj = new DonThuRp();
                DonThuInfoPrint donthu = _obj.GetInfoPrintByID(Id).First();

                doc.MailMerge.UseNonMergeFields = true;
                doc.MailMerge.Execute(
                    new string[] { "NgayNhap", "SoVanBan", "HoTen", "DiaChi", "NoiDungDonThu" },
                    new object[] { donthu.NgayNhap, donthu.SoVanBan, donthu.HoTen, donthu.DiaChi,donthu.NoiDungDonThu });

                doc.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult PrintMau23(string Id)
        {
            var session = Session["Users"];
            try
            {
                string tempfile = Server.MapPath("~/Exports/Templates/Mau23.doc");
                string filename = "Mau23" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".doc";
                string foldername = "/Exports/Files/";

                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }

                string savefile = Server.MapPath(foldername) + filename;

                Document doc = new Document(tempfile);

                _obj = new DonThuRp();
                DonThuInfoPrint donthu = _obj.GetInfoPrintByID(Id).First();

                doc.MailMerge.UseNonMergeFields = true;
                doc.MailMerge.Execute(
                    new string[] { "HoTen", "DiaChi", "NoiDungDonThu" },
                    new object[] { donthu.HoTen, donthu.DiaChi, donthu.NoiDungDonThu });

                doc.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult PrintMau30 (string Id)
        {
            var session = Session["Users"];
            try
            {
                string tempfile = Server.MapPath("~/Exports/Templates/Mau30.doc");
                string filename = "Mau30" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".doc";
                string foldername = "/Exports/Files/";

                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }

                string savefile = Server.MapPath(foldername) + filename;

                Document doc = new Document(tempfile);

                _obj = new DonThuRp();
                DonThuInfoPrint donthu = _obj.GetInfoPrintByID(Id).First();

                doc.MailMerge.UseNonMergeFields = true;
                doc.MailMerge.Execute(
                    new string[] { "HoTen", "DiaChi", "NoiDungDonThu" },
                    new object[] { donthu.HoTen, donthu.DiaChi, donthu.NoiDungDonThu });

                doc.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        private void DeleteFile(string path)
        {
            string[] filename = Directory.GetFiles(path);
            foreach (string file in filename)
            {
                if (System.IO.File.Exists(file) == true)
                {
                    System.IO.File.Delete(file);
                }
            }
        }  

    }
}

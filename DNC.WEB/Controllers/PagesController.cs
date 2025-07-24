using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using DNC.WEB.Models;
using DNC.WEB.Repository;
//using log4net;
//using THANHTRA_V2.LogException;

namespace DNC.WEB.Controllers
{
    public class PagesController : Controller
    {
        //
        // GET: /Pages/
        GenericRepository<Pages> _context = null;
        private PagesRp _obj = null;
        private DbConnectContext _db = new DbConnectContext();

        public ActionResult Index()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index?url_redirect=" + Request.Url.AbsoluteUri);
            }
            return View();
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            _context = new GenericRepository<Pages>();
            var data = _context.Get(id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            _obj = new PagesRp();
            var session = Session["Users"];
            //var session = 5;
            return Json(_obj.GetAllPage((int)session).OrderBy(x => x.ParentId).ThenBy(x => x.OrderNo), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetPagesFunctionRole(string userId, string roleId)
        {
            _obj = new PagesRp();
            return Json(_obj.GetPagesByRoleId(userId, roleId), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllSuper()
        {
            _obj = new PagesRp();
            return Json(_obj.GetAll().OrderBy(x => x.ParentId).ThenBy(x => x.OrderNo), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DirectoryLink()
        {
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/Views"));
            var filesListing = directory.GetDirectories();

            List<string> direclist = new List<string>();
            for (int i = 0; i < filesListing.Length; i++)
            {
                DirectoryInfo subdirectory = new DirectoryInfo(Server.MapPath("~/Views/" + (filesListing[i].ToString())));
                var file = subdirectory.GetFiles();

                for (int j = 0; j < file.Length; j++)
                {
                    string ext = Path.GetExtension(file[j].Name);
                    string result = file[j].ToString().Substring(0, file[j].ToString().Length - ext.Length);
                    string filepath = filesListing[i] + "/" + result;
                    direclist.Add(filepath);
                }
            }
            return Json(direclist, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult GenUrl(string path)
        //{
        //    string strUrl = "";
        //    if (path == "" || String.IsNullOrEmpty(path))
        //    {
        //        strUrl = "/";
        //        return Json(strUrl, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        strUrl = StringHelper.ToUnsignString(path).ToLower();
        //        return Json(strUrl, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpGet]
        public void GetLink(string link)
        {
            try
            {
                string url = link.Remove(1, 5).Replace("/", "").Trim();
                Session.Remove("CurentUrl");
                Session["CurentUrl"] = url;
            }
            catch (Exception)
            {

            }
        }
        [HttpGet]
        public ActionResult GetPagesIDbyLink(string path)
        {
            _obj = new PagesRp();
            return Json(_obj.GetLinkIdbyLink(path), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getForRole()
        {
            _obj = new PagesRp();
            return Json(_obj.GetPages().OrderBy(x => x.ParentId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CountChild(int id)
        {
            _obj = new PagesRp();
            return Json(_obj.CountByParentId(id), JsonRequestBehavior.AllowGet);
        }
        // Hàm Dispose Connection
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult ThongBaoCount(string IdDonVi)
        {
            _obj = new PagesRp();
            return Json(_obj.ThongBaoCount(IdDonVi), JsonRequestBehavior.AllowGet);
        }
    }
}
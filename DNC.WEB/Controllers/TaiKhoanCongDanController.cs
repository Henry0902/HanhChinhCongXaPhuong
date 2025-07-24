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
    public class TaiKhoanCongDanController : Controller
    {
        private TaiKhoanCongDanRp _obj = null;

        public TaiKhoanCongDanController()
        {
            _obj = new TaiKhoanCongDanRp();
        }

        public ActionResult CitizenList()
        {
            return View();
        }

        public ActionResult CitizenDetail(int id)
        {
            return View();
        }

        public ActionResult GetListCitizen(string keyword, string createdDate, string idCard, int gender, int status, string address, int pageNumber, int pageSize)
        {
            DateTime tempDate;
            DateTime? parsedDate = DateTime.TryParseExact(createdDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate) ? tempDate : (DateTime?)null;
            var result = _obj.SearchAll(keyword, parsedDate, idCard, gender, status, address, pageNumber, pageSize);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
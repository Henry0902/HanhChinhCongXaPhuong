using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DNC.WEB.Common;
using DNC.WEB.Models;
using DNC.WEB.Repository;

namespace DNC.WEB.Controllers
{
    public class APIChucVuController : ApiController
    {
        GenericRepository<DmChucVu> _context = null;
        DbConnectContext db = new DbConnectContext();
        // GET api/APIChucVu/DmChucVu
        [HttpGet]
        public IDictionary<string, object> GetList()
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();

            _context = new GenericRepository<DmChucVu>();
            List<DmChucVu> ListChucVu = _context.GetAll().ToList();

            try
            {
                dic.Add(Constants.CODE, Constants.SUCCESS_CODE);
                dic.Add(Constants.STATUS, true);
                dic.Add(Constants.DATA, ListChucVu);
                dic.Add(Constants.MESSAGE, Constants.SUCCESS_MESSAGE);
            }
            catch (Exception ex)
            {
                dic.Add(Constants.CODE, Constants.ERROR);
                dic.Add(Constants.STATUS, false);
                dic.Add(Constants.MESSAGE, Constants.ERROR_MESSAGE);
                return dic;
            }
            return dic;
        }

        [HttpGet]
        public List<DmChucVu> GetListtest()
        {
            _context = new GenericRepository<DmChucVu>();
            List<DmChucVu> ListChucVu = _context.GetAll().ToList();
            return ListChucVu;
        }

        public List<string> Get()
        {
            return new List<string> {  
                "Data1",  
                "Data2"  
            };
        }  
    }
}

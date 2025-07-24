using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Controllers;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class VuViecRp
    {
        private GenericRepository<VuViec> _context = null;
        private DbConnectContext _obj;
        //private Ultils logs = new Ultils();
        public VuViecRp()
        {
            _obj = new DbConnectContext();
        }
    }
}
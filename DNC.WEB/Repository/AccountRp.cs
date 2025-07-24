using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class AccountRp
    {
        private DbConnectContext _db = null;
        public AccountRp()
        {
            _db = new DbConnectContext();
        }
       
        public SystemConfigs SystemConfigs_GetAll_ByKey(string key)
        {
            var data = _db.Database.SqlQuery<SystemConfigs>("SystemConfigs_GetAll_ByKey @Key",
             new SqlParameter("@Key", key)).FirstOrDefault();
            return data;
        }
    }
}
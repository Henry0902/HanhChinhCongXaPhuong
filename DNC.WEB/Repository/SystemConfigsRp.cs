using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Controllers;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class SystemConfigsRp
    {
        private GenericRepository<SystemConfigs> _context = null;
        private DbConnectContext _obj;
        //private Ultils logs = new Ultils();
        public SystemConfigsRp()
        {
            _obj = new DbConnectContext();
        }
        // Khoi tao du lieu
        public SystemConfigs Create(SystemConfigs info)
        {
            SystemConfigs data = null;              
            data = _context.Insert(info);
            _context.Save();
            return data;
        }
        // Lay du lieu theo Id
        public SystemConfigsSearch GetById(int id)
        {
            var data = _obj.Database.SqlQuery<SystemConfigsSearch>("SystemConfig_GetById @Id", new SqlParameter("@Id", id)).FirstOrDefault();
            return data;
        }
        public List<SystemConfigsSearch> SearchAll(string keyword, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<SystemConfigsSearch>("SystemConfigs_Search @Keyword, @PageSize, @PageIndex",
                    new SqlParameter("@Keyword", keyword),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex)
                    ).ToList();
            return data;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class SystemLogsRp
    {
        private GenericRepository<SystemLogs> _context = null;
        private DbConnectContext _obj;
        public SystemLogsRp()
        {
            _obj = new DbConnectContext();
        }

        public SystemLogs Create(SystemLogs info)
        {
            SystemLogs data = null;              
            data = _context.Insert(info);
            _context.Save();
            return data;
        }

        public List<SystemLogsSearch> SearchAll(string keyword, int type, int status,string dateFrom, string dateTo, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<SystemLogsSearch>("SystemLogs_Search @Keyword,@Type, @Status, @DateFrom, @DateTo, @PageSize, @PageIndex",
            new SqlParameter("@Keyword", keyword),
            new SqlParameter("@Type", type),
            new SqlParameter("@Status", status),
            new SqlParameter("@DateFrom", dateFrom ?? (object)DBNull.Value),
            new SqlParameter("@DateTo", dateTo ?? (object)DBNull.Value),
            new SqlParameter("@PageSize", pageSize),
            new SqlParameter("@PageIndex", pageIndex)
            ).ToList();
            return data;
        }

        public void DeleteByTime(int time)
        {
            _obj = new DbConnectContext();
            var data = _obj.Database.ExecuteSqlCommand("SystemLogs_DeleteByTime @Time",
            new SqlParameter("@Time", time)
            );
        }
    }
}
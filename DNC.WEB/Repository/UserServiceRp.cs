using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class UserServiceRp
    {
        private GenericRepository<UserService> _context = null;
        private DbConnectContext _obj;

        public UserServiceRp()
        {
            _obj = new DbConnectContext();
        }

        public List<UserService> SearchAll(string strSearch, string trang_thai, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<UserService>("Userservice_Search @strSearch, @trang_thai, @PageSize, @PageIndex",
                    new SqlParameter("@strSearch", strSearch),
                    new SqlParameter("@trang_thai", trang_thai),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex)
                    ).ToList();
            return data;
        }

        public int GetTotal(string strSearch, string trang_thai)
        {
            var lstData = _obj.Database.SqlQuery<UserserviceCount>("Userservice_Count @strSearch, @trang_thai",
                    new SqlParameter("@strSearch", strSearch),
                    new SqlParameter("@trang_thai", trang_thai)
                    ).ToList();
            if (lstData.Count > 0)
            {
                return lstData.ElementAt(0).TotalRecords;
            }
            return 0;
        }

        public bool UpdateStatus(int userServiceId, int status)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("Userservice_UpdateStatus @userservice_id,@trang_thai",
                        new SqlParameter("@userservice_id", userServiceId),
                        new SqlParameter("@trang_thai", status));
                    trans.Commit();
                    return true;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }
    }
}
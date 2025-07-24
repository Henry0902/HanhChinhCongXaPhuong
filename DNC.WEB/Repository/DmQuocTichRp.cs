using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DmQuocTichRp
    {
        private GenericRepository<DmQuocTich> _context = null;
        private DbConnectContext _obj;

        public DmQuocTichRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DmQuocTichSearch> SearchAll(string thongtintimkiem, int trang_thai, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DmQuocTichSearch>("DmQuocTich_Search @thongtintimkiem,@trang_thai,@Order, @PageSize, @PageIndex",
                    new SqlParameter("@thongtintimkiem", thongtintimkiem),
                    new SqlParameter("@trang_thai", trang_thai),
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex)
                    ).ToList();
            return data;
        }

        public bool UpdateStatus(int id, int status)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("DmQuocTich_UpdateStatus @duan_id,@trang_thai",
                        new SqlParameter("@duan_id", id),
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
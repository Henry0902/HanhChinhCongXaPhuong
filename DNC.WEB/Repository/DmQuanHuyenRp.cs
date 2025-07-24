using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DmQuanHuyenRp
    {
        private GenericRepository<DmQuanHuyen> _context = null;
        private DbConnectContext _obj;

        public DmQuanHuyenRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DmQuanHuyenSearch> SearchAll(string thongtintimkiem, int trang_thai, string order, int pageSize, int pageIndex, int idtinhthanh)
        {
            var data = _obj.Database.SqlQuery<DmQuanHuyenSearch>("DmQuanHuyen_Search @thongtintimkiem,@trang_thai,@Order, @PageSize, @PageIndex,@idtinhthanh",
                    new SqlParameter("@thongtintimkiem", thongtintimkiem),
                    new SqlParameter("@trang_thai", trang_thai),
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex),
                    new SqlParameter("@idtinhthanh", idtinhthanh)
                    ).ToList();
            return data;
        }

        public bool UpdateStatus(int id, int status)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("DmQuanHuyen_UpdateStatus @duan_id,@trang_thai",
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
        public int check_delete(int id)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    int a = _obj.Database.SqlQuery<int>("DmQuanHuyen_CheckDelete @id",
                        new SqlParameter("@id", id)).FirstOrDefault();
                    return a;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return 0;
                }
            }
        }
    }
}
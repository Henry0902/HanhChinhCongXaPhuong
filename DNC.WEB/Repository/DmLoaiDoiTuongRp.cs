using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DmLoaiDoiTuongRp
    {
        private GenericRepository<DmLoaiDoiTuong> _context = null;
        private DbConnectContext _obj;

        public DmLoaiDoiTuongRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DmLoaiDoiTuongSearch> SearchAll(string thongtintimkiem, int trang_thai, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DmLoaiDoiTuongSearch>("DmLoaiDoiTuong_Search @thongtintimkiem,@trang_thai,@Order, @PageSize, @PageIndex",
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
                    _obj.Database.ExecuteSqlCommand("DmLoaiDoiTuong_UpdateStatus @duan_id,@trang_thai",
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
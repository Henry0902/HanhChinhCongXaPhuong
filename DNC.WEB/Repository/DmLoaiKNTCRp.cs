using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DmLoaiKNTCRp
    {
        private GenericRepository<DmLoaiKNTC> _context = null;
        private DbConnectContext _obj;

        public DmLoaiKNTCRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DmLoaiKNTCSearch> SearchAll(string thongtintimkiem,string code, int trang_thai, string order, int pageSize, int pageIndex, int idloaidonthu, int idnguondonthu, int loai)
        {
            var data = _obj.Database.SqlQuery<DmLoaiKNTCSearch>("DmLoaiKNTC_Search @thongtintimkiem,@code,@trang_thai,@Order, @PageSize, @PageIndex,@idloaidonthu,@idnguondonthu, @loai",
                    new SqlParameter("@thongtintimkiem", thongtintimkiem),
                    new SqlParameter("@code", code),
                    new SqlParameter("@trang_thai", trang_thai),
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex),
                    new SqlParameter("@idloaidonthu", idloaidonthu),
                    new SqlParameter("@idnguondonthu", idnguondonthu),
                    new SqlParameter("@loai", loai)
                    ).ToList();
            return data;
        }

        public bool UpdateStatus(int id, int status)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("DmLoaiKNTC_UpdateStatus @duan_id,@trang_thai",
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
                    int a = _obj.Database.SqlQuery<int>("DmLoaiKNTC_CheckDelete @id",
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
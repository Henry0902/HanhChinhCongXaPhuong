using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DmNguonDonRp
    {
        private GenericRepository<DmLoaiKNTC> _context = null;
        private DbConnectContext _obj;

        public DmNguonDonRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DmNguonDonSearch> SearchAll(string thongtintimkiem, int trangthai, string order, int pageSize, int pageIndex, int loai)
        {
            var data = _obj.Database.SqlQuery<DmNguonDonSearch>("DmNguonDon_Search @thongtintimkiem, @trangthai,@order, @pageSize, @pageIndex,@loai",
                    new SqlParameter("@thongtintimkiem", thongtintimkiem),
                    new SqlParameter("@trangthai", trangthai),
                    new SqlParameter("@order", order),
                    new SqlParameter("@pageSize", pageSize),
                    new SqlParameter("@pageIndex", pageIndex),
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
                    _obj.Database.ExecuteSqlCommand("DmNguonDon_UpdateStatus @duan_id,@trangthai",
                        new SqlParameter("@duan_id", id),
                        new SqlParameter("@trangthai", status));
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
                    int a = _obj.Database.SqlQuery<int>("DmNguonDon_CheckDelete @id",
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
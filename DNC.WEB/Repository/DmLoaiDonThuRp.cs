using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DmLoaiDonThuRp
    {
        private GenericRepository<DmLoaiDonThu> _context = null;

        private DbConnectContext _obj;

        public DmLoaiDonThuRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DmLoaiDonThuSearch> SearchAll(string thongtintimkiem, int trang_thai, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DmLoaiDonThuSearch>("DmLoaiDonThu_Search @thongtintimkiem,@trang_thai,@Order, @PageSize, @PageIndex",
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
                    _obj.Database.ExecuteSqlCommand("DmLoaiDonThu_UpdateStatus @duan_id,@trang_thai",
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
        public Dropdown1 getAllDepartmentLevel(int id)
        {
            var data = new Dropdown1();
            data.listLoaiDonThu = _obj.Database.SqlQuery<dmDropdown>("dmLoaiDonThu_GetDropDown").ToList();
            data.listLoaiKNTC = _obj.Database.SqlQuery<dmDropdown>("dmLoaiKNTC_GetDropDown").ToList();
            return data;
        }
        public int check_delete(int id)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    int a = _obj.Database.SqlQuery<int>("DmLoaiDonThu_CheckDelete @id",
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
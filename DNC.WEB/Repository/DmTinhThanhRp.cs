using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DmTinhThanhRp
    {
        private GenericRepository<DmTinhThanh> _context = null;

        private DbConnectContext _obj;

        public DmTinhThanhRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DmTinhThanhSearch> SearchAll(string thongtintimkiem, int trang_thai, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DmTinhThanhSearch>("DmTinhThanh_Search @thongtintimkiem,@trang_thai,@Order, @PageSize, @PageIndex",
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
                    _obj.Database.ExecuteSqlCommand("DmTinhThanh_UpdateStatus @duan_id,@trang_thai",
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
                    int a = _obj.Database.SqlQuery<int>("DmTinhThanh_CheckDelete @id",
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
        public Dropdown getAllDepartmentLevel(int? id)
        {
            var data = new Dropdown();
            data.listTinhThanh = _obj.Database.SqlQuery<dmDropdown>("dmTinhThanh_GetDropDown").ToList();
            data.listQuanHuyen = _obj.Database.SqlQuery<dmDropdown>("dmQuanHuyen_GetDropDown").ToList();
            data.listPhuongXa = _obj.Database.SqlQuery<dmDropdown>("dmPhuongXa_GetDropDown").ToList();
            return data;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DmPhuongXaRp
    {
        private GenericRepository<DmPhuongXa> _context = null;
        private DbConnectContext _obj;

        public DmPhuongXaRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DmPhuongXaSearch> SearchAll(string thongtintimkiem, int trang_thai, string order, int pageSize, int pageIndex, int idtinhthanh, int idtinh)
        {
            var data = _obj.Database.SqlQuery<DmPhuongXaSearch>("DmPhuongXa_Search @thongtintimkiem,@trang_thai,@Order, @PageSize, @PageIndex,@idtinhthanh,@idtinh",
                    new SqlParameter("@thongtintimkiem", thongtintimkiem),
                    new SqlParameter("@trang_thai", trang_thai),
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex),
                    new SqlParameter("@idtinhthanh", idtinhthanh),
                    new SqlParameter("@idtinh", idtinh)
                    ).ToList();
            return data;
        }

        public bool UpdateStatus(int id, int status)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("DmPhuongXa_UpdateStatus @duan_id,@trang_thai",
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
        public Dropdown SearchID(int id)
        {
            var data = new Dropdown();
            data.listQuanHuyenById = _obj.Database.SqlQuery<dmDropdown>("listQuanHuyenById_GetDropDown @id",
                new SqlParameter("@id", id)
                ).ToList();
            return data;
        }
        public List<DmPhuongXaSearch> SearchById(int ID)
        {
            var data = _obj.Database.SqlQuery<DmPhuongXaSearch>("DmPhuongXa_GetById @ID",
                    new SqlParameter("@ID", ID)
                    ).ToList();
            return data;
        }
    }
}
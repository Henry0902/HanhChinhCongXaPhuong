using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DmLoaiKNTCCTRp
    {
        private GenericRepository<DmLoaiKNTCCT> _context = null;
        private DbConnectContext _obj;

        public DmLoaiKNTCCTRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DmLoaiKNTCCTSearch> SearchAll(string thongtintimkiem, int trang_thai, string order, int pageSize, int pageIndex, int idtinhthanh, int idtinh)
        {
            var data = _obj.Database.SqlQuery<DmLoaiKNTCCTSearch>("DmLoaiKNTCCT_Search @thongtintimkiem,@trang_thai,@Order, @PageSize, @PageIndex,@idtinhthanh,@idtinh",
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
                    _obj.Database.ExecuteSqlCommand("DmLoaiKNTCCT_UpdateStatus @duan_id,@trang_thai",
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
        public Dropdown1 SearchID(int id)
        {
            var data = new Dropdown1();
            data.listLoaiKNTCById = _obj.Database.SqlQuery<dmDropdown>("listKNTCById_GetDropDown @id",
                new SqlParameter("@id", id)
                ).ToList();
            return data;
        }
        public List<DmLoaiKNTCCTSearch> SearchById(int ID)
        {
            var data = _obj.Database.SqlQuery<DmLoaiKNTCCTSearch>("DmLoaiKNTCCT_GetById @ID",
                    new SqlParameter("@ID", ID)
                    ).ToList();
            return data;
        }
    }
}
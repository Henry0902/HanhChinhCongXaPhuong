using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DmPhienTiepCongDanRp
    {
        private GenericRepository<DmPhienTiepCongDan> _context = null;
        private DbConnectContext _obj;

        public DmPhienTiepCongDanRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DmPhienTiepCongDanSearch> SearchAll(string thongtintimkiem, int trangThai, int idDonVi, int kieuTiep, int loai, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DmPhienTiepCongDanSearch>("DmPhienTiepCongDan_Search @thongtintimkiem, @trangthai, @iddonvi,@kieutiep, @loai, @order, @pageSize, @pageIndex",
                    new SqlParameter("@thongtintimkiem", thongtintimkiem),
                    new SqlParameter("@trangThai", trangThai),
                    new SqlParameter("@iddonvi", idDonVi),
                    new SqlParameter("@kieutiep", kieuTiep),
                    new SqlParameter("@loai", loai),
                    new SqlParameter("@order", order),
                    new SqlParameter("@pageSize", pageSize),
                    new SqlParameter("@pageIndex", pageIndex)
                    ).ToList();
            return data;
        }

        public bool UpdateStatus(int id, int status)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("DmPhienTiepCongDan_UpdateStatus @id, @status",
                        new SqlParameter("@id", id),
                        new SqlParameter("@status", status));

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
                    int a = _obj.Database.SqlQuery<int>("DmPhienTiepCongDan_CheckDelete @id",
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

        public List<DmPhienTiepCongDan> GetPhienTCDByDonViIncludingAll(int IdDonVi)
        {
            var data = _obj.Database.SqlQuery<DmPhienTiepCongDan>("DmPhienTiepCongDan_GetPhienTCDByDonViIncludingAll @IdDonVi", new SqlParameter("@IdDonVi", IdDonVi)).ToList();
            return data;
        }

    }
}

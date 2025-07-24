using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DmDiaDiemTiepDanRp
    {
        private GenericRepository<DmDiaDiemTiepDan> _context = null;
        private DbConnectContext _obj;

        public DmDiaDiemTiepDanRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DmDiaDiemTiepDanSearch> SearchAll(string thongtintimkiem, int iddonvi, string diachi, int trangthai,  string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DmDiaDiemTiepDanSearch>("DmDiaDiemTiepDan_Search @thongtintimkiem,@iddonvi, @diachi, @trangthai, @order, @pageSize, @pageIndex",
                    new SqlParameter("@thongtintimkiem", thongtintimkiem),
                    new SqlParameter("@iddonvi", iddonvi),
                    new SqlParameter("@diachi", diachi),
                    new SqlParameter("@trangthai", trangthai),
                    new SqlParameter("@order", order),
                    new SqlParameter("@pageSize", pageSize),
                    new SqlParameter("@pageIndex", pageIndex)
                    ).ToList();
            return data;
        }


        public bool UpdateStatus(int id, int status, string diachi)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("DmDiaDiemTiepDan_UpdateStatus @duan_id,@trangthai,@diachi",
                        new SqlParameter("@duan_id", id),
                        new SqlParameter("@trangthai", status),
                        new SqlParameter("@diachi", diachi));
                    
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
        //public int check_delete(int id)
        //{
        //    using (var trans = _obj.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            int a = _obj.Database.SqlQuery<int>("DmDiaDiemTiepDan_CheckDelete @id",
        //                new SqlParameter("@id", id)).FirstOrDefault();
        //            return a;
        //        }
        //        catch (Exception)
        //        {
        //            trans.Rollback();
        //            return 0;
        //        }
        //    }
        //}
    }
}
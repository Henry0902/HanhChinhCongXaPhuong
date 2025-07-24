using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Controllers;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class HoSoRp
    {
        private GenericRepository<HoSo> _context = null;
        private DbConnectContext _obj;
        //private Ultils logs = new Ultils();
        public HoSoRp()
        {
            _obj = new DbConnectContext();
        }

        public List<HoSoSearch> SearchAll(string nam, string kho_id, string kegia_id, string hopcap_id, string donvi_id, string loaihs_id, string domat_id, string ttvl_id,
                                    string ngay_baoquan_from, string ngay_baoquan_to, string ngay_luutru_from, string ngay_luutru_to,
                                    string Keyword, string hoso_type, string trang_thai, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<HoSoSearch>("HoSo_Search @nam,@kho_id,@kegia_id,@hopcap_id,@donvi_id,@loaihs_id,@domat_id,@ttvl_id,@ngay_baoquan_from,@ngay_baoquan_to,@ngay_luutru_from,@ngay_luutru_to,@Keyword,@hoso_type,@trang_thai,@Order, @PageSize, @PageIndex",
                    new SqlParameter("@nam", nam),
                    new SqlParameter("@kho_id", kho_id),
                    new SqlParameter("@kegia_id", kegia_id),
                    new SqlParameter("@hopcap_id", hopcap_id),
                    new SqlParameter("@donvi_id", donvi_id),
                    new SqlParameter("@loaihs_id", loaihs_id),
                    new SqlParameter("@domat_id", domat_id),
                    new SqlParameter("@ttvl_id", ttvl_id),
                    new SqlParameter("@ngay_baoquan_from", ngay_baoquan_from),
                    new SqlParameter("@ngay_baoquan_to", ngay_baoquan_to),
                    new SqlParameter("@ngay_luutru_from", ngay_luutru_from),
                    new SqlParameter("@ngay_luutru_to", ngay_luutru_to),
                    new SqlParameter("@Keyword", Keyword),
                    new SqlParameter("@hoso_type", hoso_type),
                    new SqlParameter("@trang_thai", trang_thai),
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex)
                    ).ToList();
            return data;
        }

        public bool updateStatus(int id, bool status)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("HoSo_UpdateStatus @hoso_id,@trang_thai",
                        new SqlParameter("@hoso_id", id),
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
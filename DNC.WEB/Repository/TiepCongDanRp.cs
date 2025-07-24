using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Controllers;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class TiepCongDanRp
    {
        private GenericRepository<TiepCongDan> _context = null;
        private DbConnectContext _obj;
        //private Ultils logs = new Ultils();
        public TiepCongDanRp()
        {
            _obj = new DbConnectContext();
        }

        public List<TiepCongDanSearch> SearchAll(string IdDonVi, string KieuTiepDan, string IdPhienTCD, string IdDoiTuong, string KeyWord, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa,
            string IdLoaiVu, string IdLoaiVuKNTC, string IdLoaiVuKNTCChiTiet, string NoiDung, string TuNgay, string DenNgay, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<TiepCongDanSearch>("TiepCongDan_Search @IdDonVi, @KieuTiepDan, @IdPhienTCD, @IdDoiTuong, @KeyWord, @IdTinhThanh, @IdQuanHuyen, @IdPhuongXa, @IdLoaiVu, @IdLoaiVuKNTC, @IdLoaiVuKNTCChiTiet, @NoiDung, @TuNgay, @DenNgay, @order, @pageSize, @pageIndex",
                    new SqlParameter("@IdDonVi", @IdDonVi),
                    new SqlParameter("@KieuTiepDan", KieuTiepDan),
                    new SqlParameter("@IdPhienTCD", IdPhienTCD),
                    new SqlParameter("@IdDoiTuong", IdDoiTuong),
                    new SqlParameter("@KeyWord", KeyWord),
                    new SqlParameter("@IdTinhThanh", IdTinhThanh),
                    new SqlParameter("@IdQuanHuyen", IdQuanHuyen),
                    new SqlParameter("@IdPhuongXa", IdPhuongXa),
                    new SqlParameter("@IdLoaiVu", IdLoaiVu),
                    new SqlParameter("@IdLoaiVuKNTC", IdLoaiVuKNTC),
                    new SqlParameter("@IdLoaiVuKNTCChiTiet", IdLoaiVuKNTCChiTiet),
                    new SqlParameter("@NoiDung", NoiDung),
                    new SqlParameter("@TuNgay", TuNgay),
                    new SqlParameter("@DenNgay", DenNgay),
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex)
                    ).ToList();
            return data;
        }

        public List<TiepCongDanInfo> GetInfoByID(string Id)
        {
            var data = _obj.Database.SqlQuery<TiepCongDanInfo>("TiepCongDan_GetInfoByID @Id",
                    new SqlParameter("@Id", Id)).ToList();
            return data;
        }

        public List<TiepCongDanInfoView> GetInfoViewByID(string Id)
        {
            var data = _obj.Database.SqlQuery<TiepCongDanInfoView>("TiepCongDan_GetInfoByID @Id",
                    new SqlParameter("@Id", Id)).ToList();
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
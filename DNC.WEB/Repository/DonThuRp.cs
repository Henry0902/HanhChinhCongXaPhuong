using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Controllers;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DonThuRp
    {
        private GenericRepository<DonThu> _context = null;
        private DbConnectContext _obj;
        //private Ultils logs = new Ultils();
        public DonThuRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DonThuSearch> SearchTiepNhan(string IdTrangThai, string IdDonViNhap, string IdNguonDon, string TuNgay, string DenNgay, string Keyword, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DonThuSearch>("DonThu_SearchTiepNhan @IdTrangThai,@IdDonViNhap,@IdNguonDon,@TuNgay,@DenNgay,@Keyword,@Order, @PageSize, @PageIndex",
                    new SqlParameter("@IdTrangThai", IdTrangThai),
                    new SqlParameter("@IdDonViNhap", IdDonViNhap),
                    new SqlParameter("@IdNguonDon", IdNguonDon),
                    new SqlParameter("@TuNgay", TuNgay),
                    new SqlParameter("@DenNgay", DenNgay),
                    new SqlParameter("@Keyword", Keyword),
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex)
                    ).ToList();
            return data;
        }

        public List<DonThuSearch> SearchXuLy(string IdTrangThai, string IdDonViXuLy, string IdNguonDon, string TuNgay, string DenNgay, string Keyword, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DonThuSearch>("DonThu_SearchXuLy @IdTrangThai,@IdDonViXuLy,@IdNguonDon,@TuNgay,@DenNgay,@Keyword,@Order, @PageSize, @PageIndex",
                    new SqlParameter("@IdTrangThai", IdTrangThai),
                    new SqlParameter("@IdDonViXuLy", IdDonViXuLy),
                    new SqlParameter("@IdNguonDon", IdNguonDon),
                    new SqlParameter("@TuNgay", TuNgay),
                    new SqlParameter("@DenNgay", DenNgay),
                    new SqlParameter("@Keyword", Keyword),
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex)
                    ).ToList();
            return data;
        }

        public List<DonThuSearch> SearchAll(string IdDonVi, string IdNguonDon, string IdDoiTuong, string KeyWord, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa, string IdLoaiDonThu, string IdLoaiKNTC, string IdLoaiKNTCChiTiet
            , string IdDonThuXuLy, string IdTrangThai, string IdHuongXuLy, string NoiDung, string TuNgay, string DenNgay, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DonThuSearch>("DonThu_SearchAll @IdDonVi,@IdNguonDon,@IdDoiTuong,@KeyWord,@IdTinhThanh,@IdQuanHuyen,@IdPhuongXa,@IdLoaiDonThu,@IdLoaiKNTC,@IdLoaiKNTCChiTiet,@IdDonThuXuLy,@IdTrangThai,@IdHuongXuLy,@NoiDung,@TuNgay,@DenNgay,@Order, @PageSize, @PageIndex",
                    new SqlParameter("@IdDonVi", IdDonVi),
                    new SqlParameter("@IdNguonDon", IdNguonDon),
                    new SqlParameter("@IdDoiTuong", IdDoiTuong),
                    new SqlParameter("@KeyWord", KeyWord),
                    new SqlParameter("@IdTinhThanh", IdTinhThanh),
                    new SqlParameter("@IdQuanHuyen", IdQuanHuyen),
                    new SqlParameter("@IdPhuongXa", IdPhuongXa),
                    new SqlParameter("@IdLoaiDonThu", IdLoaiDonThu),
                    new SqlParameter("@IdLoaiKNTC", IdLoaiKNTC),
                    new SqlParameter("@IdLoaiKNTCChiTiet", IdLoaiKNTCChiTiet),
                    new SqlParameter("@IdDonThuXuLy", IdDonThuXuLy),
                    new SqlParameter("@IdTrangThai", IdTrangThai),
                    new SqlParameter("@IdHuongXuLy", IdHuongXuLy),
                    new SqlParameter("@NoiDung", NoiDung),
                    new SqlParameter("@TuNgay", TuNgay),
                    new SqlParameter("@DenNgay", DenNgay),
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex)
                    ).ToList();
            return data;
        }

        public List<DonThuSearchHuy> SearchAllHuy(string IdDonVi, string IdNguonDon, string IdDoiTuong, string KeyWord, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa, string IdLoaiDonThu, string IdLoaiKNTC, string IdLoaiKNTCChiTiet
            , string IdDonThuXuLy, string IdTrangThai, string IdHuongXuLy, string NoiDung, string TuNgay, string DenNgay, string IsDelete, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DonThuSearchHuy>("DonThu_SearchAllHuy @IdDonVi,@IdNguonDon,@IdDoiTuong,@KeyWord,@IdTinhThanh,@IdQuanHuyen,@IdPhuongXa,@IdLoaiDonThu,@IdLoaiKNTC,@IdLoaiKNTCChiTiet,@IdDonThuXuLy,@IdTrangThai,@IdHuongXuLy,@NoiDung,@TuNgay,@DenNgay,@IsDelete,@Order, @PageSize, @PageIndex",
                    new SqlParameter("@IdDonVi", IdDonVi),
                    new SqlParameter("@IdNguonDon", IdNguonDon),
                    new SqlParameter("@IdDoiTuong", IdDoiTuong),
                    new SqlParameter("@KeyWord", KeyWord),
                    new SqlParameter("@IdTinhThanh", IdTinhThanh),
                    new SqlParameter("@IdQuanHuyen", IdQuanHuyen),
                    new SqlParameter("@IdPhuongXa", IdPhuongXa),
                    new SqlParameter("@IdLoaiDonThu", IdLoaiDonThu),
                    new SqlParameter("@IdLoaiKNTC", IdLoaiKNTC),
                    new SqlParameter("@IdLoaiKNTCChiTiet", IdLoaiKNTCChiTiet),
                    new SqlParameter("@IdDonThuXuLy", IdDonThuXuLy),
                    new SqlParameter("@IdTrangThai", IdTrangThai),
                    new SqlParameter("@IdHuongXuLy", IdHuongXuLy),
                    new SqlParameter("@NoiDung", NoiDung),
                    new SqlParameter("@TuNgay", TuNgay),
                    new SqlParameter("@DenNgay", DenNgay),
                    new SqlParameter("@IsDelete", IsDelete),
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex)
                    ).ToList();
            return data;
        }

        public List<DonThuSearch> SearchQuaHan(string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DonThuSearch>("DonThu_SearchQuaHan @Order, @PageSize, @PageIndex",                    
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex)
                    ).ToList();
            return data;
        }

        public List<DonThuSearch> SearchCheck(string HoTen, string CMTND, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa, string NoiDung, string order, int pageSize, int pageIndex)
        {
            var data = _obj.Database.SqlQuery<DonThuSearch>("DonThu_SearchCheck @HoTen,@CMTND,@IdTinhThanh,@IdQuanHuyen,@IdPhuongXa,@NoiDung,@Order, @PageSize, @PageIndex",
                    new SqlParameter("@HoTen", HoTen),
                    new SqlParameter("@CMTND", CMTND),
                    new SqlParameter("@IdTinhThanh", IdTinhThanh),
                    new SqlParameter("@IdQuanHuyen", IdQuanHuyen),
                    new SqlParameter("@IdPhuongXa", IdPhuongXa),
                    new SqlParameter("@NoiDung", NoiDung),
                    new SqlParameter("@Order", order),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageIndex", pageIndex)
                    ).ToList();
            return data;
        }

        public List<DonThuInfo> GetInfoByID(string Id)
        {
            var data = _obj.Database.SqlQuery<DonThuInfo>("DonThu_GetInfoByID @Id",
                    new SqlParameter("@Id", Id)).ToList();
            return data;
        }

        public List<DonThuInfoView> GetInfoViewByID(string Id)
        {
            var data = _obj.Database.SqlQuery<DonThuInfoView>("DonThu_GetInfoByID @Id",
                    new SqlParameter("@Id", Id)).ToList();
            return data;
        }

        public List<DonThuInfoPrint> GetInfoPrintByID(string Id)
        {
            var data = _obj.Database.SqlQuery<DonThuInfoPrint>("DonThu_GetInfoPrintByID @Id",
                    new SqlParameter("@Id", Id)).ToList();
            return data;
        }

        public bool UpdateHuongXuLy(int Id, int IdHuongXuLy, int IdDonViXuLy, int IdDonViXacMinh, int IdDonViTiepNhan, int IdCanBoXuLy, int IdNguoiTao ,string NgayThoiHanThuLy, int IdTrangThai)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("DonThu_UpdateHuongXuLy @Id,@IdHuongXuLy,@IdDonViXuLy,@IdDonViXacMinh,@IdDonViTiepNhan,@IdCanBoXuLy,@IdNguoiTao,@NgayThoiHanThuLy,@IdTrangThai",
                        new SqlParameter("@Id", Id),
                        new SqlParameter("@IdHuongXuLy", IdHuongXuLy),
                        new SqlParameter("@IdDonViXuLy", IdDonViXuLy),
                        new SqlParameter("@IdDonViXacMinh", IdDonViXacMinh),
                        new SqlParameter("@IdDonViTiepNhan", IdDonViTiepNhan),
                        new SqlParameter("@IdCanBoXuLy", IdCanBoXuLy),
                        new SqlParameter("@IdNguoiTao", IdNguoiTao),
                        new SqlParameter("@NgayThoiHanThuLy", NgayThoiHanThuLy),
                        new SqlParameter("@IdTrangThai", IdTrangThai));
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

        public bool UpdateHuongPheDuyetXuLy(int Id, int IdHuongXuLy, int IdDonViXuLy, int IdDonViXacMinh, int IdDonViTiepNhan, string NgayThoiHanThuLy, int IdTrangThai)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("DonThu_UpdateHuongPheDuyetXuLy @Id,@IdHuongXuLy,@IdDonViXuLy,@IdDonViXacMinh,@IdDonViTiepNhan,@NgayThoiHanThuLy,@IdTrangThai",
                        new SqlParameter("@Id", Id),
                        new SqlParameter("@IdHuongXuLy", IdHuongXuLy),
                        new SqlParameter("@IdDonViXuLy", IdDonViXuLy),
                        new SqlParameter("@IdDonViXacMinh", IdDonViXacMinh),
                        new SqlParameter("@IdDonViTiepNhan", IdDonViTiepNhan),
                        new SqlParameter("@NgayThoiHanThuLy", NgayThoiHanThuLy),
                        new SqlParameter("@IdTrangThai", IdTrangThai));
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

        public bool UpdateTrangThai(int Id, int IdTrangThai)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("DonThu_UpdateTrangThai @Id,@IdTrangThai",
                        new SqlParameter("@Id", Id),
                        new SqlParameter("@IdTrangThai", IdTrangThai));
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

        public bool UpdateIsDelete(int Id, int IsDelete)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("DonThu_UpdateIsDelete @Id,@IsDelete",
                        new SqlParameter("@Id", Id),
                        new SqlParameter("@IsDelete", IsDelete));
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

        public bool UpdateDonThuGoc(int Id)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("DonThu_UpdateDonThuGoc @Id",
                        new SqlParameter("@Id", Id));
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
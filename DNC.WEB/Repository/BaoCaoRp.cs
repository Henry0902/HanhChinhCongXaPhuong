using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Controllers;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class BaoCaoRp
    {
        private DbConnectContext _obj;
        public BaoCaoRp()
        {
            _obj = new DbConnectContext();
        }

        public List<BaoCao_TiepCongDan_DonThu_ByYear> BaoCao_TiepCongDan_DonThu_ByYear(string Nam)
        {
            var data = _obj.Database.SqlQuery<BaoCao_TiepCongDan_DonThu_ByYear>("BaoCao_TiepCongDan_DonThu_ByYear @Nam", new SqlParameter("@Nam", Nam)).ToList();
            return data;
        }

        public List<BaoCao_DonThuTheoLoai> BaoCao_DonThuTheoLoai(string Nam)
        {
            var data = _obj.Database.SqlQuery<BaoCao_DonThuTheoLoai>("BaoCao_DonThuTheoLoai @Nam", new SqlParameter("@Nam", Nam)).ToList();
            return data;
        }

        public List<BaoCao_SoTiepCongDan> BaoCao_SoTiepCongDan(string IdDonVi, string KieuTiepDan, string IdPhienTCD, string IdDoiTuong, string KeyWord, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa,
                                                string IdLoaiVu, string IdLoaiVuKNTC, string IdLoaiVuKNTCChiTiet, string NoiDung, string TuNgay, string DenNgay, string order)
        {
            var data = _obj.Database.SqlQuery<BaoCao_SoTiepCongDan>("BaoCao_SoTiepCongDan @IdDonVi, @KieuTiepDan, @IdPhienTCD, @IdDoiTuong, @KeyWord, @IdTinhThanh, @IdQuanHuyen, @IdPhuongXa, @IdLoaiVu, @IdLoaiVuKNTC, @IdLoaiVuKNTCChiTiet, @NoiDung, @TuNgay, @DenNgay, @order",
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
                    new SqlParameter("@Order", order)).ToList();
            return data;
        }

        public List<BaoCao_SoTiepNhanDonThu> BaoCao_SoTiepNhanDonThu(string IdDonVi, string IdNguonDon, string IdDoiTuong, string KeyWord, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa, string IdLoaiDonThu, string IdLoaiKNTC,
                                                string IdLoaiKNTCChiTiet, string IdDonThuXuLy, string IdTrangThai, string IdHuongXuLy, string NoiDung, string TuNgay, string DenNgay, string order)
        {
            var data = _obj.Database.SqlQuery<BaoCao_SoTiepNhanDonThu>("BaoCao_SoTiepNhanDonThu @IdDonVi,@IdNguonDon,@IdDoiTuong,@KeyWord,@IdTinhThanh,@IdQuanHuyen,@IdPhuongXa,@IdLoaiDonThu,@IdLoaiKNTC,@IdLoaiKNTCChiTiet,@IdDonThuXuLy,@IdTrangThai,@IdHuongXuLy,@NoiDung,@TuNgay,@DenNgay,@Order",
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
                    new SqlParameter("@Order", order)).ToList();
            return data;
        }

        public List<BaoCao_ThongKeDonThuTiepNhan> BaoCao_ThongKeDonThuTiepNhan(string Nam)
        {
            var data = _obj.Database.SqlQuery<BaoCao_ThongKeDonThuTiepNhan>("BaoCao_ThongKeDonThuTiepNhan @Nam", new SqlParameter("@Nam", Nam)).ToList();
            return data;
        }

        public List<BaoCao_ThongKeDonThuGiaiQuyet> BaoCao_ThongKeDonThuGiaiQuyet(string Nam)
        {
            var data = _obj.Database.SqlQuery<BaoCao_ThongKeDonThuGiaiQuyet>("BaoCao_ThongKeDonThuGiaiQuyet @Nam", new SqlParameter("@Nam", Nam)).ToList();
            return data;
        }

        public List<BaoCao_ThongKeKetQuaTiepDan> BaoCao_ThongKeKetQuaTiepDan(string Nam)
        {
            var data = _obj.Database.SqlQuery<BaoCao_ThongKeKetQuaTiepDan>("BaoCao_ThongKeKetQuaTiepDan @Nam", new SqlParameter("@Nam", Nam)).ToList();
            return data;
        }

        public List<BaoCao_TongHop_TiepCongDan> BaoCao_TongHop_TiepCongDan(string IdDonVi, string TuNgay, string DenNgay)
        {
            var data = _obj.Database.SqlQuery<BaoCao_TongHop_TiepCongDan>("BaoCao_TongHop_TiepCongDan @IdDonVi, @TuNgay, @DenNgay", 
                new SqlParameter("@IdDonVi", IdDonVi),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay)).ToList();
            return data;
        }

        public List<BaoCao_TongHop_DonThu> BaoCao_TongHop_DonThu(string IdDonVi, string TuNgay, string DenNgay)
        {
            var data = _obj.Database.SqlQuery<BaoCao_TongHop_DonThu>("BaoCao_TongHop_DonThu @IdDonVi, @TuNgay, @DenNgay",
                new SqlParameter("@IdDonVi", IdDonVi),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay)).ToList();
            return data;
        }

        public List<BaoCao_DonThuTheoDiaBan> BaoCao_DonThuTheoDiaBan(string IdTinhThanh, string TuNgay, string DenNgay)
        {
            var data = _obj.Database.SqlQuery<BaoCao_DonThuTheoDiaBan>("BaoCao_DonThuTheoDiaBan @IdTinhThanh, @TuNgay, @DenNgay",
                new SqlParameter("@IdTinhThanh", IdTinhThanh),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay)).ToList();
            return data;
        }

        public List<BaoCao_TongHopKetQuaXuLyDonThu> BaoCao_TongHopKetQuaXuLyDonThu(string IdDonVi, string TuNgay, string DenNgay)
        {
            var data = _obj.Database.SqlQuery<BaoCao_TongHopKetQuaXuLyDonThu>("BaoCao_TongHopKetQuaXuLyDonThu @IdDonVi, @TuNgay, @DenNgay",
                new SqlParameter("@IdDonVi", IdDonVi),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay)).ToList();
            return data;
        }

        public List<BaoCao_TongHopKetQuaXuLyDonThu01> BaoCao_TongHopKetQuaXuLyDonThu01(string IdDonVi, string TuNgay, string DenNgay)
        {
            var data = _obj.Database.SqlQuery<BaoCao_TongHopKetQuaXuLyDonThu01>("BaoCao_TongHopKetQuaXuLyDonThu01 @IdDonVi, @TuNgay, @DenNgay",
                new SqlParameter("@IdDonVi", IdDonVi),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay)).ToList();
            return data;
        }

        public List<BaoCao_TongHopKetQuaXuLyDonThuKhieuNai> BaoCao_TongHopKetQuaXuLyDonThuKhieuNai(string IdDonVi, string TuNgay, string DenNgay)
        {
            var data = _obj.Database.SqlQuery<BaoCao_TongHopKetQuaXuLyDonThuKhieuNai>("BaoCao_TongHopKetQuaXuLyDonThuKhieuNai @IdDonVi, @TuNgay, @DenNgay",
                new SqlParameter("@IdDonVi", IdDonVi),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay)).ToList();
            return data;
        }

        public List<BaoCao_TongHopKetQuaXuLyDonThuToCao> BaoCao_TongHopKetQuaXuLyDonThuToCao(string IdDonVi, string TuNgay, string DenNgay)
        {
            var data = _obj.Database.SqlQuery<BaoCao_TongHopKetQuaXuLyDonThuToCao>("BaoCao_TongHopKetQuaXuLyDonThuToCao @IdDonVi, @TuNgay, @DenNgay",
                new SqlParameter("@IdDonVi", IdDonVi),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay)).ToList();
            return data;
        }

        public List<BaoCao_TongHopKetQuaXuLyDonThuKienNghiPhanAnh> BaoCao_TongHopKetQuaXuLyDonThuKienNghiPhanAnh(string IdDonVi, string TuNgay, string DenNgay)
        {
            var data = _obj.Database.SqlQuery<BaoCao_TongHopKetQuaXuLyDonThuKienNghiPhanAnh>("BaoCao_TongHopKetQuaXuLyDonThuKienNghiPhanAnh @IdDonVi, @TuNgay, @DenNgay",
                new SqlParameter("@IdDonVi", IdDonVi),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay)).ToList();
            return data;
        }

        public List<BaoCao_TongHopKetQuaTiepCongDan> BaoCao_TongHopKetQuaTiepCongDan(string IdDonVi, string TuNgay, string DenNgay)
        {
            var data = _obj.Database.SqlQuery<BaoCao_TongHopKetQuaTiepCongDan>("BaoCao_TongHopKetQuaTiepCongDan @IdDonVi, @TuNgay, @DenNgay",
                new SqlParameter("@IdDonVi", IdDonVi),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay)).ToList();
            return data;
        }

        public List<BaoCao_TongHopKetQuaTiepCongDan01> BaoCao_TongHopKetQuaTiepCongDan01(string IdDonVi, string TuNgay, string DenNgay)
        {
            var data = _obj.Database.SqlQuery<BaoCao_TongHopKetQuaTiepCongDan01>("BaoCao_TongHopKetQuaTiepCongDan01 @IdDonVi, @TuNgay, @DenNgay",
                new SqlParameter("@IdDonVi", IdDonVi),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay)).ToList();
            return data;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNC.WEB.Models
{
    public partial class BaoCao_TiepCongDan_DonThu_ByYear
    {
        public int Thang { get; set; }
        public int TiepCongDan { get; set; }
        public int DonThu { get; set; }
    }

    public partial class BaoCao_DonThuTheoLoai
    {
        public string label { get; set; }
        public int value { get; set; }
    }

    public partial class BaoCao_SoTiepCongDan
    {
        public string NgayTiep { get; set; }
        public string ThongTin { get; set; }
        public string NoiDungTiepDan { get; set; }
        public string PhanloaiDon { get; set; }
        public string CQDaGiaiQuyet { get; set; }
        public string ThuLyGiaiQUyet { get; set; }
        public string HuongDan { get; set; }
        public string ChuyenDon { get; set; }
        public string VBDonDoc { get; set; }
        public string TenTrangThai { get; set; }     
    }

    public partial class BaoCao_SoTiepNhanDonThu
    {
        public string NgayNhap { get; set; }
        public string TenNguonDon { get; set; }
        public string ThongTin { get; set; }
        public string NoiDungDonThu { get; set; }
        public string PhanloaiDon { get; set; }
        public string CQDaGiaiQuyet { get; set; }
        public string ThuLyGiaiQUyet { get; set; }
        public string HuongDan { get; set; }
        public string ChuyenDon { get; set; }
        public string VBDonDoc { get; set; }
        public string LuuTheoDoi { get; set; }
        public string TenTrangThai { get; set; }
    }

    public partial class BaoCao_ThongKeDonThuTiepNhan
    {
        public string Name { get; set; }
        public int sl { get; set; }
    }

    public partial class BaoCao_ThongKeDonThuGiaiQuyet
    {
        public string Name { get; set; }
        public int sl { get; set; }
    }

    public partial class BaoCao_ThongKeKetQuaTiepDan
    {
        public string Name { get; set; }
        public int SLTiepCongDan { get; set; }
        public int SLDonThu { get; set; }
    }

    public partial class BaoCao_TongHop_TiepCongDan
    {
        public string TenDonVi { get; set; }
        public int TTiepDan { get; set; }
        public int TSoNguoi { get; set; }
        public int TSoVu { get; set; }

        public int TTTinhUyTD { get; set; }
        public int TTTinhUySN { get; set; }
        public int TTHdndTD { get; set; }
        public int TTHdndSN { get; set; }
        public int TTBantcdTD { get; set; }
        public int TTBantcdSN { get; set; }
        public int HdndTD { get; set; }
        public int HdndSN { get; set; }
        public int DbqdTD { get; set; }
        public int DbqdSN { get; set; }
        public int CTUbndTD { get; set; }
        public int CTUbndSN { get; set; }
        public int BTTinhUyTD { get; set; }
        public int BTTinhUySN { get; set; }

        public int KhieuNaiHanhChinh { get; set; }
        public int KhieuNaiTuPhap { get; set; }
        public int KhieuNaiChinhSach { get; set; }
        public int KhieuNaiDatDai { get; set; }
        public int ToCaoHanhChinh { get; set; }
        public int ToCaoTuPhap { get; set; }
        public int ToCaoThamNhung { get; set; }
        public int ToCaoDatDai { get; set; }
        public int TranhChap { get; set; }
        public int PhanAnhChinhSach { get; set; }
        public int PhanAnhTuPhap { get; set; }
        public int PhanAnhDatDai { get; set; }
        public int PhanAnhKhac { get; set; }

        public int CaNhan { get; set; }
        public int DoanDongNguoi { get; set; }
        public int CoQuanToChuc { get; set; }

        public int ChuaGiaiQuyet { get; set; }
        public int GiaiQuyetLanDau { get; set; }
        public int GiaiQuyetNhieuLan { get; set; }
    }

    public partial class BaoCao_TongHop_DonThu
    {
        public string TenDonVi { get; set; }
        public int TNTrongKy { get; set; }       
        public int KhieuNaiHanhChinh { get; set; }
        public int KhieuNaiTuPhap { get; set; }
        public int KhieuNaiChinhSach { get; set; }
        public int KhieuNaiDatDai { get; set; }
        public int ToCaoHanhChinh { get; set; }
        public int ToCaoTuPhap { get; set; }
        public int ToCaoThamNhung { get; set; }
        public int ToCaoDatDai { get; set; }
        public int TranhChap { get; set; }
        public int PhanAnhChinhSach { get; set; }
        public int PhanAnhDatDai { get; set; }
        public int PhanAnhTuPhap { get; set; }
        public int PhanAnhKhac { get; set; }

        public int ChuaGiaiQuyet { get; set; }
        public int GiaiQuyetLanDau { get; set; }
        public int GiaiQuyetNhieuLan { get; set; }

        public int ThuLyGiaiQuyet { get; set; }
        public int HuongDan { get; set; }
        public int ChuyenDon { get; set; }
        public int DonDoc { get; set; }
        public int DangXuLy { get; set; }
        public int KhongXuLy { get; set; }
    }

    public partial class BaoCao_DonThuTheoDiaBan
    {
        public string TenQuanHuyen { get; set; }
        public int TongDT { get; set; }
        public int TongVu { get; set; }
        public int DTDuDieuKien { get; set; }
        public int DTKhongDuDieuKien { get; set; }
    }

    public partial class BaoCao_TongHopKetQuaXuLyDonThu
    {
        public string TenDonVi { get; set; }
        public int TNTrongKy { get; set; }
        public int TNTrongKyDoanDongNguoi { get; set; }
        public int TNTrongKyCaNhan { get; set; }
        public int TNTrongKyKhac { get; set; }
        public int DonDaXuLy { get; set; }
        public int DonDuDieuKien { get; set; }
        public int VuDuDieuKien { get; set; }
        public int DonKhieuNai { get; set; }
        public int DonToCao { get; set; }
        public int DonPhanAnh { get; set; }
        public int DonTranhChap { get; set; }
        public int GiaiQuyetLanDau { get; set; }
        public int GiaiQuyetNhieuLan { get; set; }
        public int ChuaGiaiQuyet { get; set; }
        public int DonThuocThamQuyen { get; set; }
        public int DonThuocThamQuyenKhieuNai { get; set; }
        public int DonThuocThamQuyenToCao { get; set; }
        public int DonThuocThamQuyenPhanAnh { get; set; }
        public int DonThuocThamQuyenTranhChap { get; set; }        
        public int DonKhongThuocThamQuyen { get; set; }
        public int DonHuongDan { get; set; }
        public int DonChuyenDon { get; set; }
        public int DonDonDoc { get; set; }
        public int DonChuyenPhucDap { get; set; }
    }


    public partial class BaoCao_TongHopKetQuaTiepCongDan
    {
        public string TenDonVi { get; set; }
        public int TSLuot { get; set; }
        public int TSNguoi { get; set; }
        public int TSVuViec { get; set; }
        public int TTXSoLuot { get; set; }
        public int TTXSoNguoi { get; set; }
        public int TTXVuViecLanDau { get; set; }
        public int TTXVuViecNhieuLan { get; set; }
        public int TTXSoDoan { get; set; }
        public int TTXSoNguoiTrongDoan { get; set; }
        public int TTXSoDoanLanDau { get; set; }
        public int TTXSoDoanNhieuLan { get; set; }

        public int TDKSoky { get; set; }
        public int TDKSoLuot { get; set; }
        public int TDKSoNguoi { get; set; }
        public int TDKVuViecLanDau { get; set; }
        public int TDKVuViecNhieuLan { get; set; }
        public int TDKSoDoan { get; set; }
        public int TDKSoNguoiTrongDoan { get; set; }
        public int TDKSoDoanLanDau { get; set; }
        public int TDKSoDoanNhieuLan { get; set; }

        public int TDK_UQSoKy { get; set; }
        public int TDK_UQSoLuot { get; set; }
        public int TDK_UQSoNguoi { get; set; }
        public int TDK_UQVuViecLanDau { get; set; }
        public int TDK_UQVuViecNhieuLan { get; set; }
        public int TDK_UQSoDoan { get; set; }
        public int TDK_UQSoNguoiTrongDoan { get; set; }
        public int TDK_UQSoDoanLanDau { get; set; }
        public int TDK_UQSoDoanNhieuLan { get; set; }

    }

    public partial class BaoCao_TongHopKetQuaTiepCongDan01
    {
        public string TenDonVi { get; set; }
        public int TSLuot { get; set; }
        public int TSNguoi { get; set; }
        public int TSVuViec { get; set; }
        public int TTXSoLuot { get; set; }
        public int TTXSoNguoi { get; set; }
        public int TTXVuViecLanDau { get; set; }
        public int TTXVuViecNhieuLan { get; set; }
        public int TTXSoDoan { get; set; }
        public int TTXSoNguoiTrongDoan { get; set; }
        public int TTXSoDoanLanDau { get; set; }
        public int TTXSoDoanNhieuLan { get; set; }

        public int TDKSoky { get; set; }
        public int TDKSoLuot { get; set; }
        public int TDKSoNguoi { get; set; }
        public int TDKVuViecLanDau { get; set; }
        public int TDKVuViecNhieuLan { get; set; }
        public int TDKSoDoan { get; set; }
        public int TDKSoNguoiTrongDoan { get; set; }
        public int TDKSoDoanLanDau { get; set; }
        public int TDKSoDoanNhieuLan { get; set; }

        public int TDK_UQSoKy { get; set; }
        public int TDK_UQSoLuot { get; set; }
        public int TDK_UQSoNguoi { get; set; }
        public int TDK_UQVuViecLanDau { get; set; }
        public int TDK_UQVuViecNhieuLan { get; set; }
        public int TDK_UQSoDoan { get; set; }
        public int TDK_UQSoNguoiTrongDoan { get; set; }
        public int TDK_UQSoDoanLanDau { get; set; }
        public int TDK_UQSoDoanNhieuLan { get; set; }

    }

    public partial class BaoCao_TongHopKetQuaXuLyDonThu01
    {
        public string TenDonVi { get; set; }
        public int TNTrongKy { get; set; }
        public int TNTruocKy { get; set; }
        public int DonDaXuLy { get; set; }
        public int DonChuaXyLy { get; set; }
        public int VuDuDieuKien { get; set; }
        public int DonKhieuNai { get; set; }
        public int DonToCao { get; set; }
        public int DonPhanAnh { get; set; }
        public int DonTranhChap { get; set; }
        public int GiaiQuyetLanDau { get; set; }
        public int GiaiQuyetNhieuLan { get; set; }
        public int ChuaGiaiQuyet { get; set; }
        public int DonThuocThamQuyen { get; set; }
        public int DonThuocThamQuyenKhieuNai { get; set; }
        public int DonThuocThamQuyenToCao { get; set; }
        public int DonThuocThamQuyenPhanAnh { get; set; }
        public int DonThuocThamQuyenTranhChap { get; set; }
        public int DonHuongDan { get; set; }
        public int DonChuyenDon { get; set; }
        public int DonDonDoc { get; set; }
        public int DonChuyenPhucDap { get; set; }


    }

    public partial class BaoCao_TongHopKetQuaXuLyDonThuKhieuNai
    {
        public string TenDonVi { get; set; }
        public int TNTrongKy { get; set; }
        public int TNTruocKy { get; set; }
        public int DonDaXuLyKyTruoc { get; set; }
        public int DonDaXuLyTrongKy { get; set; }
        public int DonDuDieuKien { get; set; }
        public int VuDuDieuKien { get; set; }
        public int HanhChinhChinhSach { get; set; }
        public int HanhChinhDatDai { get; set; }
        public int HanhChinhKhac { get; set; }
        public int LinhVucTuPhap { get; set; }
        public int LinhVucDangDoanThe { get; set; }
        public int LinhVucKhac { get; set; }
        public int GiaiQuyetLanDau { get; set; }
        public int GiaiQuyetLan2 { get; set; }
        public int BanAnTAND { get; set; }
        public int ChuaGiaiQuyet { get; set; }
        public int KetQuaXuLyLanDau { get; set; }
        public int KetQuaXuLyLan2 { get; set; }
        public int DonHuongDan { get; set; }
        public int DonDonDoc { get; set; }
        public int DonChuyenPhucDap { get; set; }
    }

    public partial class BaoCao_TongHopKetQuaXuLyDonThuToCao
    {
        public string TenDonVi { get; set; }
        public int TNTrongKy { get; set; }
        public int TNTruocKy { get; set; }
        public int DonDaXuLyKyTruoc { get; set; }
        public int DonDaXuLyTrongKy { get; set; }
        public int DonDuDieuKien { get; set; }
        public int HanhChinhChinhSach { get; set; }
        public int HanhChinhDatDai { get; set; }
        public int HanhChinhCongChucCongVu { get; set; }
        public int HanhChinhKhac { get; set; }
        public int ThamNhung { get; set; }
        public int LinhVucTuPhap { get; set; }
        public int LinhVucDangDoanThe { get; set; }
        public int LinhVucKhac { get; set; }
        public int QuaThoiHanChuaGiaiQuyet { get; set; }
        public int DaCoKetLuanGiaiQuyet { get; set; }
        public int ChuaGiaiQuyet { get; set; }
        public int ToCaoLanDau { get; set; }
        public int ToCaoTiep { get; set; }
        public int DonChuyenDon { get; set; }
        public int DonDonDoc { get; set; }
        public int DonChuyenPhucDap { get; set; }
    }

    public partial class BaoCao_TongHopKetQuaXuLyDonThuKienNghiPhanAnh
    {
        public string TenDonVi { get; set; }
        public int TNTrongKy { get; set; }
        public int TNTruocKy { get; set; }
        public int DonDaXuLyKyTruoc { get; set; }
        public int DonDaXuLyTrongKy { get; set; }
        public int DonDuDieuKien { get; set; }

        public int CheDoChinhSach { get; set; }
        public int DatDai { get; set; }
        public int Khac { get; set; }
        public int TuPhap { get; set; }
        public int DaDuocGiaiQuyet { get; set; }
        public int ChuaDuocGiaiQuyet { get; set; }
        public int VuViecThuocThamQuyen { get; set; }
        public int DonChuyenDon { get; set; }
        public int DonDonDoc { get; set; }
        public int DonChuyenPhucDap { get; set; }
        public int VuViecDaGiaiQuyet { get; set; }
        public int VuViecChuaGiaiQuyet { get; set; }
    }
}

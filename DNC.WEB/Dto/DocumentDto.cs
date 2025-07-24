using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNC.WEB.Dto
{
    public class DocumentDto
    {
        public string LinhVuc { get; set; }
        public string MaLinhVuc { get; set; }
        public string LoaiVanBan { get; set; }
        public string Guid { get; set; }
        public string ScheduleId { get; set; }
        public string AssignId { get; set; }
        public string DoKhan { get; set; }
        public string TrichYeu { get; set; }
        public string SoHieu { get; set; }
        public string KyHieu { get; set; }
        public string MaHSCV { get; set; }
        public string NguoiKy { get; set; }
        public string NgayBanHanh { get; set; }
        public string TrangThaiXem { get; set; }
        public string NgayNhan { get; set; }
        public string Nam { get; set; }
        public string NoiNhan { get; set; }
        public string CoQuanBanHanh { get; set; }
        public string Id { get; set; }
        public string ButPhe { get; set; }
        public string QtxlNgoaiDV { get; set; }
        public string QtxlTrongDV { get; set; }
        public string ChucVuNguoiKy { get; set; }
        public string DoMat { get; set; }
        public string NoiDung { get; set; }
        public string LoaiVBHanhChinh { get; set; }
        public int TrangThai { get; set; }
        public string Message { get; set; }
        public string MaDinhDanh { get; set; }
        public string MaLoai { get; set; }
        public string MaCapDo { get; set; }
        public string TenCapDo { get; set; }
        public string NguoiTao { get; set; }
        public string CongVan { get; set; }
        public string NgayLuu { get; set; }
        public string NhanVienLuu { get; set; }
        public string XuatXu { get; set; }
        public string Duyet { get; set; }
        public string NgayDuyet { get; set; }
        public string NgayDen { get; set; }
        public string NguoiDuyet { get; set; }
        public string GhiChu { get; set; }
        public string TrangThaiChuyenVien { get; set; }
        public string MaChuyenVien { get; set; }
        public string BenNgoai { get; set; }
        public string HanXuLy { get; set; }
        public string NoiLuu { get; set; }
        public string SoSaoY { get; set; }
        public string LienKet { get; set; }
        public string TenCongVan { get; set; }
        public string TenLienKet { get; set; }
        public string LanhDaoChuyen { get; set; }
        public string TrangThaiVB { get; set; }
        public string SoDen { get; set; }
        public string SoDenMoRong { get; set; }
        public string ButPheLD { get; set; }
        public string HT_Nguoi_Duyet { get; set; }
        public string NguonVanBan { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<object> ListRelatedDocument { get; set; }
        public string IdLienThong { get; set; }
        public List<object> DonViNhanVanBanNgoaDonVi { get; set; }
    }

    public class Attachment
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        //public string FileContent { get; set; }
        public string FilePath { get; set; }
        public int AttachmentStatus { get; set; }
    }

    public class DocumentValue
    {
        public int Total { get; set; }
        public List<DocumentDto> Data { get; set; }
    }

    public class DocumentResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public DocumentValue Value { get; set; }
    }
}

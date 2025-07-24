namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DmPhienTiepCongDan")]
    public partial class DmPhienTiepCongDan
    {
        [Key]
        public int Id { get; set; }
        public string TenPhienTCD { get; set; }
        public int TrangThai { get; set; }
        public int IdDonVi { get; set; }
        public string MoTa { get; set; }
        public int SoThuTu { get; set; }// số thứ tự hiển thị trong danh mục
        public int KieuTiep { get; set; } // kiểu tiếp của phiên tiếp dân : tiếp thường xuyên hay thủ trưởng tiếp
        public int Loai { get; set; }// Loại : bình thường hay đặc biệt
        public int MaNguoiTao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int MaNguoiThayDoi { get; set; }// mã người thay đổi gần nhất
        public DateTime? ThoiDiemCapNhat { get; set; }// Thời điểm cập nhật gần nhất
    }

    public partial class DmPhienTiepCongDanSearch
    {
        [Key]
        public int id { get; set; }
        public string TenPhienTCD { get; set; }
        public int TrangThai { get; set; }
        public int IdDonVi { get; set; }
        public string MoTa { get; set; }
        public int SoThuTu { get; set; }
        public int KieuTiep { get; set; }
        public int Loai { get; set; }
        public int MaNguoiTao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int MaNguoiThayDoi { get; set; }
        public DateTime? ThoiDiemCapNhat { get; set; }
        public int? TotalRecords { get; set; }
    }

    public class DmPhienTiepCongDanDDL
    {
        public int Id { get; set; }
        public string TenPhienTCD { get; set; }

        public int IdDonVi { get; set; }

    }
}
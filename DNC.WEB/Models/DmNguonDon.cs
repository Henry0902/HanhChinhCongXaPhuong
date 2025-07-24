namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DmNguonDon")]
    public partial class DmNguonDon
    {
        [Key]
        public int Id { get; set; }
        public string TenNguonDon { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public string MoTa { get; set; }
        public int SoThuTu { get; set; } // số thứ tự hiển thị trong danh mục
        public int Loai { get; set; } // Loại : bình thường hay đặc biệt
        public int MaNguoiTao { get; set; }
        public int MaNguoiThayDoi { get; set; } // mã người thay đổi gần nhất
        public DateTime? ThoiDiemCapNhat { get; set; } // Thời điểm cập nhật gần nhất

    }

    public partial class DmNguonDonSearch
    {
        [Key]
        public int id { get; set; }
        public string TenNguonDon { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public string MoTa { get; set; }
        public int SoThuTu { get; set; } // số thứ tự hiển thị trong danh mục
        public int Loai { get; set; } // Loại : bình thường hay đặc biệt
        public int MaNguoiTao { get; set; }
        public int MaNguoiThayDoi { get; set; } // mã người thay đổi gần nhất
        public DateTime? ThoiDiemCapNhat { get; set; } // Thời điểm cập nhật gần nhất
        public int? TotalRecords { get; set; }
    }
}
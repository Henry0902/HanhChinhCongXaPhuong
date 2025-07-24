namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KetLuanGiaiQuyet")]
    public partial class KetLuanGiaiQuyet
    {
        [Key]
        public int Id { get; set; }
        public int IdDonThu { get; set; }
        public DateTime? NgayBanHanh { get; set; }
        public int IdLoaiKetQua { get; set; }
        public string NoiDung { get; set; }
        public double? NNSoTien { get; set; }
        public double? NNSoDat { get; set; }
        public double? NNSoDatSX { get; set; }
        public double? CNSoTien { get; set; }
        public double? CNSoDat { get; set; }
        public double? CNSoDatSX { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? IdDonThuGoc { get; set; }
    }

    public partial class KetLuanGiaiQuyetInfoView
    {
        [Key]
        public int Id { get; set; }
        public int IdDonThu { get; set; }
        public DateTime? NgayBanHanh { get; set; }
        public int IdLoaiKetQua { get; set; }
        public string TenLoaiKetQua { get; set; }
        public string NoiDung { get; set; }
        public double? NNSoTien { get; set; }
        public double? NNSoDat { get; set; }
        public double? NNSoDatSX { get; set; }
        public double? CNSoTien { get; set; }
        public double? CNSoDat { get; set; }
        public double? CNSoDatSX { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? IdDonThuGoc { get; set; }
        public virtual List<FileUpload> FileUpload { get; set; }
    }
}
namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dmLoaiKNTCChiTiet")]
    public partial class DmLoaiKNTCCT
    {
        [Key]
        public int id { get; set; }
        public int IdLoaiKNTC { get; set; }
        public string TenLoaiKNTCChiTiet { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
    }

    public partial class DmLoaiKNTCCTSearch
    {
        [Key]
        public int id { get; set; }
        public int IdLoaiKNTC { get; set; }
        public string TenLoaiKNTCChiTiet { get; set; }
        public int idTinh2 { get; set; }
        public string TenLoaiKNTC { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? TotalRecords { get; set; }
    }
}
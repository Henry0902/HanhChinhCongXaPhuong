namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dmQuanHuyen")]
    public partial class DmQuanHuyen
    {
        [Key]
        public int id { get; set; }
        public int idTinhThanh { get; set; }
        public string TenQuanHuyen { get; set; }
        public string MaQuanHuyen { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
    }
    public partial class DmQuanHuyenSearch
    {
        [Key]
        public int id { get; set; }
        public int idTinhThanh { get; set; }
        public string TenTinhThanh { get; set; }
        public string TenQuanHuyen { get; set; }
        public string MaQuanHuyen { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? TotalRecords { get; set; }
    }
}
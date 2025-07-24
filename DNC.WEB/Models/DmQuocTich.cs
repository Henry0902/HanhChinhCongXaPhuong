namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dmQuocTich")]
    public partial class DmQuocTich
    {
        [Key]
        public int id { get; set; }
        public string TenQuocTich { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
    }

    public partial class DmQuocTichSearch
    {
        [Key]
        public int id { get; set; }
        public string TenQuocTich { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? TotalRecords { get; set; }
    }
}
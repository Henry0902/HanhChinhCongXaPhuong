namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dmChucVu")]
    public partial class DmChucVu
    {
        [Key]
        public int id { get; set; }
        public string TenChucVu { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
    }

    public partial class DmChucVuSearch
    {
        [Key]
        public int id { get; set; }
        public string TenChucVu { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? TotalRecords { get; set; }
    }
}
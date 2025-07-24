namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dmPhuongXa")]
    public partial class DmPhuongXa
    {
        [Key]
        public int id { get; set; }
        public int idQuanHuyen { get; set; }
        public string TenPhuongXa { get; set; }
        public string MaPhuongXa { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
    }

    public partial class DmPhuongXaSearch
    {
        [Key]
        public int id { get; set; }
        public int idQuanHuyen { get; set; }
        public int idTinh2 { get; set; }
        public string TenPhuongXa { get; set; }
        public string TenQuanHuyen { get; set; }
        public string MaPhuongXa { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? TotalRecords { get; set; }
    }
}
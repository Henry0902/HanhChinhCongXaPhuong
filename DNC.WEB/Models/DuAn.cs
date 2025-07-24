namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DuAn")]
    public partial class DuAn
    {
        [Key]
        public int duan_id { get; set; }
        public string duan_ma { get; set; }
        public string duan_ten { get; set; }
        public string duan_mota { get; set; }
        public DateTime? ngay_tao { get; set; }
        public int trang_thai { get; set; }
    }

    public partial class DuAnSearch
    {
        [Key]
        public int duan_id { get; set; }
        public string duan_ma { get; set; }
        public string duan_ten { get; set; }
        public string duan_mota { get; set; }
        public DateTime? ngay_tao { get; set; }
        public int trang_thai { get; set; }
        public int? TotalRecords { get; set; }
    }
}
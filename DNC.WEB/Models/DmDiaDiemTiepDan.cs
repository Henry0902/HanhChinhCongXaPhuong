namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DmDiaDiemTiepDan")]
    public partial class DmDiaDiemTiepDan
    {
        [Key]
        public int Id { get; set; }
        public string Ten { get; set; }
        public int IdDonVi { get; set; }
        public string DiaChi { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
    }

    public partial class DmDiaDiemTiepDanSearch
    {
        [Key]
        public int id { get; set; }
        public string Ten { get; set; }
        public int IdDonVi { get; set; }
        public string DiaChi { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? TotalRecords { get; set; }

    }
}
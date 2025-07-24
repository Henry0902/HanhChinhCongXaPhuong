namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DmCoQuan")]
    public partial class DmCoQuan
    {
        [Key]
        public int Id { get; set; }
        public string TenCoQuan { get; set; }
        public int ThamQuyenGiaiQuyet { get; set; }
        public int TrangThai { get; set; }
    }
}
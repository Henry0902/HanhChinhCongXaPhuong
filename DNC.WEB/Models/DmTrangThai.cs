namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DmTrangThai")]
    public partial class DmTrangThai
    {
        [Key]
        public int Id { get; set; }
        public string TenTrangThai { get; set; }
        public int TrangThai { get; set; }
    }
}
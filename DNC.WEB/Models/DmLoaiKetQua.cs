namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DmLoaiKetQua")]
    public partial class DmLoaiKetQua
    {
        [Key]
        public int Id { get; set; }
        public string TenLoaiKetQua { get; set; }
        public int TrangThai { get; set; }
    }
}
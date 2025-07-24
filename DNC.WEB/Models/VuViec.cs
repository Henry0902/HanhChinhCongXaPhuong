using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("VuViec")]
    public partial class VuViec
    {
        [Key]
        public int Id { get; set; }
        public string NoiDungVuViec { get; set; }
        public int LoaiVuViecId { get; set; }
        public int LinhVucId { get; set; }
        public int NguoiTaoId { get; set; }
        public DateTime? NgayTao { get; set; }
        public int NguoiCapNhatId { get; set; }
        public DateTime? NgayCapNhat { get; set; }
    }
}
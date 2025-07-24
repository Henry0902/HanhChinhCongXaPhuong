namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SystemLogs
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string UserName { get; set; }

        public DateTime ActionDate { get; set; }

        [Required]
        [StringLength(500)]
        public string ActionName { get; set; }

        [StringLength(50)]
        public string IPAddress { get; set; }

        [StringLength(50)]
        public string MacAddress { get; set; }

        public Byte Type { get; set; }

        [Required]
        public string Message { get; set; }

        public string InnerException { get; set; }

        public string UserAgent { get; set; }

        public string RawURL { get; set; }

        [StringLength(500)]
        public string Method { get; set; }

        public bool Status { get; set; }  
    }

    public partial class SystemLogsSearch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string UserName { get; set; }

        public DateTime ActionDate { get; set; }

        [Required]
        [StringLength(500)]
        public string ActionName { get; set; }

        [StringLength(50)]
        public string IPAddress { get; set; }

        [StringLength(50)]
        public string MacAddress { get; set; }

        public Byte Type { get; set; }

        [Required]
        public string Message { get; set; }

        public string InnerException { get; set; }

        public string UserAgent { get; set; }

        public string RawURL { get; set; }

        [StringLength(500)]
        public string Method { get; set; }
        public bool Status { get; set; }  
        public int TotalRecords { get; set; }
    }
}

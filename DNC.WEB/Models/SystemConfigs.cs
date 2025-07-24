namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemConfigs")]
    public partial class SystemConfigs
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Key { get; set; }

        [StringLength(250)]
        public string Value { get; set; }

        public bool? CanEdit { get; set; }

        public int? Type { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedBy { get; set; }
    }

    [Table("SystemConfigs")]
    public partial class SystemConfigsSearch
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Key { get; set; }

        [StringLength(250)]
        public string Value { get; set; }
        public bool? CanEdit { get; set; }

        public int? Type { get; set; }

        public int TotalRecords { get; set; }
    }
}

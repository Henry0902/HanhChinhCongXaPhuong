namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pages
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public int OrderNo { get; set; }
        public string CssClass { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public int? ParentId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public partial class PagesView
    {
        public int PageId { get; set; }
        public string FunctionId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        [StringLength(10)]
        public string Target { get; set; }

        [StringLength(100)]
        public string CssClass { get; set; }

        public int? ParentId { get; set; }

        public int? OrderNo { get; set; }
        public int? hasChild { get; set; }
    }

    public class PageRole
    {
        public int PageId { get; set; }
        public string FunctionId { get; set; }
    }

    public partial class PagesViewRole
    {
        public int PageId { get; set; }
        public string Functions { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        [StringLength(10)]
        public string Target { get; set; }

        [StringLength(100)]
        public string CssClass { get; set; }

        public int? ParentId { get; set; }

        public int? OrderNo { get; set; }
        public int? hasChild { get; set; }
    }

}

namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DepartmentUsers
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int DepartmentId { get; set; }
    }
}

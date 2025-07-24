namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UsersRoles
    {
        public int Id { get; set; }
        public int DerpartmentId { get; set; }
        public int UserId { get; set; }

        public int RoleId { get; set; }
        
    }

    public class RoleUserView
    {
        public DateTime? Duedate { get; set; }
        public int UserId { get; set; }
        public int SessionID { get; set; }
    }
}

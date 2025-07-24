namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        [StringLength(255)]
        public string Password { get; set; }
        public string DisplayName { get; set; }
        [StringLength(255)]
        public string Avatar { get; set; }
        public bool? Gender { get; set; }
         
        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(255)]
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(250)]
        public string Position { get; set; }
        public int DepartmentId { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }
        public int Status { get; set; }
        public int IsSuper { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [StringLength(20)]
        [Index(IsUnique = true)]
        public string IdCard { get; set; }

        public DateTime? IssuanceDate { get; set; }

        [StringLength(255)]
        public string IssuanceAgency { get; set; }

        public int? EthnicId { get; set; }

        public int? NationalityId { get; set; }

        [StringLength(255)]
        public string SpecifiedAddress { get; set; }

        public int? ProvinceId { get; set; }

        public int? DistrictId { get; set; }

        public int? CommuneId { get; set; }

        public int? WrongLogin { get; set; }

        public DateTime? WrongLoginDate { get; set; }
    }
    public class UsersLogin
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public bool IsSuper { get; set; }
        public bool IsLocked { get; set; }
    }

    public class UsersGridView
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public int DisplayId { get; set; }
        public string Mobile { get; set; }
        public string Email  { get; set; }
        public string Position { get; set; } 
        public DateTime? CreatedDate { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public int DepartmentId { get; set; }
        public int CountTotal { get; set; }
    }

    public class UsersSuggest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Display { get; set; }
    }

    public class UserInfoMail
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    public partial class UpdateProfileRequest
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string FileName { get; set; }

        public string AvatarFile { get; set; }

        public bool? Gender { get; set; }

        public string DateOfBirth { get; set; }

        [StringLength(20)]
        [Index(IsUnique = true)]
        public string IdCard { get; set; }

        public string IssuanceDate { get; set; }

        [StringLength(255)]
        public string IssuanceAgency { get; set; }

        public int? EthnicId { get; set; }

        public int? NationalityId { get; set; }

        [StringLength(255)]
        public string SpecifiedAddress { get; set; }

        public int? ProvinceId { get; set; }

        public int? DistrictId { get; set; }

        public int? CommuneId { get; set; }

        public int Status { get; set; }
    }


    public partial class UserModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        
        public string DisplayName { get; set; }
        
        public string Avatar { get; set; }
        
        public bool? Gender { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        public int Status { get; set; }
        
        public int IsSuper { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        
        public string IdCard { get; set; }

        public DateTime? IssuanceDate { get; set; }

        public string IssuanceAgency { get; set; }

        public int? EthnicId { get; set; }

        public int? NationalityId { get; set; }

        public bool IsLocked { get; set; }

        public string FullAddress { get; set; }
    }
}

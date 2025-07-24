using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNC.WEB.Models
{
    [Table("UsersTemp")]
    public partial class UsersTemp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }

        [StringLength(255)]
        public string Avatar { get; set; }

        [Required]
        public bool Gender { get; set; }

        [StringLength(20)]
        [Index(IsUnique = true)]
        public string Mobile { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(255)]
        public string Position { get; set; }

        public int? DepartmentId { get; set; }

        [Required]
        public bool IsLocked { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public bool IsSuper { get; set; }

        [StringLength(20)]
        [Index(IsUnique = true)]
        [Required]
        public string IdCard { get; set; }

        public DateTime? IssuanceDate { get; set; }

        [StringLength(255)]
        public string IssuanceAgency { get; set; }

        public int? EthnicId { get; set; }

        public int? NationalityId { get; set; }

        [StringLength(255)]
        [Required]
        public string SpecifiedAddress { get; set; }

        [Required]
        public int ProvinceId { get; set; }

        [Required]
        public int DistrictId { get; set; }

        [Required]
        public int CommuneId { get; set; }

        [StringLength(255)]
        public string IpAddress { get; set; }

        [Required]
        [StringLength(6)]
        public string Otp { get; set; }

        [Required]
        public DateTime OtpStartTime { get; set; }

        [Required]
        public DateTime OtpEndTime { get; set; }

        [Required]
        public int OtpStatus { get; set; }

        [Required]
        public int TimesEntryOtp { get; set; }

        [Required]
        public int TimesSendOtp { get; set; }

        [Required]
        public DateTime OtpFirstSend { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateUserTempRequest
    {
        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        [Required]
        [StringLength(32)]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }

        [Required]
        public int Gender { get; set; }

        [StringLength(20)]
        [Index(IsUnique = true)]
        public string Mobile { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public string DateOfBirth { get; set; }

        [StringLength(20)]
        [Index(IsUnique = true)]
        [Required]
        public string IdCard { get; set; }

        public string IssuanceDate { get; set; }

        [StringLength(255)]
        public string IssuanceAgency { get; set; }

        public int? EthnicId { get; set; }

        public int? NationalityId { get; set; }

        [StringLength(255)]
        [Required]
        public string SpecifiedAddress { get; set; }

        [Required]
        public int ProvinceId { get; set; }

        [Required]
        public int DistrictId { get; set; }

        [Required]
        public int CommuneId { get; set; }
    }
}

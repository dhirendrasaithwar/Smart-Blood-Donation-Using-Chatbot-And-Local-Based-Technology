using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class UserViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsUserVerify { get; set; }
        public long BloodTypeId { get; set; }
        public string BloodType { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsPreviousDonate { get; set; }
        public DateTime? LastDonationDate { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class Login
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class UserClaimViewModel
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string UserName { get; set; }
        public long BloodTypeId { get; set; }
        public bool Status { get; set; }
        public List<string> Roles { get; set; }
    }
    public class Register
    {
        public long UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again! ")]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool IsUserVerify { get; set; }
        [Required]
        public string CityName { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public long BloodTypeId { get; set; }
        public string BloodType { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string IsPreviousDonate { get; set; }
        public DateTime? LastDonationDate { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class UserAndBloodCount
    {
        public UserAndBloodCount()
        {
            BloodStockInfo = new List<Tuple<string, long, long?>>();
        }
        public int TotalUser { get; set; }
        public int TotalBloodRequest { get; set; }
        public int PendingRequest { get; set; }
        public int CompletedRequest { get; set; }
        public int NotManagedRequest { get; set; }

        public int TotalBloodRequestOfUser { get; set; }
        public int Reserve{get;set;}
        public int PendingRequestOfUser { get; set; }
        public int CompletedRequestOfUser { get; set; }
        public int NotManagedRequestOfUser { get; set; }
        public List<Tuple<string,long,long?>> BloodStockInfo { get; set; }
    }
    public class ForgotPassword
    {
        public long UserId { get; set; }
        [Required]
        public string Email { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }
    }

    public class ChangePassword
    {
        [Required]
        [MinLength(6)]
        public string CurrentPassword { get; set; }
        [Required]
        [MinLength(6)]
        public string NewPassword{get;set;}
        [Required]
        [MinLength(6)]
        [Compare("NewPassword", ErrorMessage = "Current password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class ResetPassword
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required(ErrorMessage = "Re-Enter your Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public string ResetToken { get; set; }
    }
}

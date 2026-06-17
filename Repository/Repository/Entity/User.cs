namespace Repository
{
    public class User
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
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StreetAddress { get; set; }
        public string CityName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual BloodType BLOODTYPE { get; set; }
        public virtual ICollection<Donation>DONATION { get; set; }
        public virtual ICollection<UserRole> USERROLE { get; set; }
        public virtual ICollection<BloodRequest>  BLOODREQUEST { get; set; }
        public virtual ICollection<DonarRequest>  DONARREQUEST { get; set; }
        public virtual ICollection<DonarRequest>  REQUEST { get; set; }
        public virtual ICollection<ChatBot>  CHATBOOT { get; set; }
    }
}

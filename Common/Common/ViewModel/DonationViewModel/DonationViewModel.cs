namespace Common
{
    public class DonationViewModel
    {
        public long DonationId { get; set; }
        public long UserId { get; set; }
        public DateTime DonationDate { get; set; }
        public string BloodType { get; set; }
        public string Location { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class CreateDonation
    {
        public long DonationId { get; set; }
        public long UserId { get; set; }
        public DateTime DonationDate { get; set; }
        public string Location { get; set; }
        public long BloodTypeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class DonationList
    {
        public long DonationId { get; set; }
        public long UserId { get; set; }
        public DateTime DonationDate { get; set; }
        public string BloodType { get; set; }
        public string Location { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }
}

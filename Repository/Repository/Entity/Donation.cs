namespace Repository
{
    public class Donation
    {
        public long DonationID { get; set; }
        public long UserId { get; set; }
        public DateTime DonationDate { get; set; }
        public long BloodTypeId { get; set; }
        public string Location { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        public BloodType BloodType { get; set; }

        public virtual User USER { get; set; }
    }
}

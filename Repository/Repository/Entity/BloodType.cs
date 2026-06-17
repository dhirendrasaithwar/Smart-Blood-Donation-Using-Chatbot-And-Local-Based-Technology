namespace Repository
{
    public class BloodType
    {
        public long BloodTypeId { get; set; }
        public string BloodTypes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        public virtual ICollection<Donation> Donation { get; set; }

        public virtual ICollection<User> USER { get; set; }
        public virtual ICollection<BloodRequest> BLOODREQUEST { get; set; }
        public virtual ICollection<BloodStock>  BLOODSTOCK { get; set; }
    }
}

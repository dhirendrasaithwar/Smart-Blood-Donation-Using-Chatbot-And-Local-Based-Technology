using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class BloodRequestViewModel
    {
        public long BloodRequestId { get; set; }
        public long? UserId { get; set; }
        public long BloodTypeId { get; set; }
        public string BloodType { get; set; }
        public int Quantity { get; set; }
        public DateTime RequiredDate { get; set; }
        public string UrgencyType { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class CreateBloodRequest
    {
        public long BloodRequestId { get; set; }
        public long? UserId { get; set; }
        [Required]
        public long BloodTypeId { get; set; }
        public string BloodType { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime RequiredDate { get; set; }
        [Required]
        public string UrgencyType { get; set; }
        [Required]
        public string Location { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class UpdateBloodRequest
    {
        public long BloodRequestId { get; set; }
        public long? UserId { get; set; }
        [Required]
        public long BloodTypeId { get; set; }
        public string BloodType { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime RequiredDate { get; set; }
        [Required]
        public string UrgencyType { get; set; }
        [Required]
        public string Location { get; set; }
        public string Status { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

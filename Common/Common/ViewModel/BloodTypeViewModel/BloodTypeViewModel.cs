using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class BloodTypeViewModel
    {
        public long BloodTypeId { get; set; }
        public string BloodTypes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class CreateBloodType
    {
        public long BloodTypeId { get; set; }
        public string BloodTypes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class UpdateBloodType
    {
        public long BloodTypeId { get; set; }
        [Required]
        public string BloodTypes { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Common;

public class BloodStockViewModel
{
    public long BloodSockId{get;set;}
    public long BloodTypeId { get; set; }
    public string BloodTypeName { get; set; }
    public string Status { get; set; }
    public long Quantity { get; set; }
    public long InHold{ get; set; }
}

public class AddBloodStockViewModel
{
    [Required]
    public long BloodTypeId { get; set; }
    [Required]
    public long Quantity{ get; set; }
}

public class UpdateBloodStockViewModel
{
    [Required]
    public long BloodStockId { get; set; }
    [Required]
    public long BloodTypeId { get; set; }
    [Required]
    public long Quantity { get; set; }
}
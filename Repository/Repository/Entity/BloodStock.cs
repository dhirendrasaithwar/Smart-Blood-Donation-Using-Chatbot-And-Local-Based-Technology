namespace Repository;

public class BloodStock
{
    public long BloodStockId { get; set; }
    public long? ReserveQuantity { get; set; }
    public DateTime LastUpdated { get; set; }
    public long BloodTypeId { get; set; }
    public long InHold{get; set;}
    
    public BloodType BLOODTYPES { get; set; }
}
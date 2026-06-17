namespace Repository;

public class DonarRequest
{
    public long DonarRequestId{get;set;}
    public long DonarId{get;set;}
    public long RequesterId{get;set;}
    public DateTime RequestDate{get;set;}
    public long Quantity { get; set; }
    public string Status { get; set; }
    public string DonationDate { get; set; }
    public string DonationTime { get; set; }
    public User DonarUser{get;set;}
    public User Requester{get;set;}
}
namespace Common.ViewModel.SearchViewModel;

public class DonarSearch
{
    public string CityName { get; set; }
    public string StateName { get; set; }
    public long BloodGroupId { get; set; }
}

public class DonarSearchResult
{
    public long DonarId { get; set; }
    public string DonarName { get; set; }
    public string CityName { get; set; }
    public string StreetName { get; set; }
    public string Status { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public string BloodTypeName { get; set; }
}

public class RequestDonar
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string DonationDate { get; set; }
    public string DonationTime { get; set; }
}
public class RequestDonarResult
{
    public long Id { get; set; }
    public string DonarName { get; set; }
    public string RequesterName { get; set; }
    public string Status { get; set; }
    public string DonationDate { get; set; }
    public string DonationTime { get; set; }
    public string RequesterAddress { get; set; }
    public string DonationAddress { get; set; }
    public string DonationPhoneNumber { get; set; }
    public string RequesterPhoneNumber { get; set; }
}
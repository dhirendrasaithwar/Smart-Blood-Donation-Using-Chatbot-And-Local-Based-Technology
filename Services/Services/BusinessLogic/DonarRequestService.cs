using System.Globalization;
using Common;
using Common.ViewModel.SearchViewModel;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Services;

public class DonarRequestService : IDonarRequestService
{
    private readonly IUnitOfWork _unitOfWork;
    public DonarRequestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public BaseResponseModel<List<DonarSearchResult>> SearchDonar(string streetName, string cityName, long bloodTypeId)
    {
        BaseResponseModel<List<DonarSearchResult>> response = new();
        try
        {
            var result = _unitOfWork._db.Users.Include(x => x.BLOODTYPE).Where(x => x.BloodTypeId == bloodTypeId && (x.StreetAddress == streetName || x.CityName == cityName)).ToList();
            if (result.Count < 0)
            {
                response.Status = "00";
                response.Message = "No Donor Found Nearby!!!";
                return response;
            }

            response.Data = result.Select(x => new DonarSearchResult()
            {
                CityName = x.CityName,
                DonarId = x.UserId,
                StreetName = x.StreetAddress,
                DonarName = x.FullName,
                Status = StaticMethods.BloodDonationStatus(Convert.ToDateTime(x.LastDonationDate)),
                EmailAddress = x.Email,
                BloodTypeName = x.BloodType,
                PhoneNumber = x.PhoneNumber,
            }).ToList();
            if (response.Data.Any(x => x.DonarId == _unitOfWork.ActionUser.UserId))
            {
                response.Data.Remove(response.Data.FirstOrDefault(x => x.DonarId == _unitOfWork.ActionUser.UserId));
            }
            response.Status = "00";
            response.Message = "Donor Found Nearby!!!";
            return response;
        }
        catch (Exception ex)
        {
            response.Status = "1";
            response.Message = ex.Message;
            return response;
        }
    }

    public BaseResponseModel<List<RequestDonarResult>> DonationList()
    {
        BaseResponseModel<List<RequestDonarResult>> response = new();
        try
        {
            var actionUser = _unitOfWork.ActionUser;
            var list = _unitOfWork._db.DonarRequests.Include(x => x.Requester).Where(x => x.DonarId == actionUser.UserId).ToList();
            if (list.Count == 0)
            {
                response.Status = "00";
                response.Message = "No Request Available!!!";
                return response;
            }
            response.Data = list.Select(x => new RequestDonarResult()
            {
                Status = x.Status,
                DonarName = actionUser.FullName,
                DonationAddress = $"{actionUser.CityName},{actionUser.StreetAddress}",
                DonationDate = x.DonationDate,
                DonationPhoneNumber = actionUser.PhoneNumber,
                DonationTime = x.DonationTime,
                Id = x.DonarRequestId,
                RequesterAddress = $"{x.Requester.CityName},{x.Requester.StreetAddress}",
                RequesterName = x.Requester.FullName,
                RequesterPhoneNumber = x.Requester.PhoneNumber,
            }).ToList();
            response.Status = "00";
            response.Message = "Donation List Available!!!";
            return response;
        }
        catch (Exception ex)
        {
            response.Status = "1";
            response.Message = ex.Message;
            return response;
        }
    }

    public BaseResponseModel<List<string>> RequestDoner(RequestDonar model)
    {
        BaseResponseModel<List<string>> response = new();
        try
        {
            if (model.UserId == 0)
            {
                response.Status = "1";
                response.Message = "No User Found!!!";
                return response;
            }
            var lastDonation = _unitOfWork._db.Donations.Where(x => x.UserId == model.UserId).ToList();
            if (lastDonation.Any())
            {
                var lastest = lastDonation.OrderByDescending(x => x.DonationDate).FirstOrDefault();
                if (lastDonation != null)
                {
                    if (StaticMethods.CheckEligibility(lastest.DonationDate) == false)
                    {
                        response.Status = "1";
                        response.Message = "Cannot Donate now!!!";
                        return response;
                    }
                }
            }

            var donar = new DonarRequest()
            {
                DonarId = model.UserId,
                Status = "Pending",
                RequestDate = DateTime.Now,
                DonationDate = model.DonationDate,
                DonationTime = model.DonationTime,
                Quantity = 1,
                RequesterId = _unitOfWork.ActionUser.UserId,
            };
            _unitOfWork._db.DonarRequests.Add(donar);
            _unitOfWork._db.SaveChanges();

            response.Status = "00";
            response.Message = "Donation Added Successfully!!!";
            return response;
        }
        catch (Exception ex)
        {
            response.Status = "1";
            response.Message = ex.Message;
            return response;
        }
    }

    public BaseResponseModel<List<RequestDonarResult>> RequestList()
    {
        BaseResponseModel<List<RequestDonarResult>> response = new();
        try
        {
            var actionUser = _unitOfWork.ActionUser;
            var list = _unitOfWork._db.DonarRequests.Include(x => x.DonarUser).Where(x => x.RequesterId == actionUser.UserId).ToList();
            if (list.Count == 0)
            {
                response.Status = "00";
                response.Message = "No Request Available!!!";
                return response;
            }
            response.Data = list.Select(x => new RequestDonarResult()
            {
                Status = x.Status,
                DonarName = x.DonarUser.FullName,
                DonationAddress = $"{x.DonarUser.CityName},{x.DonarUser.StreetAddress}",
                DonationDate = x.DonationDate,
                DonationPhoneNumber = x.DonarUser.PhoneNumber,
                DonationTime = x.DonationTime,
                Id = x.DonarRequestId,
                RequesterAddress = $"{x.Requester.CityName},{x.Requester.StreetAddress}",
                RequesterName = actionUser.FullName,
                RequesterPhoneNumber = actionUser.PhoneNumber,
            }).ToList();
            response.Status = "00";
            response.Message = "Donation List Available!!!";
            return response;
        }
        catch (Exception ex)
        {
            response.Status = "1";
            response.Message = ex.Message;
            return response;
        }
    }

    public BaseResponseModel<string> Accept(long id)
    {
        BaseResponseModel<string> response = new();
        try
        {
            var request = _unitOfWork._db.DonarRequests.FirstOrDefault(x => x.DonarRequestId == id);
            if (request == null)
            {
                response.Status = "00";
                response.Message = "No Request Available!!!";
                return response;
            }
            var lastDonation = _unitOfWork._db.Donations.Where(x => x.UserId == _unitOfWork.ActionUser.UserId).ToList();
            if (lastDonation.Any())
            {
                var lastest = lastDonation.OrderByDescending(x => x.DonationDate).FirstOrDefault();
                if (lastDonation == null)
                {
                    request.Status = "Rejected";
                    _unitOfWork._db.DonarRequests.Update(request);
                    _unitOfWork._db.SaveChanges();
                    if (StaticMethods.CheckEligibility(lastest.DonationDate) == false)
                    {
                        response.Status = "1";
                        response.Message = "Cannot Donate now!!!";
                        return response;
                    }
                }
            }
            request.Status = "Accepted";
            _unitOfWork._db.DonarRequests.Update(request);
            _unitOfWork._db.SaveChanges();


            var donation = new Donation()
            {
                DonationDate = DateTime.ParseExact(

                    request.DonationDate,

                    "yyyy-MM-dd",

                    CultureInfo.InvariantCulture

                ),
                UserId = _unitOfWork.ActionUser.UserId,
                BloodTypeId = _unitOfWork.ActionUser.BloodTypeId,
                CreatedBy = _unitOfWork.ActionUser.CreatedBy,
                Location = _unitOfWork.ActionUser.Address,
                CreatedDate = DateTime.Now,
            };
            _unitOfWork._db.Donations.Add(donation);
            _unitOfWork._db.SaveChanges();
            var user = _unitOfWork._db.Users.FirstOrDefault(x => x.UserId == _unitOfWork.ActionUser.UserId);
            user.LastDonationDate = donation.DonationDate;
            _unitOfWork._db.Users.Update(user);
            _unitOfWork._db.SaveChanges();
            response.Status = "00";
            response.Message = "Donation Added Successfully!!!";
            return response;
        }
        catch (Exception ex)
        {
            response.Status = "1";
            response.Message = ex.Message;
            return response;
        }
    }

    public BaseResponseModel<string> Reject(long id)
    {
        BaseResponseModel<string> response = new();
        try
        {
            var request = _unitOfWork._db.DonarRequests.FirstOrDefault(x => x.DonarRequestId == id);
            if (request == null)
            {
                response.Status = "00";
                response.Message = "No Request Available!!!";
                return response;
            }
            request.Status = "Declined";
            _unitOfWork._db.DonarRequests.Update(request);
            _unitOfWork._db.SaveChanges();

            response.Status = "00";

            response.Message = "Request Declined!!!";
            return response;
        }
        catch (Exception ex)
        {
            response.Status = "1";
            response.Message = ex.Message;
            return response;
        }
    }
}

public interface IDonarRequestService
{
    BaseResponseModel<List<DonarSearchResult>> SearchDonar(string streetName, string cityName, long bloodTypeId);
    BaseResponseModel<List<RequestDonarResult>> DonationList();
    BaseResponseModel<List<string>> RequestDoner(RequestDonar model);
    BaseResponseModel<List<RequestDonarResult>> RequestList();

    BaseResponseModel<string> Accept(long id);
    BaseResponseModel<string> Reject(long id);
}   
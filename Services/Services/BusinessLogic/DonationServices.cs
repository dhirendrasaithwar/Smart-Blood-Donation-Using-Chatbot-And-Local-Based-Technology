using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository;

namespace Services
{
    public class DonationServices: IDonationServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DonationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BaseResponseModel<string> Create(CreateDonation model)
        {
            BaseResponseModel<string> response = new BaseResponseModel<string>();
            try
            {
                var actionUser = _unitOfWork.ActionUser;
                if (actionUser == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }

                bool allowed = true;
                var lastDonation = _unitOfWork._db.Donations.Where(x=>x.UserId==actionUser.UserId).ToList().OrderByDescending(x=>x.DonationDate).FirstOrDefault();
                if (lastDonation != null)
                {
                    if (DateTime.Now < lastDonation.DonationDate.AddMonths(3))
                    {
                        allowed = false;
                    }
                    else
                    {
                        allowed = true;
                    }
                }

                if (string.IsNullOrWhiteSpace(model.Location))
                {
                    response.Status = "1";
                    response.Message = "Please enter a location";
                    return response;
                }
                var bloodType = _unitOfWork._db.BloodTypes.FirstOrDefault(x => x.BloodTypeId == actionUser.BloodTypeId);
                if (bloodType == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }
                
                var transaction = _unitOfWork._db.Database.BeginTransaction();
                try
                {
                    var donation = new Donation()
                    {
                        DonationDate = DateTime.Now,
                        UserId = actionUser.UserId,
                        BloodTypeId = actionUser.BloodTypeId,
                        CreatedBy = actionUser.UserName,
                        CreatedDate = DateTime.Now,
                        Location = model.Location,
                    };
                    _unitOfWork._db.Donations.Add(donation);
                    
                    if (!_unitOfWork._db.BloodStocks.Any(x =>x.BloodTypeId == actionUser.BloodTypeId))
                    {
                        var stock = new BloodStock()
                        {
                            BloodTypeId = actionUser.BloodTypeId,
                            InHold = 0,
                            ReserveQuantity = 1,
                            LastUpdated = DateTime.Now,
                        };
                        _unitOfWork._db.BloodStocks.Add(stock);
                    }
                    else
                    {
                        var bloodStock = _unitOfWork._db.BloodStocks.FirstOrDefault(x => x.BloodTypeId == actionUser.BloodTypeId);

                        bloodStock.ReserveQuantity = bloodStock.ReserveQuantity +1;
                    
                        _unitOfWork._db.BloodStocks.Update(bloodStock);                        
                    }
                    
                    
                    _unitOfWork._db.SaveChanges();
                    
                    transaction.Commit();
                    response.Status = "00";
                    response.Message = "Success";
                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Status = "1";
                    response.Message = "Technical error occur";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }
        public BaseResponseModel<List<DonationViewModel>> GetDonationByUser()
        {
            BaseResponseModel<List<DonationViewModel>> response = new BaseResponseModel<List<DonationViewModel>>();
            try
            {
                var actionUser = _unitOfWork.ActionUser;

                var userDonation = _unitOfWork._db.Donations.Include(x => x.BloodType).Where(x=>x.UserId== actionUser.UserId).ToList();
                if(userDonation == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }
                else
                {
                    var donation = userDonation.Select(x => new DonationViewModel
                    {
                        DonationId = x.DonationID,
                        UserId = x.UserId,
                        DonationDate = x.DonationDate,
                        BloodType = x.BloodType.BloodTypes,
                        Location= x.Location,
                    }).ToList();

                    response.Status = "0";
                    response.Message = "Success";
                    response.Data = donation;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = "1";
                response.Message = "Techinical error occur";
                return response;
            }
        }
        public BaseResponseModel<List<DonationList>> DonationList()
        {
            BaseResponseModel<List<DonationList>> response = new BaseResponseModel<List<DonationList>>();
            try
            {
                var userDonation = _unitOfWork._db.Donations.Include(x => x.USER)
                    .Include(x => x.BloodType).ToList();
                if (userDonation == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }
                else
                {
                    var donation = userDonation.Select(x => new DonationList
                    {
                        DonationId = x.DonationID,
                        UserId = x.UserId,
                        DonationDate = x.DonationDate,
                        BloodType = x.BloodType.BloodTypes,
                        Location = x.Location,
                        UserName = x.USER.UserName,
                        PhoneNumber= x.USER.PhoneNumber,
                    }).ToList();

                    response.Status = "0";
                    response.Message = "Success";
                    response.Data = donation;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }
        public BaseResponseModel<string> Delete(long DonationID)
        {
            BaseResponseModel<string> response = new BaseResponseModel<string>();
            try
            {
                var data = _unitOfWork._db.Donations.Where(X=>X.DonationID== DonationID).FirstOrDefault();
                if (data != null)
                {
                    _unitOfWork._db.Donations.Remove(data);
                    _unitOfWork._db.SaveChanges();

                    response.Status = "0";
                    response.Message = "success";
                    return response;
                }
                else
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = "1";
                response.Message = "Techinical error occur";
                return response;
            }
        }

        public BaseResponseModel<string> GetDonationPrevInfo()
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var actionUser = _unitOfWork.ActionUser;
                var lastDonation = _unitOfWork._db.Donations.Where(x=>x.UserId==actionUser.UserId).ToList();
                if (lastDonation == null || lastDonation.Count == 0)
                {
                    response.Status = "00";
                    response.Message = "No data found";
                    return response;
                }
                var lastest = lastDonation.OrderByDescending(x=>x.DonationDate).FirstOrDefault();
                if (DateTime.Now < lastest.DonationDate.AddMonths(3))
                {
                    DateTime nextEligibleDate = lastest.DonationDate.AddMonths(3);
                    TimeSpan remaining = nextEligibleDate - DateTime.Now;

                    response.Status = "1";
                    response.Message = $"Sorry, you can donate after {nextEligibleDate:dd MMM yyyy}. ({remaining.Days} days remaining)";
                    return response;
                }
                response.Status = "00";
                response.Message = "Success";
                return response;
            }catch(Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }
    }
    public interface IDonationServices
    {
        BaseResponseModel<string> Create(CreateDonation model);
        BaseResponseModel<List<DonationViewModel>> GetDonationByUser();
        BaseResponseModel<List<DonationList>> DonationList();
        BaseResponseModel<string> Delete(long DonationID);
        BaseResponseModel<string> GetDonationPrevInfo();
    }
}

using Common;
using Repository;
using System.Collections.Generic;

namespace Services
{
    public class BloodRequestServices : IBloodRequestServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public BloodRequestServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BaseResponseModel<string> Create(CreateBloodRequest model)
        {
            BaseResponseModel<string> response = new BaseResponseModel<string>();
            try
            {
                if (model.RequiredDate < DateTime.Now)
                {
                    response.Status = "1";
                    response.Message = "Required date must be in the future";
                    return response;
                }
                var actionUser = _unitOfWork.ActionUser;
                
var bloodType = _unitOfWork._db.BloodTypes.FirstOrDefault(x => x.BloodTypeId == model.BloodTypeId);
if (bloodType == null)
{
 response.Status = "1";
 response.Message = "Blood type not found";
 return response;
}

string status = null;
var stock = _unitOfWork._db.BloodStocks.FirstOrDefault(x => x.BloodTypeId == model.BloodTypeId);
if (stock != null && (stock.ReserveQuantity - stock.InHold) >= model.Quantity)
{
    status ="Reserved";
}
else
{
    status = "Pending";
}
var transaction = _unitOfWork._db.Database.BeginTransaction();
try
{
    var request = new BloodRequest()
    {
        BloodTypeId = bloodType.BloodTypeId,
        CreatedBy = actionUser.CreatedBy,
        BloodType = bloodType.BloodTypes,
        Location = model.Location,
        RequiredDate = model.RequiredDate,
        UserId = actionUser.UserId,
        Status = status,
        Quantity = model.Quantity,
        UrgencyType = model.UrgencyType,
        CreatedDate = DateTime.Now,
    };
    _unitOfWork._db.BloodRequests.Add(request);
    if (stock != null)
    {
        stock.ReserveQuantity = stock.ReserveQuantity - model.Quantity;
        stock.InHold = stock.InHold + model.Quantity;
    
        _unitOfWork._db.BloodStocks.Update(stock);        
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




                response.Status = "0";
                response.Message = "Requested";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }
        public BaseResponseModel<List<BloodRequestViewModel>> GetBloodRequestsByUser()
        {
            BaseResponseModel<List<BloodRequestViewModel>> response = new BaseResponseModel<List<BloodRequestViewModel>>();
            try
            {
                var actionUser = _unitOfWork.ActionUser;
                var data = _unitOfWork._db.BloodRequests.Where(x => x.UserId == actionUser.UserId).ToList();
                if (data == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }
                else
                {
                    var blood = data.Select(x => new BloodRequestViewModel
                    {
                        UserId = x.UserId,
                        BloodRequestId = x.BloodRequestId,
                        BloodTypeId = x.BloodTypeId,
                        BloodType = x.BloodType,
                        UrgencyType = x.UrgencyType,
                        Location = x.Location,
                        Status = x.Status,
                        RequiredDate = x.RequiredDate,
                        CreatedBy = x.CreatedBy,
                        Quantity = x.Quantity,

                    }).ToList();

                    response.Status = "00";
                    response.Message = "Getting data";
                    response.Data = blood;
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
        public BaseResponseModel<List<BloodRequestViewModel>> GetBloodRequests()
        {
            BaseResponseModel<List<BloodRequestViewModel>> response = new BaseResponseModel<List<BloodRequestViewModel>>();
            try
            {
                var data = _unitOfWork._db.BloodRequests.ToList();
                if (data == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }
                else
                {
                    var blood = data.Select(x => new BloodRequestViewModel
                    {
                        UserId = x.UserId,
                        BloodRequestId = x.BloodRequestId,
                        BloodTypeId = x.BloodTypeId,
                        BloodType = x.BloodType,
                        UrgencyType = x.UrgencyType,
                        Location = x.Location,
                        Status = x.Status,
                        RequiredDate = x.RequiredDate,
                        CreatedBy = x.CreatedBy,
                        Quantity = x.Quantity,

                    }).ToList();

                    response.Status = "00";
                    response.Message = "Getting data";
                    response.Data = blood;
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
        public BaseResponseModel<UpdateBloodRequest> GetBloodById(long BloodRequestId)
        {
            BaseResponseModel<UpdateBloodRequest> response = new BaseResponseModel<UpdateBloodRequest>();
            try
            {
                var data = _unitOfWork._db.BloodRequests.Where(x=>x.BloodRequestId== BloodRequestId).FirstOrDefault();
                if(data!= null)
                {
                    var blood = new UpdateBloodRequest
                    {
                        UserId = data.UserId,
                        BloodRequestId = data.BloodRequestId,
                        BloodTypeId = data.BloodTypeId,
                        BloodType = data.BloodType,
                        UrgencyType = data.UrgencyType,
                        Location = data.Location,
                        Status = data.Status,
                        RequiredDate = data.RequiredDate,
                        Quantity = data.Quantity,
                    };
                    response.Status = "00";
                    response.Message = "Getting data";
                    response.Data = blood;
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
                response.Message = "Technical error occur";
                return response;
            }
        }
        public BaseResponseModel<string> Update(UpdateBloodRequest model)
        {
            BaseResponseModel<string> response = new BaseResponseModel<string>();
            try
            {
                var blood = _unitOfWork._db.BloodRequests.Where(x=>x.BloodRequestId== model.BloodRequestId).FirstOrDefault();
                if(blood == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }
                else
                {
                    var actionUser = _unitOfWork.ActionUser;
                    var bloodtype = _unitOfWork._db.BloodTypes.Where(x=>x.BloodTypeId == model.BloodTypeId).FirstOrDefault();
                    if (bloodtype == null)
                    {
                        response.Status = "1";
                        response.Message = "No data found";
                        return response;
                    }
var transaction = _unitOfWork._db.Database.BeginTransaction();
try
{

    blood.UrgencyType = model.UrgencyType;
    blood.BloodTypeId= model.BloodTypeId;
    blood.BloodType = bloodtype.BloodTypes;
    blood.Location = model.Location;
    blood.RequiredDate= model.RequiredDate;
    blood.UpdatedBy = actionUser.UserName;
    blood.UpdatedDate = DateTime.Now;
    blood.Quantity= model.Quantity;

    _unitOfWork._db.BloodRequests.Update(blood);

    if (model.Status.ToUpper() == "COMPLETED")
    {
                    var stock = _unitOfWork._db.BloodStocks.FirstOrDefault(x => x.BloodTypeId == model.BloodTypeId);
                    if (stock != null)
                    {
                        stock.InHold = stock.InHold - blood.Quantity;
                    }
                    _unitOfWork._db.BloodStocks.Update(stock);
    }
    _unitOfWork._db.SaveChanges();

    transaction.Commit();
    response.Status="0";
    response.Message = "Success";
    return response;
}catch(Exception ex)
{
    transaction.Rollback();
    response.Status = "1";
    response.Message = "Technical error occur";
    return response;
}
                   
                }
            }
            catch (Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }
        public BaseResponseModel<string> Delete(long BloodRequestId)
        {
            BaseResponseModel<string> response = new BaseResponseModel<string>();
            try
            {
                var data = _unitOfWork._db.BloodRequests.Where(x=>x.BloodRequestId==BloodRequestId).FirstOrDefault();
                if(data == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }
                else
                {
                    _unitOfWork._db.BloodRequests.Remove(data);
                    _unitOfWork._db.SaveChanges();

                    response.Status="00";
                    response.Message = "Success";
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

        public BaseResponseModel<string> CannotManageBlood(long bloodRequestId)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var bloodRequest = _unitOfWork._db.BloodRequests.FirstOrDefault(x => x.BloodRequestId == bloodRequestId);
                if (bloodRequest == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }

                bloodRequest.Status = "NotManaged";
                bloodRequest.UpdatedBy = _unitOfWork.ActionUser.UserName;
                bloodRequest.UpdatedDate = DateTime.Now;
                _unitOfWork._db.BloodRequests.Update(bloodRequest);
                _unitOfWork._db.SaveChanges();
                
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

        public BaseResponseModel<string> Completed(long bloodRequestId)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var bloodRequest = _unitOfWork._db.BloodRequests.FirstOrDefault(x => x.BloodRequestId == bloodRequestId);
                if (bloodRequest == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }

                var transaction = _unitOfWork._db.Database.BeginTransaction();
                try
                {
                    bloodRequest.Status = "Completed";
                    bloodRequest.UpdatedBy = _unitOfWork.ActionUser.UserName;
                    bloodRequest.UpdatedDate = DateTime.Now;
                    _unitOfWork._db.BloodRequests.Update(bloodRequest);

                    var bloodStock =
                        _unitOfWork._db.BloodStocks.FirstOrDefault(x => x.BloodTypeId == bloodRequest.BloodTypeId);
                    if (bloodStock != null)
                    {
                        bloodStock.InHold = bloodStock.InHold - bloodRequest.Quantity;

                        _unitOfWork._db.BloodStocks.Update(bloodStock);

                    }

                    _unitOfWork._db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Status = "1";
                    response.Message = "Technical error occur";
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
    public interface IBloodRequestServices
    {
        BaseResponseModel<List<BloodRequestViewModel>> GetBloodRequestsByUser();
        BaseResponseModel<List<BloodRequestViewModel>> GetBloodRequests();
        BaseResponseModel<string> Create(CreateBloodRequest model);
        BaseResponseModel<string> Update(UpdateBloodRequest model);
        BaseResponseModel<UpdateBloodRequest> GetBloodById(long BloodRequestId);
        BaseResponseModel<string> Delete(long BloodRequestId);
        BaseResponseModel<string> CannotManageBlood(long bloodRequestId);
        BaseResponseModel<string> Completed(long bloodRequestId);
    }
}

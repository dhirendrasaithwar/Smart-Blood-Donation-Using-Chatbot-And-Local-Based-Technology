using Common;
using Repository;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Services
{
    public class BloodTypeServices: IBloodTypeServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public BloodTypeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BaseResponseModel<CreateBloodType> Create(CreateBloodType model)
        {
            BaseResponseModel<CreateBloodType> response = new BaseResponseModel<CreateBloodType>();
            try
            {
                var bloodType = _unitOfWork._db.BloodTypes.Where(x=>x.BloodTypes.ToLower() == model.BloodTypes.ToLower()).FirstOrDefault();
                if(bloodType != null)
                {
                    response.Status = "1";
                    response.Message = "Blood type is already exist";
                    return response;
                }
                if (model.BloodTypes == null)
                {
                    response.Status = "1";
                    response.Message = "Blood type is required";
                    return response;
                }
                var actionUser = _unitOfWork.ActionUser;
                var blood = new BloodType
                {
                    BloodTypes = model.BloodTypes,
                    CreatedBy = actionUser.UserName,
                    CreatedDate = DateTime.Now,
                };
                _unitOfWork._db.BloodTypes.Add(blood);
                _unitOfWork._db.SaveChanges();

                response.Status = "0";
                response.Message = "Blood type created";
                return response;
            }
            catch(Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }
        public BaseResponseModel<IEnumerable<BloodTypeViewModel>> AllBloodType()
        {
            BaseResponseModel<IEnumerable<BloodTypeViewModel>> response = new BaseResponseModel<IEnumerable<BloodTypeViewModel>>();
            try
            {
                var blood = _unitOfWork._db.BloodTypes.ToList();
                if(!blood.Any())
                {
                    response.Status = "1";
                    response.Message = "No records found";
                    return response;
                }
                else
                {
                    response.Status = "0";
                    response.Message = "Getting blood list";
                    response.Data = blood.Select(x=> new BloodTypeViewModel
                    {
                        BloodTypeId = x.BloodTypeId,
                        BloodTypes = x.BloodTypes,
                    }).ToList();
                    return response;
                }
            }
            catch(Exception ex )
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }

        public BaseResponseModel<UpdateBloodType> Update(UpdateBloodType model)
        {
            BaseResponseModel<UpdateBloodType> response = new BaseResponseModel<UpdateBloodType>();
            try
            {
                if (model.BloodTypes == null)
                {
                    response.Status = "1";
                    response.Message = "Blood type is required";
                    return response;
                }
                var data = _unitOfWork._db.BloodTypes.Where(x=>x.BloodTypeId == model.BloodTypeId).FirstOrDefault();
                if( data != null)
                {
                    bool exists = _unitOfWork._db.BloodTypes.Any(bt => bt.BloodTypes == model.BloodTypes && bt.BloodTypeId != model.BloodTypeId);

                    if (exists)
                    {
                        response.Status = "1";
                        response.Message = "Blood Type is already exist";
                        return response;
                    }
                    var actionUser = _unitOfWork.ActionUser;

                    data.BloodTypes= model.BloodTypes;
                    data.UpdatedBy = actionUser.UserName;
                    data.UpdatedDate = DateTime.Now;

                    _unitOfWork._db.BloodTypes.Update(data);
                    _unitOfWork._db.SaveChanges();

                    response.Status = "0";
                    response.Message = "Updated";
                    return response;
                }
                else
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }
                
            }
            catch(Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }
        public BaseResponseModel<UpdateBloodType> BloodTypeGetById(long BloodTypeId)
        {
            BaseResponseModel<UpdateBloodType> response = new BaseResponseModel<UpdateBloodType>();
            try
            {
                var data = _unitOfWork._db.BloodTypes.Where(x=>x.BloodTypeId ==BloodTypeId).FirstOrDefault();
                if (data == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }
                else
                {
                    var blood = new UpdateBloodType
                    {
                        BloodTypeId = data.BloodTypeId,
                        BloodTypes = data.BloodTypes
                    };
                    response.Status = "0";
                    response.Message = "Getting data";
                    response.Data = blood;
                    return response;
                }
            }
            catch(Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }
        public BaseResponseModel<string> Delete(long BloodTypeId)
        {
            BaseResponseModel<string> response= new BaseResponseModel<string>();
            try
            {
                var data = _unitOfWork._db.BloodTypes.Where(x=>x.BloodTypeId==BloodTypeId).FirstOrDefault();
                if(data == null)
                {
                    response.Status = "1";
                    response.Message = "No data found";
                    return response;
                }
                else
                {
                    _unitOfWork._db.BloodTypes.Remove(data);
                    _unitOfWork._db.SaveChanges();

                    response.Status = "0";
                    response.Message = "Deleted";
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
    }
    public interface IBloodTypeServices
    {
        BaseResponseModel<IEnumerable<BloodTypeViewModel>> AllBloodType();
        BaseResponseModel<CreateBloodType> Create(CreateBloodType model);
        BaseResponseModel<UpdateBloodType> Update(UpdateBloodType model);
        BaseResponseModel<UpdateBloodType> BloodTypeGetById(long BloodTypeId);
        BaseResponseModel<string> Delete(long BloodTypeId);
    }
}

using Common;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Services;

public class BloodStockService : IBloodStockService
{
    private readonly IUnitOfWork _unitOfWork;
    public BloodStockService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public BaseResponseModel<List<BloodStockViewModel>> AllStockList()
    {
        var response = new BaseResponseModel<List<BloodStockViewModel>>();
        try
        {
            var stock = _unitOfWork._db.BloodStocks.Include(x => x.BLOODTYPES).ToList();
            if (stock.Count == 0 || stock == null)
            {
                response.Status = "00";
                response.Data = null;;
                response.Message = "NO STOCK AVAILABLE";
                return response;
            }

            var list = stock.Select(x => new BloodStockViewModel()
            {
                Status = (x.ReserveQuantity ?? 0) > 0 ? "In Stock" : "Out Of Stock",
                InHold = x.InHold,
                BloodTypeId = x.BloodTypeId,
                BloodTypeName = x.BLOODTYPES.BloodTypes,
                BloodSockId = x.BloodStockId,
                Quantity = Convert.ToInt64(x.ReserveQuantity)

            }).ToList();

            response.Status = "00";
            response.Data = list;
            response.Message = "List Featched Successfully";
            return response;
        }
        catch (Exception e)
        {
            response.Status = "1";
            response.Message = e.Message;
            return  response;
        }
    }

    public BaseResponseModel<string> AddStock(AddBloodStockViewModel model)
    {
        BaseResponseModel<string> response = new BaseResponseModel<string>();
        try
        {
            if (_unitOfWork._db.BloodStocks.Any(x => x.BloodStockId == model.BloodTypeId))
            {
                response.Status = "1";
                response.Data = "BLOOD TYPE ALREADY EXISTS";
                return response;
            }

            if (model.BloodTypeId == 0)
            {
                response.Status = "1";
                response.Message = "BLOOD TYPE ID IS REQUIRED";
                return response;
            }

            if (model.Quantity == 0)
            {
                response.Status = "1";
                response.Message = "Quantity IS NULL";
                return response;
            }

            var stock = new BloodStock()
            {
                ReserveQuantity = model.Quantity,
                BloodTypeId = model.BloodTypeId,
                LastUpdated = DateTime.Now,
                InHold = 0
            };
            
            _unitOfWork._db.BloodStocks.Add(stock);
            _unitOfWork._db.SaveChanges();
            
            response.Status = "00";
            response.Data = "BLOOD STOCK ADDED";
            return response;
        }
        catch (Exception e)
        {
            response.Status = "1";
            response.Message = e.Message;
            return response;
        }
    }

    public BaseResponseModel<UpdateBloodStockViewModel> GetBloodStockById(long bloodStockId)
    {
        var response = new BaseResponseModel<UpdateBloodStockViewModel>();
        try
        {
var data = _unitOfWork._db.BloodStocks.FirstOrDefault(x => x.BloodStockId == bloodStockId);
if (data == null)
{
    response.Status = "00";
    response.Message = "NO STOCK AVAILABLE";
    return response;
}

response.Data = new UpdateBloodStockViewModel()
{
    BloodStockId = data.BloodStockId,
    Quantity = Convert.ToInt64(data.ReserveQuantity),
    BloodTypeId = data.BloodTypeId,
};
response.Status = "00";
response.Message ="DATA FETCHED SUCCESSFULLY!!!";
return response;
    
        }
        catch (Exception ex)
        {
            response.Status = "1";
            response.Message = ex.Message;
            return response;
        }
    }

    public BaseResponseModel<UpdateBloodStockViewModel> UpdateBloodStock(UpdateBloodStockViewModel model)
    {
        var response = new BaseResponseModel<UpdateBloodStockViewModel>();
        try
        {
            var data =_unitOfWork._db.BloodStocks.FirstOrDefault(x => x.BloodStockId == model.BloodStockId);
            if (data == null)
            {
                response.Status = "00";
                response.Message = "NO STOCK AVAILABLE";
                return response;
            }
            data.ReserveQuantity = model.Quantity;
            _unitOfWork._db.BloodStocks.Update(data);
            _unitOfWork._db.SaveChanges();
            
            response.Status = "00";
            response.Message = "BLOOD STOCK UPDATED";
            return response;
        }catch(Exception e)
        {
            response.Status = "1";
            response.Message = e.Message;
            return response;
        }
    }
}

public interface IBloodStockService
{
    BaseResponseModel<List<BloodStockViewModel>> AllStockList();
    BaseResponseModel<string> AddStock(AddBloodStockViewModel model);
    BaseResponseModel<UpdateBloodStockViewModel> GetBloodStockById(long bloodStockId);
    BaseResponseModel<UpdateBloodStockViewModel> UpdateBloodStock(UpdateBloodStockViewModel model);
}
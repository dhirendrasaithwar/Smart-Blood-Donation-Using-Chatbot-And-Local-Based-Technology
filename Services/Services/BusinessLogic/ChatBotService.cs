using Common;
using Common;
using Repository;

namespace Services;

public class ChatBotService : IChatBotService
{
    private readonly IUnitOfWork _unitOfWork;
    public ChatBotService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public BaseResponseModel<List<string>> GetCategory()
    {
        
        BaseResponseModel<List<string>> response = new();
        try
        {
var categories = _unitOfWork._db.ChatBots.Select(x => x.Category).Distinct().ToList();
if (!categories.Any())
{
    response.Status = "1";
    response.Message = "No categories found";
    return response;
}
response.Data = categories;
response.Status = "00";
response.Message = "Success";
return response;
        }
        catch (Exception ex)
        {
            response.Status = "1";
            response.Message = ex.Message;
            return response;
        }
    }

    public BaseResponseModel<List<QuestionDto>> Questions(string category)
    {
        BaseResponseModel<List<QuestionDto>> response = new();
        try
        {

         var quesions = _unitOfWork._db.ChatBots.Where(x => x.Category.ToUpper() == category.ToUpper()).ToList();
         if (quesions.Any())
         {
             response.Data = quesions.Select(x => new QuestionDto()
             {
Id = x.Id,
Question = x.Name
             }).ToList();
         }
         
         response.Status = "00";
         response.Message = "Success";
         return response;
        }catch(Exception ex)
        {
            response.Status = "1";
            response.Message = ex.Message;
            return response;
        }
    }

    public BaseResponseModel<string> Answer(long id)
    {
        BaseResponseModel<string> response = new();
        try
        {
            var answer = _unitOfWork._db.ChatBots.Where(x => x.Id == id).FirstOrDefault();
            if (answer == null)
            {
                response.Data = "No answer found";
                response.Status = "1";
                response.Message = "No category found";
                return response;
            }
            response.Data = answer.Answer;
            response.Status = "00";
            response.Message = "Success";
            return response;
            
        }catch(Exception ex)
        {
            response.Status = "1";
            response.Message = ex.Message;
            return response;
        }
    }

    public BaseResponseModel<string> AddChabotQuestion(AddChatbotViewModel model)
    {
        BaseResponseModel<string> response = new();
        try
        {
            if (string.IsNullOrWhiteSpace(model.Category))
            {
                response.Status = "1";
                response.Message = "Please choose a category";
                return response;
            }

            if (string.IsNullOrWhiteSpace(model.Question))
            {
                response.Status = "1";
                response.Message = "Please enter a question";
                return response;
            }

            if (string.IsNullOrWhiteSpace(model.Answer))
            {
                response.Status = "1";
                response.Message = "Please enter a answer";
                return response;
            }

            _unitOfWork._db.ChatBots.Add(new ChatBot()
            {
Name = model.Question,
                Category = model.Category,
                    Answer =  model.Answer
            });
            _unitOfWork._db.SaveChanges();
            response.Status = "00";
            response.Message = "Success";
            return response;
        }catch(Exception ex)
        {
            response.Status = "1";
            response.Message = ex.Message;
            return response;
        }
    }
}

public interface IChatBotService
{
    BaseResponseModel<List<string>> GetCategory();
    BaseResponseModel<List<QuestionDto>> Questions(string category);
    BaseResponseModel<string> Answer(long id);
    BaseResponseModel<string> AddChabotQuestion(AddChatbotViewModel model);
}
using Common;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Services;

namespace Web.Controllers;

public class ChatBotController : Controller
{
    private readonly IChatBotService _chatBotService;

    public ChatBotController(IChatBotService chatBotService)
    {
        _chatBotService = chatBotService;
    }

    [HttpGet]
    public IActionResult GetCategory()
    {
        var response = _chatBotService.GetCategory();
        if (response.Status == "00")
        {
            return  Ok(response.Data);
        }
        return BadRequest(response);
    }

    [HttpGet]
    public IActionResult Questions(string category)
    {
        var response = _chatBotService.Questions(category);
        if (response.Status == "00")
        {
            return Ok(response.Data);
        }else
        {
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult Answer(long id)
    {
        var response = _chatBotService.Answer(id);
        if (response.Status == "00")
        {
            return Ok(new {answer =response.Data});
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult AddQuestion()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddQuestion(AddChatbotViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = _chatBotService.AddChabotQuestion(model);
            if (response.Status == "00")
            {
                ViewBag.message = response.Data;
                return View(model);
            }
            else
            {
                return View(model);
            }
        }
        else
        {
            ViewBag.message = "Please fill in all fields";
            return View(model);
        }
    }
    
}
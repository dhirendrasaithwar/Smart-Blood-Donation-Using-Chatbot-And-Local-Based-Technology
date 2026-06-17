using Common.ViewModel.SearchViewModel;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Web.Controllers;

public class DonarLocationController : Controller
{
    private readonly IDonarRequestService _donarRequestService;

    public DonarLocationController(IDonarRequestService donarRequestService)
    {
        _donarRequestService = donarRequestService;
    }
    public IActionResult Index(string message)
    {
        ViewBag.Message = message;

        return View();
    }

    [HttpGet]
    public IActionResult Search(string streetName, string cityName, long bloodGroupId, string message)
    {
        var response = _donarRequestService.SearchDonar(streetName, cityName, bloodGroupId);
        return Ok(response.Data);
    }

    [HttpPost]
    public IActionResult RequestDoner(RequestDonar model)
    {
        var response = _donarRequestService.RequestDoner(model);
        if (response.Status == "00")
        {
            return RedirectToAction("MyPersonalRequest");
        }

        return RedirectToAction("Index", new { message = response.Message });
    }

    [HttpGet]
    public IActionResult RequestList()
    {
        var response = _donarRequestService.DonationList();
        return View(response.Data);
    }

    [HttpGet]
    public IActionResult MyPersonalRequest()
    {
        var response = _donarRequestService.RequestList();
        return View(response.Data);
    }
    [HttpPost]
    public IActionResult Accept(RequestDonar donar)
    {
        var response = _donarRequestService.Accept(donar.Id);
        if (response.Status == "00")
        {
            return RedirectToAction("RequestList");
        }
        return RedirectToAction("RequestList");
    }

    [HttpPost]
    public IActionResult Reject(long id)
    {
        var response = _donarRequestService.Reject(id);
        if (response.Status == "00")
        {
            return RedirectToAction("RequestList");
        }
        return RedirectToAction("RequestList");
    }

}
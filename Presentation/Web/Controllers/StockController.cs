using System.Security.Claims;
using Common;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Web.Controllers;

public class StockController : Controller
{
    private readonly IBloodStockService _bloodStockService;
private readonly IHttpContextAccessor _httpContextAccessor;
    public StockController(IBloodStockService bloodStockService, IHttpContextAccessor httpContextAccessor)
    {
        _bloodStockService = bloodStockService;
        _httpContextAccessor = httpContextAccessor;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var roleType = _httpContextAccessor.HttpContext.User
            .FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        
        ViewBag.RoleType = roleType;
        return View(_bloodStockService.AllStockList().Data);
    }
    [HttpGet]
    public IActionResult AddStock()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddStock(AddBloodStockViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = _bloodStockService.AddStock(model);
            if (response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        else
        {
            return View(model);
        }
    }
    [HttpGet]
    public IActionResult EditStock(long stockId)
    {
        var response = _bloodStockService.GetBloodStockById(stockId);
        if (response.Status == "00")
        {
            return View(response.Data);
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult EditStock(UpdateBloodStockViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = _bloodStockService.UpdateBloodStock(model);
            if (response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        else
        {
            return View(model);
        }
    }
}
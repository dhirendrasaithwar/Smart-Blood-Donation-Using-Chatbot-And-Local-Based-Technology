using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Web.Controllers
{
    [Authorize]
    public class BloodRequestController : Controller
    {
        private readonly IBloodRequestServices _bloodRequest;

        public BloodRequestController(IBloodRequestServices bloodRequest)
        {
            _bloodRequest = bloodRequest;
        }

        public IActionResult Index()
        {
            var data = _bloodRequest.GetBloodRequestsByUser();
            if (data.Status == "00")
            {
                return View(data.Data);
            }
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AllBoodRequest(string message)
        {
            var data = _bloodRequest.GetBloodRequests();
            if (data.Status == "00")
            {
                if (!string.IsNullOrEmpty(message))
                {
                    ViewBag.message = message;
                }
                return View(data.Data);
            }
            return View();
        }
        [HttpGet]
        public IActionResult RequestBlood()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RequestBlood(CreateBloodRequest model)
        {
            var blood = _bloodRequest.Create(model);
            if (blood.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = blood.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(long BloodRequestId)
        {
            var data = _bloodRequest.GetBloodById(BloodRequestId);
            if (data.Status == "00")
            {
                return View(data.Data);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(UpdateBloodRequest model)
        {
            var data = _bloodRequest.Update(model);
            if(data.Status == "00")
            {
                
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = data.Message; 
            }
            return View();
        }
        public IActionResult Delete(long BloodRequestId)
        {
            var data = _bloodRequest.Delete(BloodRequestId);
            if (data.Status == "00")
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult RequestCompleted(long bloodRequestId)
        {
            var response = _bloodRequest.Completed(bloodRequestId);
            if (response.Status == "00")
            {
                return RedirectToAction("AllBoodRequest", new {message = response.Message});
            }
            else
            {
                return RedirectToAction("AllBoodRequest", new {message = response.Message});
            }
        } 
        [HttpGet]
        public IActionResult CannotManageBlood(long bloodRequestId)
        {
            var response = _bloodRequest.CannotManageBlood(bloodRequestId);
            if (response.Status == "00")
            {
                return RedirectToAction("AllBoodRequest", new {message = response.Message});
            }else
            {
                return RedirectToAction("AllBoodRequest", new {message = response.Message});
            }
        }
        
    }
}

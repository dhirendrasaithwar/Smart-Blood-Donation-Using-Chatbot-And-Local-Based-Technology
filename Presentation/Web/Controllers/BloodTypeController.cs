using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Web.Controllers
{
    [Authorize]
    public class BloodTypeController : Controller
    {
        private readonly IBloodTypeServices _bloodTypeServices;

        public BloodTypeController(IBloodTypeServices bloodTypeServices)
        {
            _bloodTypeServices = bloodTypeServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var data = _bloodTypeServices.AllBloodType();
            if (data.Status == "0")
            {
                var blood = data.Data;
                return View(blood);
            }
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateBloodType model)
        {
            var blood = _bloodTypeServices.Create(model);
            if (blood.Status == "0")
            {
                return RedirectToAction("Index","BloodType");
            }
            else
            {
                ViewBag.message = blood.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(long BloodTypeId)
        {
            var data = _bloodTypeServices.BloodTypeGetById(BloodTypeId);
            if (data.Status == "0")
            {
                return View(data.Data);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Edit(UpdateBloodType model)
        {
            var update = _bloodTypeServices.Update(model);
            if (update.Status == "0")
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = update.Message;
            }
            return View();
        }
        public IActionResult Delete(long BloodTypeId)
        {
            var data = _bloodTypeServices.Delete(BloodTypeId);
            if (data.Status == "0")
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}

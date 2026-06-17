using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Web
{
    public class DonationController : Controller
    {
        private readonly IDonationServices _donationServices;

        public DonationController(IDonationServices donationServices)
        {
            _donationServices = donationServices;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var data = _donationServices.GetDonationByUser();
            if (data.Status == "0")
            {
                return View(data.Data);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles ="SuperAdmin")]
        public IActionResult DonationList()
        {
            var data = _donationServices.DonationList();
            if (data.Status == "0")
            {
                return View(data.Data);
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var response = _donationServices.GetDonationPrevInfo();
            if(response.Status == "1")
                ViewBag.message = response.Message;
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateDonation model)
        {
            var data = _donationServices.Create(model);
            if (data.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = data.Message;
            }
            return View();
        }
        [Authorize]
        public IActionResult Delete(long DonationID)
        {
            _donationServices.Delete(DonationID);
            return RedirectToAction("Index");
        }
    }
}

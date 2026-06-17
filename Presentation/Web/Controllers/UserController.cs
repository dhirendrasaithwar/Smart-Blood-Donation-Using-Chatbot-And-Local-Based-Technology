using Common;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Web
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _userServices.Login(login);
            if(user.Status == "00")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.message = user.Message;
                return View();
            }
            
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Register register)
        {
            var user = _userServices.Register(register);
            if(user.Status == "00")
            {
                return RedirectToAction("Login","User");
            }
            else
            {
                ViewBag.message = user.Message;
            }
            return View();
        }
        public IActionResult Logout()
        {
            _userServices.Logout();
            return RedirectToAction("Login","User");
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(ForgotPassword model)
        {
            var data = _userServices.ForgotPassword(model);
            if (data.Status == "0")
            {
                ModelState.Clear();
                ViewBag.message = data.Message;
                return View();
            }
            else
            {
                ViewBag.message = data.Message;
            }
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string Token, long UserId)
        {
            var model = new ResetPassword
            {
                UserId = UserId,
                ResetToken = Token
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPassword model)
        {
            var data = _userServices.ResetPassword(model);
            if (data.Status == "0")
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.message = data.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public  IActionResult ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var response = _userServices.ChangePassword(model);
                if (response.Status == "00")
                {
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ViewBag.message = response.Message;
                    return View(model);
                }                
            }
            else
            {
                ViewBag.message = "PLEASE FILL THE FORM PROPERLY!!!";
                return View(model);
            }

        }
    }
}

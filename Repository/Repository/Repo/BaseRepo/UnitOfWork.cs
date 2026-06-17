using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DBContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = new UserRepository(db);
            _roleRepository = new RoleRepository(db);
            //_webHostEnvironment = webHostEnvironment;
            _bloodRequestRepository= new BloodRequestRepository(db);
            _bloodTypeRepository= new BloodTypeRepository(db);
            _donationRepository= new DonationRepository(db);

            var userAgent = httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
            if (!string.IsNullOrEmpty(userAgent))
                UserAgent = Convert.ToString(userAgent[0]);
            StringValues ipAddress;
            if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("ClientIP", out ipAddress))
            {
                IpAddress = ipAddress;
            }
            else
            {
                IpAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            string UserName = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            ActionUser = _db.Users.Include("USERROLE").FirstOrDefault(x => x.UserName == UserName);
        }


        public User ActionUser { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; } //Browser
        public DBContext _db { get; private set; }
        public IHttpContextAccessor _httpContextAccessor { get; private set; }
        //public IWebHostEnvironment _webHostEnvironment { get; private set; }
        public IUserRepository _userRepository { get; private set; }
        public IRoleRepository _roleRepository { get; private set; }
        public IBloodRequestRepository _bloodRequestRepository { get; private set; }
        public IBloodTypeRepository _bloodTypeRepository { get; private set; }
        public IDonationRepository _donationRepository { get; private set; }

    }
}

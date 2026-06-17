using Microsoft.AspNetCore.Http;

namespace Repository
{
    public interface IUnitOfWork
    {
        public DBContext _db { get; }
        public User ActionUser { get; }
        public string IpAddress { get; }
        public string UserAgent { get; } //Browser
        IHttpContextAccessor _httpContextAccessor { get; }
        //IWebHostEnvironment _webHostEnvironment { get; }
        IUserRepository _userRepository { get; }
        IRoleRepository _roleRepository { get; }
        IBloodRequestRepository _bloodRequestRepository { get; }
        IBloodTypeRepository _bloodTypeRepository { get; }
        IDonationRepository _donationRepository { get; }
    }
}

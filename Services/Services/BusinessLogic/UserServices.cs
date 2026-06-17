using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Web.Helpers;

namespace Services
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
public async Task<BaseResponseModel<UserClaimViewModel>> Login(Login login)
{
    BaseResponseModel<UserClaimViewModel> response = new BaseResponseModel<UserClaimViewModel>();

    try
    {
        var user = _unitOfWork._db.Users
            .Include("USERROLE.ROLE")
            .FirstOrDefault(x => x.Email == login.Email);

        if (user == null)
        {
            response.Status = "1";
            response.Message = "Error!! User Not Found";
            return response;
        }

        if (!StaticMethods.VerifyPassword(login.Password, user.Password))
        {
            response.Status = "1";
            response.Message = "Username or password doesn't match";
            return response;
        }

        if (user.Status.ToUpper() != "ACTIVE")
        {
            response.Status = "1";
            response.Message = "Please verify your email first";
            return response;
        }

        UserClaimViewModel claim = new UserClaimViewModel
        {
            UserId = user.UserId,
            UserName = user.UserName,
            BloodTypeId = user.BloodTypeId,
            Fullname = user.FullName,
            Email = user.Email,
            Roles = user.USERROLE.Select(x => x.ROLE.RoleType).ToList()
        };

        ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
        identity.AddClaim(new Claim("Email", user.Email));

        foreach (var role in user.USERROLE.Select(x => x.ROLE))
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role.RoleType));
        }

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
        };

        await _unitOfWork._httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            authProperties
        );

        response.Status = "00";
        response.Message = "LOGIN SUCCESS!!!";
        response.Data = claim;

        return response;
    }
    catch
    {
        response.Status = "1";
        response.Message = "Technical Error occur";
        return response;
    }
}
        public BaseResponseModel<Register> Register(Register register)
        {
            BaseResponseModel<Register> response = new BaseResponseModel<Register>();
            try
            {
                var data = _unitOfWork._db.Users.Where(x => x.Email == register.Email).FirstOrDefault();
                if (data != null)
                {
                    response.Status = "1";
                    response.Message = "Email Already exist";
                    return response;
                }
                if (data?.PhoneNumber == register.PhoneNumber)
                {
                    response.Status = "1";
                    response.Message = "Phone Already exist";
                    return response;
                }
                if (data?.UserName == register.UserName)
                {
                    response.Status = "1";
                    response.Message = "UserName Already exist";
                    return response;
                }
                bool previousDonation = false;
                if (register.IsPreviousDonate.ToLower() == "yes")
                {
                    previousDonation = true;
                }
                else
                {
                    previousDonation = false;
                }

                if (string.IsNullOrWhiteSpace(register.CityName))
                {
                    response.Status = "1";
                    response.Message = "Please enter City Name";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(register.StreetName))
                {
                    response.Status = "1";
                    response.Message = "Please enter Street Name";
                    return response;
                }
                var bloodtype = _unitOfWork._db.BloodTypes.Where(x => x.BloodTypeId == register.BloodTypeId).FirstOrDefault();
                var actionuser = _unitOfWork.ActionUser;
                var user = new User
                {
                    UserName = register.UserName,
                    FullName = register.FullName,
                    Email = register.Email,
                    PhoneNumber = register.PhoneNumber,
                    Address = register.Address,
                    Status = "Active",
                    DateOfBirth = register.DateOfBirth,
                    Password = StaticMethods.HashPassword(register.Password).Item2,
                    BloodTypeId = register.BloodTypeId,
                    BloodType = bloodtype.BloodTypes,
                    Gender = register.Gender,
                    StreetAddress =  register.StreetName,
                    CityName = register.CityName,
                    IsPreviousDonate = previousDonation,
                    LastDonationDate = register.LastDonationDate,
                    IsUserVerify = false,
                    CreatedBy = "Self",
                    CreatedDate = DateTime.Now,
                };
                _unitOfWork._db.Users.Add(user);
                _unitOfWork._db.SaveChanges();
                var role = _unitOfWork._db.Roles.Where(x => x.RoleType == "NormalUser").FirstOrDefault();
                if (role == null)
                {
                    var roles = new Role
                    {
                        RoleType = "NormalUser",
                        CreatedBy = "System",
                        CreatedDate = DateTime.Now,
                    };
                    _unitOfWork._db.Roles.Add(roles);
                    _unitOfWork._db.SaveChanges();

                    var userrole = new UserRole
                    {
                        UserId = user.UserId,
                        RoleId = roles.RoleId,
                        CreatedBy = "System",
                        CreatedDate = DateTime.Now,
                    };

                    _unitOfWork._db.UserRoles.Add(userrole);
                    _unitOfWork._db.SaveChanges();
                }
                else
                {
                    var userrole = new UserRole
                    {
                        UserId = user.UserId,
                        RoleId = role.RoleId,
                        CreatedBy = "System",
                        CreatedDate = DateTime.Now,
                    };

                    _unitOfWork._db.UserRoles.Add(userrole);
                    _unitOfWork._db.SaveChanges();
                }

                response.Status = "00";
                response.Message = "Register successfully..";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical Error occur";
                return response;
            }
        }
        public void Logout()
        {
            _unitOfWork._httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        public BaseResponseModel<UserAndBloodCount> Count()
        {
            BaseResponseModel<UserAndBloodCount> response = new BaseResponseModel<UserAndBloodCount>();
            try
            {
                var actionUser = _unitOfWork.ActionUser;

                var TotalUser = _unitOfWork._db.Users.Count();
                var TotalBloodRequestCount = _unitOfWork._db.BloodRequests.Count();
                var PendingBloodRequest = _unitOfWork._db.BloodRequests.Where(x => x.Status.ToLower() == "pending").Count();
                var CompletedBloodRequest = _unitOfWork._db.BloodRequests.Where(x => x.Status.ToLower() == "completed").Count();
                var NotManagedBloodRequest = _unitOfWork._db.BloodRequests.Where(x => x.Status.ToLower() == "notmanaged").Count();

                var UserTotalBloodRequest = _unitOfWork._db.BloodRequests.Where(x => x.UserId == actionUser.UserId).Count();
                var UserPendingBloodRequest = _unitOfWork._db.BloodRequests.Where(x => x.UserId == actionUser.UserId && x.Status.ToLower() == "pending").Count();
                var UserCompletedBloodRequest = _unitOfWork._db.BloodRequests.Where(x => x.UserId == actionUser.UserId && x.Status.ToLower() == "completed").Count();
                var UserNotManagedBloodRequest = _unitOfWork._db.BloodRequests.Where(x => x.UserId == actionUser.UserId && x.Status.ToLower() == "notmanaged").Count();
                var reserveBloodRequest = _unitOfWork._db.BloodRequests.Where(x => x.Status.ToLower() == "reserve").Count();

                var counts = new UserAndBloodCount
                {
                    TotalUser = TotalUser,
                    TotalBloodRequest = TotalBloodRequestCount,
                    PendingRequest = PendingBloodRequest,
                    CompletedRequest= CompletedBloodRequest,
                    NotManagedRequest= NotManagedBloodRequest,
                    Reserve =  reserveBloodRequest,
                    TotalBloodRequestOfUser = UserTotalBloodRequest,
                    PendingRequestOfUser = UserPendingBloodRequest,
                    CompletedRequestOfUser= UserCompletedBloodRequest,
                    NotManagedRequestOfUser= UserNotManagedBloodRequest,
                };
                var tuple = new List<Tuple<string, long, long>>();
                var stockCount = _unitOfWork._db.BloodStocks.Include(x => x.BLOODTYPES).ToList();
                if (stockCount != null && stockCount.Count > 0)
                {
                    foreach (var stock in stockCount)
                    {
                        counts.BloodStockInfo.Add(
                            new Tuple<string, long, long?>(
                                stock.BLOODTYPES.BloodTypes,
                                stock.InHold,
                                stock.ReserveQuantity 
                            )
                        );
                    }


                }
                response.Status = "00";
                response.Message = "Success";
                response.Data = counts;
                return response;

            }
            catch (Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }
        //forget password start
        public void SendPasswordResetEmail(string userEmail, string resetLink, long UserID)
        {
            // Configure the SMTP client
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("dhirendrasaithwar@gmail.com", "ffljjeecvmflkzmc"),
                EnableSsl = true,
            };
            resetLink = GenerateResetLink(resetLink, UserID);
            // Create the email message
            var message = new MailMessage
            {
                From = new MailAddress("dhirendrasaithwar@gmail.com"),
                Subject = "Reset Your Password",
                IsBodyHtml = true
            };
            var htmlBody = $@"
            <div style='text-align: center'>
                <h2>Reset Your Password</h2>
                <p>Click the button to reset your password.</p>
                <br />
                <table border='0' cellpadding='0' cellspacing='0' style='width: 100%'>
                    <tr>
                        <td align='center' bgcolor='#fff'>
                            <a href='{resetLink}' style='display: inline-block; padding: 12px 20px; font-size: 14px; color: #ffffff; text-decoration: none; border-radius: 4px; background-color: red;'>Reset Your Password</a>
                        </td>
                    </tr>
                </table>
                <br />
                <p>Please do not reply to this message. Replies to this message are routed to an unmonitored mailbox. If you have questions please go to our website. You may also call us at +977-9800000000. </p>
                <h2>Welcome to Blood Donation System</h2>
                <h5>The Blood Donation System Team</h5>
<h5>Email: dhirendrasaithwar@gmail.com</h5>
            </div>";
            message.Body = htmlBody;
            message.To.Add(userEmail);

            // Send the email
            smtpClient.Send(message);
        }

        public BaseResponseModel<string> ForgotPassword(ForgotPassword forgotPassword)
        {
            BaseResponseModel<string> response = new BaseResponseModel<string>();
            try
            {
                var user = _unitOfWork._db.Users.Where(x=>x.Email== forgotPassword.Email).FirstOrDefault();
                if (user != null)
                {
                    string resetToken = Guid.NewGuid().ToString();
                    user.ResetToken = resetToken;
                    user.ResetTokenExpiration = DateTime.Now.AddHours(24);

                    _unitOfWork._db.Users.Update(user);
                    _unitOfWork._db.SaveChanges();
                    SendPasswordResetEmail(user.Email, resetToken, user.UserId);

                    response.Status = "0";
                    response.Message = "Reset link sent to your email";
                    return response;
                }
                else
                {
                    response.Status = "1";
                    response.Message = "User doesnot exist";
                    return response;
                }
            }
            catch(Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }

        private string GenerateResetLink(string resetToken, long UserID)
        {
            var scheme = _unitOfWork._httpContextAccessor.HttpContext.Request.Scheme;
            var host = _unitOfWork._httpContextAccessor.HttpContext.Request.Host.Value;

            string resetLink = $"{scheme}://{host}/User/resetpassword?token={resetToken}&userID={UserID}";

            return resetLink;
        }

        public BaseResponseModel<string> ResetPassword(ResetPassword resetPassword)
        {
            BaseResponseModel<string> response = new BaseResponseModel<string>();
            try
            {
                var user = _unitOfWork._db.Users.Where(x => x.ResetToken == resetPassword.ResetToken && x.ResetTokenExpiration > DateTime.Now).FirstOrDefault();
                if (user == null)
                {
                    response.Status = "1";
                    response.Message = "Invalid or expire reset token";
                    return response;
                }

                user.Password = StaticMethods.HashPassword(resetPassword.Password).Item2;
                user.ResetToken = null;
                user.ResetTokenExpiration = null;

                _unitOfWork._db.Users.Update(user);
                _unitOfWork._db.SaveChanges();

                response.Status = "0";
                response.Message = "Password change successfully done...";
                return response;

            }
            catch (Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }

        public BaseResponseModel<string> ChangePassword(ChangePassword changePassword)
        {
            BaseResponseModel<string> response = new BaseResponseModel<string>();
            try
            {
                var actionUser = _unitOfWork.ActionUser;
                var user = _unitOfWork._db.Users.FirstOrDefault(x => x.UserId == actionUser.UserId);
                if (user == null)
                {
                    response.Status = "1";
                    response.Message = "User doesnot exist";
                    return response;
                }

                if (!StaticMethods.VerifyPassword(changePassword.CurrentPassword, user.Password))
                {
                    response.Status = "1";
                    response.Message = "Invalid password";
                    return response;
                }
                changePassword.NewPassword = StaticMethods.HashPassword(changePassword.NewPassword).Item2;
                user.Password = changePassword.NewPassword;
                _unitOfWork._db.Users.Update(user);
                _unitOfWork._db.SaveChanges();
                
                response.Status = "00";
                response.Message = "Password changed successfully";
                return response;
                
            }catch(Exception ex)
            {
                response.Status = "1";
                response.Message = "Technical error occur";
                return response;
            }
        }
    }
    public interface IUserServices
    {
        Task<BaseResponseModel<UserClaimViewModel>> Login(Login login);
        BaseResponseModel<Register> Register(Register register);
        void Logout();
        BaseResponseModel<UserAndBloodCount> Count();
        BaseResponseModel<string> ForgotPassword(ForgotPassword forgotPassword);
        BaseResponseModel<string> ResetPassword(ResetPassword resetPassword);
        BaseResponseModel<string> ChangePassword(ChangePassword changePassword);
    }
}

using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace Service
{
    public static class DbInitializer
    {
        public static async Task Initialize(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                DBContext context = serviceScope.ServiceProvider.GetService<DBContext>();
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    context.Database.EnsureCreated();
                }

                try
                {
                    if(!context.BloodTypes.Any())
                    {
                        List<string> bloodTypes = new List<string>();
                        bloodTypes.Add("A+");
                        bloodTypes.Add("A-");
                        bloodTypes.Add("B+");
                        bloodTypes.Add("B-");
                        bloodTypes.Add("AB+");
                        bloodTypes.Add("AB-");
                        bloodTypes.Add("O+");
                        bloodTypes.Add("O-");
                        foreach (var bloodType in bloodTypes)
                        {
                            var blood = new BloodType
                            {
                                BloodTypes = bloodType,
                                CreatedBy = "SuperAdmin",
                                CreatedDate = DateTime.Now,
                            };
                            context.BloodTypes.Add(blood);
                            context.SaveChanges();                            
                        }

                    }
                    if (context.Users.FirstOrDefault(x => x.UserName == "SuperAdmin") == null)
                    {
                        var password = StaticMethods.HashPassword("admin123");
                        var bloodtype = context.BloodTypes.FirstOrDefault(x => x.BloodTypes.ToUpper() == "O+");
                        var DOB = "2004-04-19";
                        DateTime date = DateTime.ParseExact(DOB, "yyyy-MM-dd", null);
                        var user = new User
                        {
                            UserName = "SuperAdmin",
                            FullName = "Dhirendra Saithwar",
                            Password = password.Item2,
                            Address = "Y031 7QY",
                            PhoneNumber = "07438186155",
                            Email = "dhirendrasaithwar@gmail.com",
                            Gender = "Male",
                            DateOfBirth= date,
                            IsUserVerify= true,
                            BloodTypeId = bloodtype.BloodTypeId,
                            CityName = "York",
                            StreetAddress = "Lockwood",
                            BloodType = bloodtype.BloodTypes,
                            Status = "ACTIVE",
                            CreatedBy = "SuperAdmin",
                            CreatedDate = DateTime.Now,
                        };
                        context.Users.Add(user);
                        context.SaveChanges();
                    }
                    if (context.Roles.FirstOrDefault(x => x.RoleType == "SuperAdmin") == null)
                    {
                        var role = new Role
                        {
                            RoleType = "SuperAdmin",
                            CreatedBy = "SuperAdmin",
                            CreatedDate = DateTime.Now,
                        };
                        context.Roles.Add(role);
                        context.SaveChanges();
                    }
                    if (!context.UserRoles.Any(x => x.USER.UserName == "SuperAdmin" && x.ROLE.RoleType== "SuperAdmin"))
                    {
                        var user = context.Users.FirstOrDefault(x => x.UserName == "SuperAdmin");
                        var role = context.Roles.FirstOrDefault(x => x.RoleType == "SuperAdmin");

                        if (user != null && role != null)
                        {
                            var userRole = new UserRole()
                            {
                                RoleId = role.RoleId,
                                UserId = user.UserId,
                                CreatedBy = "SuperAdmin",
                                CreatedDate = DateTime.Now,
                            };
                            context.UserRoles.Add(userRole);
                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }


        }
    }
}

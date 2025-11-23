using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presistence.IdentityData.DataSeed
{
    public class IdentityDataIntializer : IDataSeed
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public IdentityDataIntializer(UserManager<ApplicationUser> userManager 
            , RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task InitializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var user1 = new ApplicationUser
                    {
                        DisplayName="Morad Nasr",
                        UserName="MoradNasr",
                        Email="MoradNasr@gmail.com",
                        PhoneNumber="01220818777",
                    };
                    var user2 = new ApplicationUser
                    {
                        DisplayName="Sophie Peter",
                        UserName="SophiePeter",
                        Email="SophiePeter@gmail.com",
                        PhoneNumber="01220818666",
                    };


                   await _userManager.CreateAsync(user1, "P@ssw0rd");
                   await _userManager.CreateAsync(user2, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(user1, "SuperAdmin");
                    await _userManager.AddToRoleAsync(user2, "Admin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While Seeding Database {ex} Happend");
            }
        }
    }
}

using JadeshGeneral.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtmosERP.Core.Data
{
    public static class SeedData
    {
        public static void Seed(UserManager<ApplicationUser> usermanager, RoleManager<IdentityRole> rolemanager, ApplicationDbContext context)
        {
            SeedRoles(rolemanager);
            SeedUsers(usermanager);
        }
        private static void SeedRoles(RoleManager<IdentityRole> rolemanager)
        {
            if (!rolemanager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Admin",

                };
                var result = rolemanager.CreateAsync(role).Result;
            }

            if (!rolemanager.RoleExistsAsync("Customer").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Customer",

                };
                var result = rolemanager.CreateAsync(role).Result;
            }
        }
        private static void SeedUsers(UserManager<ApplicationUser> usermanager)
        {
            if (usermanager.FindByNameAsync("admin@jadesh.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@jadesh.com",
                    Email = "admin@jadesh.com",
                    FirstName = "Jadesh",
                    LastName = "Admin",
                    PhoneNumber = "08065964773",
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    DateCreated = DateTime.Now

                };
                var result = usermanager.CreateAsync(user, "Password@1").Result;

                if (result.Succeeded)
                {
                    usermanager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

        }
    }
}

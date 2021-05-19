using Microsoft.AspNetCore.Identity;
using ELearn.Data.Context;
using ELearn.Data.Models;
using ELearn.Repo.Infrastructure;
using ELearn.Services.Seed.Interface;
using ELearn.Services.Site.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELearn.Services.Seed.Service
{
    public class SeedService : ISeedService
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public SeedService(IUnitOfWork<DatabaseContext> db, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedCategories()
        {
            if (!_db.CategoryRepository.Get(c => c.Parent == null, null, "Parent").Any())
            {
                var categories = new List<Category>()
                {
                    new Category(){Name = "دانشگاهی و تخصصی"},
                    new Category(){Name = "برنامه نویسی"},
                    new Category(){Name = "نرم افزار های کاربردی"},
                    new Category(){Name = "علوم پایه"}
                };

                foreach (var category in categories)
                {
                    _db.CategoryRepository.Add(category);
                }
                _db.Save();
            }
        }

        public void SeedRoles()
        {
            if (!_roleManager.Roles.Any())
            {
                var roles = new List<Role>()
                {
                    new Role() {Name = "System"},
                    new Role() {Name = "Admin"},
                    new Role() {Name = "Teacher"},
                    new Role() {Name = "Student"},
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }
            }
        }

        //public void SeedSetting()
        //{
        //    if (!_db.SettingRepository.Get().Any())
        //    {
        //        var setting = new Setting();
        //        _db.SettingRepository.Add(setting);
        //        _db.Save();
        //    }
        //}

        public void SeedUsers()
        {
            if (!_userManager.Users.Any(u => u.NormalizedUserName == "ADMIN@ADMIN"))
            {
                // Admin
                var adminUser = new User()
                {
                    UserName = "09130001111",
                    FirstName = "مدیر وبسایت",
                    LastName = "مدیر وبسایت",
                    NationalCode = "1111111111",
                    PhotoUrl = "/images/site/default.png",
                    RegisterDate = DateTime.Now,
                    Status = true
                };
                var adminResult = _userManager.CreateAsync(adminUser, "1234").Result;
                if (adminResult.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("09130001111").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "System" }).Wait();
                }
            }
        }
    }
}

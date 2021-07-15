using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Services.Seed.Interface
{
    public interface ISeedService
    {
        void SeedRoles();
        void SeedUsers();
        void SeedCategories();
        void SeedSetting();
    }
}

using ELearn.Repo.Infrastructure;
using ELearn.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using ELearn.Data.Context;
using ELearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Repo.Repositories.Repo
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DbContext _db;

        public UserRepository(DbContext db) : base(db)
        {
            _db = (_db ?? (DatabaseContext)_db);
        }

        public bool UserExists(string username)
        {
            var temp = username.ToUpper();
            if (Get(u => u.NormalizedUserName == temp, string.Empty) != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            var temp = username.ToUpper();
            if (await GetAsync(u => u.NormalizedUserName == temp, string.Empty) != null)
            {
                return true;
            }

            return false;
        }
    }
}

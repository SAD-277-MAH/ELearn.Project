using ELearn.Repo.Infrastructure;
using ELearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Repo.Repositories.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        bool UserExists(string username);
        Task<bool> UserExistsAsync(string username);
    }
}

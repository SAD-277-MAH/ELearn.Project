using ELearn.Data.Context;
using ELearn.Data.Models;
using ELearn.Repo.Infrastructure;
using ELearn.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearn.Repo.Repositories.Repo
{
    public class ExamRepository : Repository<Exam>, IExamRepository
    {
        private readonly DbContext _db;

        public ExamRepository(DbContext db) : base(db)
        {
            _db = _db ?? (DatabaseContext)_db;
        }
    }
}

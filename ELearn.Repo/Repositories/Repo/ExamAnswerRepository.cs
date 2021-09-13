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
    public class ExamAnswerRepository : Repository<ExamAnswer>, IExamAnswerRepository
    {
        private readonly DbContext _db;

        public ExamAnswerRepository(DbContext db) : base(db)
        {
            _db = _db ?? (DatabaseContext)_db;
        }
    }
}

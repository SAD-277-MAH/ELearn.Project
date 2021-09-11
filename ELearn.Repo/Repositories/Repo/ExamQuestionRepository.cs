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
    public class ExamQuestionRepository : Repository<ExamQuestion>, IExamQuestionRepository
    {
        private readonly DbContext _db;

        public ExamQuestionRepository(DbContext db) : base(db)
        {
            _db = _db ?? (DatabaseContext)_db;
        }
    }
}

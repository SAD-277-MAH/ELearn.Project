using ELearn.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Repo.Infrastructure
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        ITokenRepository TokenRepository { get; }
        ISettingRepository SettingRepository { get; }
        IStudentRepository StudentRepository { get; }
        ITeacherRepository TeacherRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICourseRepository CourseRepository { get; }
        ISessionRepository SessionRepository { get; }
        IUserCourseRepository UserCourseRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        ICommentRepository CommentRepository { get; }
        IDocumentRepository DocumentRepository { get; }
        IExamRepository ExamRepository { get; }
        IExamQuestionRepository ExamQuestionRepository { get; }
        IExamAnswerRepository ExamAnswerRepository { get; }

        bool Save();
        Task<bool> SaveAsync();
    }
}

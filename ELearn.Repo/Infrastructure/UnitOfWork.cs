using Microsoft.EntityFrameworkCore;
using ELearn.Repo.Repositories.Interface;
using ELearn.Repo.Repositories.Repo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Repo.Infrastructure
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        #region Constructor
        protected readonly DbContext _db;

        public UnitOfWork()
        {
            _db = new TContext();
        }
        #endregion

        #region Repository
        private IUserRepository userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(_db);
                }
                return userRepository;
            }
        }

        private IRoleRepository roleRepository;
        public IRoleRepository RoleRepository
        {
            get
            {
                if (roleRepository == null)
                {
                    roleRepository = new RoleRepository(_db);
                }
                return roleRepository;
            }
        }

        private ITokenRepository tokenRepository;
        public ITokenRepository TokenRepository
        {
            get
            {
                if (tokenRepository == null)
                {
                    tokenRepository = new TokenRepository(_db);
                }
                return tokenRepository;
            }
        }

        private ISettingRepository settingRepository;
        public ISettingRepository SettingRepository
        {
            get
            {
                if (settingRepository == null)
                {
                    settingRepository = new SettingRepository(_db);
                }
                return settingRepository;
            }
        }

        private IStudentRepository studentRepository;
        public IStudentRepository StudentRepository
        {
            get
            {
                if (studentRepository == null)
                {
                    studentRepository = new StudentRepository(_db);
                }
                return studentRepository;
            }
        }

        private ITeacherRepository teacherRepository;
        public ITeacherRepository TeacherRepository
        {
            get
            {
                if (teacherRepository == null)
                {
                    teacherRepository = new TeacherRepository(_db);
                }
                return teacherRepository;
            }
        }

        private ICategoryRepository categoryRepository;
        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (categoryRepository == null)
                {
                    categoryRepository = new CategoryRepository(_db);
                }
                return categoryRepository;
            }
        }

        private ICourseRepository courseRepository;
        public ICourseRepository CourseRepository
        {
            get
            {
                if (courseRepository == null)
                {
                    courseRepository = new CourseRepository(_db);
                }
                return courseRepository;
            }
        }

        private ISessionRepository sessionRepository;
        public ISessionRepository SessionRepository
        {
            get
            {
                if (sessionRepository == null)
                {
                    sessionRepository = new SessionRepository(_db);
                }
                return sessionRepository;
            }
        }

        private IUserCourseRepository userCourseRepository;
        public IUserCourseRepository UserCourseRepository
        {
            get
            {
                if (userCourseRepository == null)
                {
                    userCourseRepository = new UserCourseRepository(_db);
                }
                return userCourseRepository;
            }
        }

        private IOrderRepository orderRepository;
        public IOrderRepository OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new OrderRepository(_db);
                }
                return orderRepository;
            }
        }

        private IOrderDetailRepository orderDetailRepository;
        public IOrderDetailRepository OrderDetailRepository
        {
            get
            {
                if (orderDetailRepository == null)
                {
                    orderDetailRepository = new OrderDetailRepository(_db);
                }
                return orderDetailRepository;
            }
        }

        private ICommentRepository commentRepository;
        public ICommentRepository CommentRepository
        {
            get
            {
                if (commentRepository == null)
                {
                    commentRepository = new CommentRepository(_db);
                }
                return commentRepository;
            }
        }

        private IDocumentRepository documentRepository;
        public IDocumentRepository DocumentRepository
        {
            get
            {
                if (documentRepository == null)
                {
                    documentRepository = new DocumentRepository(_db);
                }
                return documentRepository;
            }
        }
        #endregion

        #region Save
        public bool Save()
        {
            if (_db.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> SaveAsync()
        {
            if (await _db.SaveChangesAsync() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Destructor
        ~UnitOfWork()
        {
            Dispose(false);
        }
        #endregion
    }
}

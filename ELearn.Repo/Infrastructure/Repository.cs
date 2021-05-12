//using Shop.Common.Extentions;
//using Shop.Common.Pagination;
//using Shop.Data.Dtos.Common.Pagination;
using Banking.Data.Dtos.Common.Pagination;
using ELearn.Common.Extentions;
using ELearn.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Repo.Infrastructure
{
    public abstract class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        #region Constructor
        private readonly DbContext _db;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }
        #endregion

        #region 
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Entity Not Found");
            }
            _dbSet.Update(entity);
        }

        public void Delete(object id)
        {
            TEntity entity = Get(id);

            if (entity == null)
            {
                throw new ArgumentException("Entity Not Found");
            }
            _dbSet.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> expression)
        {
            IEnumerable<TEntity> entities = Get(expression, null, "");

            foreach (TEntity entity in entities)
            {
                _dbSet.Remove(entity);
            }
        }

        public IEnumerable<TEntity> Get()
        {
            return _dbSet.AsEnumerable();
        }

        public TEntity Get(object id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeEntity = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            foreach (var include in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.AsEnumerable();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression, string includeEntity = "")
        {
            IQueryable<TEntity> query = _dbSet;

            query = query.Where(expression);

            foreach (var include in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> expression,
            int page, int count,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeEntity = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            foreach (var include in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query.Skip(count * (page - 1)).Take(count);

            return query.AsEnumerable();
        }

        public PagedList<TEntity> GetPagedList(
            PaginationDto pagination,
            Expression<Func<TEntity, bool>> expression,
            string orderBy = "",
            string includeEntity = "")
        {
            IQueryable<TEntity> query;

            // Filter
            if (expression != null)
            {
                query = _dbSet.Where(expression);
            }
            else
            {
                query = _dbSet.AsQueryable();
            }
            // IncludeEntity
            foreach (var includeentity in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeentity);
            }
            // OrderBy
            if (string.IsNullOrEmpty(orderBy) || string.IsNullOrWhiteSpace(orderBy))
            {
                if (orderBy.Split(',')[1] == "asc")
                {
                    query = query.OrderBy(orderBy.Split(',')[0]);
                }
                else if (orderBy.Split(',')[1] == "desc")
                {
                    query = query.OrderByDescending(orderBy.Split(',')[0]);
                }
                else
                {
                    query = query.OrderBy(orderBy.Split(',')[0]);
                }

            }

            return PagedList<TEntity>.ToPagedList(query, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region Async
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeEntity = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            foreach (var include in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, string includeEntity = "")
        {
            IQueryable<TEntity> query = _dbSet;

            query = query.Where(expression);

            foreach (var include in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> expression,
            int page, int count,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeEntity = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            foreach (var include in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query.Skip(count * (page - 1)).Take(count);

            return await query.ToListAsync();
        }

        public async Task<PagedList<TEntity>> GetPagedListAsync(
            PaginationDto paginationDto,
            Expression<Func<TEntity, bool>> expression,
            string orderBy = "",
            string includeEntity = "")
        {
            IQueryable<TEntity> query;

            // Filter
            if (expression != null)
            {
                query = _dbSet.Where(expression);
            }
            else
            {
                query = _dbSet.AsQueryable();
            }
            // IncludeEntity
            foreach (var includeentity in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeentity);
            }
            // OrderBy
            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrWhiteSpace(orderBy))
            {
                if (orderBy.Split(',')[1] == "asc")
                {
                    query = query.OrderBy(orderBy.Split(',')[0]);
                }
                else if (orderBy.Split(',')[1] == "desc")
                {
                    query = query.OrderByDescending(orderBy.Split(',')[0]);
                }
                else
                {
                    query = query.OrderBy(orderBy.Split(',')[0]);
                }
            }

            return await PagedList<TEntity>.ToPagedListAsync(query, paginationDto.PageNumber, paginationDto.PageSize);
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
        ~Repository()
        {
            Dispose(false);
        }
        #endregion
    }
}

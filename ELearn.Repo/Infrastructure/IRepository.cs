using Banking.Data.Dtos.Common.Pagination;
using ELearn.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Repo.Infrastructure
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(object id);
        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> expression);

        IEnumerable<TEntity> Get();
        TEntity Get(object id);
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeEntity = "");
        TEntity Get(Expression<Func<TEntity, bool>> expression, string includeEntity = "");
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> expression,
            int page, int count,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeEntity = "");
        PagedList<TEntity> GetPagedList(
            PaginationDto pagination,
            Expression<Func<TEntity, bool>> expression,
            string orderBy = "",
            string includeEntity = "");

        //--------------------------------------------------------------------

        Task AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAsync();
        Task<TEntity> GetAsync(object id);
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeEntity = "");
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, string includeEntity = "");
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> expression,
            int page, int count,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeEntity = "");
        Task<PagedList<TEntity>> GetPagedListAsync(
            PaginationDto pagination,
            Expression<Func<TEntity, bool>> expression,
            string orderBy = "",
            string includeEntity = "");
    }
}

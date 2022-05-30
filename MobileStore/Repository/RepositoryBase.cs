using Microsoft.EntityFrameworkCore;
using MobileStore.Contracts;
using MobileStore.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MobileStore.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected MobileStoreDbContext context;

        public RepositoryBase(MobileStoreDbContext context)
        {
            this.context = context;
        }

        public void Create(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return (!trackChanges) ?
                context.Set<T>().AsNoTracking() :
                context.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return (!trackChanges) ? context.Set<T>().AsNoTracking().Where(expression) :
                context.Set<T>().Where(expression);
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
    }
}

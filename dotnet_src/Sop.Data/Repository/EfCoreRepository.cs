using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sop.Data.Repository
{
    /// <summary>
    ///     EfCoreRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EfCoreRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Attach(entity);
            _context.Update(entity);
        }

        /// <summary>
        /// </summary>
        /// <param name="entities"></param>
        public void Update(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                throw new ArgumentNullException("entities");

            _context.UpdateRange(entities);
        }

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties"></param>
        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                if (string.IsNullOrEmpty(propertyName)) propertyName = GetPropertyName(property.Body.ToString());
                _context.Entry(entity).Property(propertyName).IsModified = true;
            }
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.Remove(entity);
        }


        public void Delete(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                throw new ArgumentNullException("entities");

            _context.RemoveRange(entities);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            _context.RemoveRange(Entities.Where(predicate));
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GetPropertyName(string str)
        {
            return str.Split(',')[0].Split('.')[1];
        }

    #region MyRegion

        private readonly DbContext _context;
        private DbSet<TEntity> _entities;

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public EfCoreRepository(DbContext context)
        {
            _context = context;
        }

    #endregion

    #region GetById Gets

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(object id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            return Entities.Find(id);
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TEntity> GetByIdAsync(object id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            return Entities.FindAsync(id);
        }

        /// <summary>
        ///     Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => Entities;

        /// <summary>
        ///     Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only
        ///     operations
        /// </summary>
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

    #endregion


    #region Insert

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Add(entity);
        }

        /// <summary>
        /// </summary>
        /// <param name="entities"></param>
        public void Insert(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                throw new ArgumentNullException("entities");

            Entities.AddRange(entities);
        }

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return Entities.AddAsync(entity);
        }

        /// <summary>
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                throw new ArgumentNullException("entities");

            return Entities.AddRangeAsync(entities);
        }

    #endregion

    #region Properties

        /// <summary>
        ///     Gets an entity set
        /// </summary>
        protected virtual DbSet<TEntity> Entities => _entities ?? (_entities = _context.Set<TEntity>());

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        private void AttachIfNot(TEntity entity)
        {
            var entry = _context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null) return;
            _context.Attach(entity);
        }

    #endregion
    }
}
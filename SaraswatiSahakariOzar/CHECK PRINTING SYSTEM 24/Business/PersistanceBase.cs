using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Business
{
    public class PersistenceBase<T>
        where T : BaseEntity
    {
        public PersistenceBase(CPSDbContext context)
        {
            DbContext = context;
        }

        /// <summary>
        /// Get or set the DBContext.
        /// </summary>
        private CPSDbContext DbContext { get; set; }

        /// <summary>
        /// Retuns the all entites.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = DbContext.Set<T>().Where(w => !w.IsDeleted);
            return query;
        }

        /// <summary>
        /// Retuns entities who's are match with filter criteria.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<T> FilterBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = GetAll().Where(predicate);
            return query;
        }

        /// <summary>
        /// Retuns first entity who's is match with filter criteria.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = GetAll().Where(predicate);
            return query;
        }

        /// <summary>
        /// Add entity into contex.
        /// </summary>
        /// <param name="entity"></param>
        public bool SaveOrUpdate(T entity, List<System.ComponentModel.DataAnnotations.ValidationResult> validationErrors)
        {
            SetAuditInfo(entity);

            if (entity.IsValid(validationErrors))
            {
                if (entity.Id == 0)
                    return Add(entity);
                else
                    return Edit(entity);
            }

            return validationErrors.Count == 0;
        }

        /// <summary>
        /// Add entity into contex.
        /// </summary>
        /// <param name="entity"></param>
        public bool SaveOrUpdate(T entity)
        {
            SetAuditInfo(entity);

            if (entity.Id == 0)
                return Add(entity);
            else
                return Edit(entity);
        }

        /// <summary>
        /// Add entity into contex.
        /// </summary>
        /// <param name="entity"></param>
        private bool Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
            return true;
        }

        /// <summary>
        /// Update the entity state to modified into context.
        /// </summary>
        /// <param name="entity"></param>
        private bool Edit(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            return true;
        }

        /// <summary>
        /// Set Audit Fields
        /// </summary>
        /// <param name="entity"></param>
        private void SetAuditInfo(T entity)
        {
            if (App.Current.Windows[0].Tag != null)
            {
                var oLoggedInUser = (UserMasterDTO)App.Current.Windows[0].Tag;
                if (entity.Id == 0 || entity.CreatedBy == null)
                {
                    entity.CreatedBy = oLoggedInUser.UserId;
                    entity.CreatedOn = DateTime.Now;
                }
                entity.UpdatedBy = oLoggedInUser.UserId;
                entity.UpdatedOn = DateTime.Now;
            }
        }

        /// <summary>
        /// Delete entity from the context.
        /// </summary>
        /// <param name="entity"></param>
        public virtual bool Delete(T entity)
        {
            SetAuditInfo(entity);
            DbContext.Set<T>().Remove(entity);
            return true;
        }

        public virtual DbSet<T> Set()
        {
            return DbContext.Set<T>();
        }
    }
}

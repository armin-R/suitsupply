using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Armin.Suitsupply.Domain.Data;
using Armin.Suitsupply.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Armin.Suitsupply.Domain.Stores
{
    public class Store<T> : IStore<T> where T : EntityBase
    {
        protected readonly DomainDbContext DbContext;
        private DbSet<T> _entities;

        public Store(DomainDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual T Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Add(entity);
            SaveChanges();
            return entity;
        }

        public virtual T GetById(long id)
        {
            return Entities.Find(id);
        }


        public virtual void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.LastUpdate = DateTime.Now;

            SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Remove(entity);
            SaveChanges();
        }

        public virtual int SaveChanges()
        {
            var entities = from e in DbContext.ChangeTracker.Entries()
                where e.State == EntityState.Added
                      || e.State == EntityState.Modified
                select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
            }

            return DbContext.SaveChanges();
        }

        public virtual IQueryable<T> Table => Entities;

        private DbSet<T> Entities => _entities ?? (_entities = DbContext.Set<T>());
    }
}
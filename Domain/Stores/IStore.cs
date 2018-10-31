using System;
using System.Collections.Generic;
using System.Text;
using Armin.Suitsupply.Domain.Entities;

namespace Armin.Suitsupply.Domain.Stores
{
    public interface IStore<T> where T : EntityBase

    {
        T GetById(long id);

        T Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        int SaveChanges();

    }
}


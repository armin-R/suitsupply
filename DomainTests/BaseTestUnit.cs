using System;
using System.Collections.Generic;
using System.Text;
using Armin.Suitsupply.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace DomainTests
{
    public class BaseTestUnit
    {
        public DomainDbContext BuildDataContext(bool withData = true)
        {
            var builder = new DbContextOptionsBuilder<DomainDbContext>();
            builder = builder.UseInMemoryDatabase("TestDb");

            var db = new DomainDbContext(builder.Options);
            db.Database.EnsureCreated();

            if (withData)
            {
                DataSeed seed = new DataSeed(db);
                seed.Seed();
            }

            return db;
        }
    }
}

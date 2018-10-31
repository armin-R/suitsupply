using System;
using System.Collections.Generic;
using System.Text;
using Armin.Suitsupply.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Armin.Suitsupply.Domain.Data
{
    public class DomainDbContext : DbContext
    {
        protected DomainDbContext()
        {
        }

        public DomainDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
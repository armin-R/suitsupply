using System;
using System.Collections.Generic;
using System.Text;
using Armin.Suitsupply.Domain.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace Armin.Suitsupply.Domain.Data
{
    public class DataSeed
    {
        private readonly DomainDbContext _context;

        public DataSeed(DomainDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Products.Any()) return;

            _context.Products.Add(new Product {Name = "Product 1", Price = 100});
            _context.Products.Add(new Product {Name = "Product 2", Price = 200});
            _context.SaveChanges();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Suitsupply.Domain.Data;
using Armin.Suitsupply.Domain.Entities;
using Armin.Suitsupply.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Armin.Suitsupply.Domain.Stores
{
    public class ProductStore : Store<Product>, IProductStore
    {
        public ProductStore(DomainDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Product>> Search(string name)
        {
            IQueryable<Product> query = Table;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
            }

            return await query.ToListAsync();
        }

        public override void Delete(Product product)
        {
            FileSystem fs = new FileSystem();
            if (!string.IsNullOrEmpty(product.Photo))
            {
                if (fs.Exists(product.Photo))
                {
                    fs.Delete(product.Photo);
                }
            }

            base.Delete(product);
        }
    }
}
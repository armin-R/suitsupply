using System.Collections.Generic;
using System.Threading.Tasks;
using Armin.Suitsupply.Domain.Entities;

namespace Armin.Suitsupply.Domain.Stores
{
    public interface IProductStore
    {
        Product Create(Product entity);
        Product GetById(long id);
        void Update(Product entity);
        void Delete(Product entity);
        Task<IEnumerable<Product>> Search(string name);
    }
}
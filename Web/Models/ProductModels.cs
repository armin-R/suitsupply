using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Armin.Suitsupply.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Armin.Suitsupply.Web.Models
{
    public class ProductInputModel
    {
        public Product ToEntity()
        {
            return new Product
            {
                Name = Name,
                Price = Price
            };
        }


        [Required] public string Name { get; set; }

        public IFormFile Photo { get; set; }

        [Required] public decimal Price { get; set; }
    }


    public class ProductOutputModel
    {
        public static ProductOutputModel FromEntity(Product p)
        {
            return new ProductOutputModel
            {
                Id = p.Id,
                LastUpdate = p.LastUpdate,
                Name = p.Name,
                Photo = p.Photo,
                Price = p.Price
            };
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public decimal Price { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
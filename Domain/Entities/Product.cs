using System;
using System.ComponentModel.DataAnnotations;

namespace Armin.Suitsupply.Domain.Entities
{
    public class Product : EntityBase
    {
        public Product()
        {
            Price = 0;
        }

        [Required]
        public string Name { get; set; }

        public string Photo { get; set; }

        [Required]
        public decimal Price { get; set; }


    }
}

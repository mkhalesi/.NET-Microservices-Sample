using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Model.Entities
{
    public class Category
    {
        public Category()
        {
            Products = new List<Product>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Product> Products { get; set; }

    }
}

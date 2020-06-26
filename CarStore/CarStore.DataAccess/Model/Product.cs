using System;
using System.Collections.Generic;

namespace CarStore.DataAccess.Model
{
    public partial class Product
    {
        public Product()
        {
            Stock = new HashSet<Stock>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<Stock> Stock { get; set; }
    }
}

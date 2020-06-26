using System;
using System.Collections.Generic;

namespace CarStore.DataAccess.Model
{
    public partial class Stock
    {
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int LocationId { get; set; }
        public int Inventory { get; set; }

        public virtual Location Location { get; set; }
        public virtual Product Product { get; set; }
    }
}

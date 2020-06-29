using System;
namespace CarStore.Library.Model
{
    public class Stock
    {
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int LocationId { get; set; }
        public int Inventory { get; set; }
    }
}

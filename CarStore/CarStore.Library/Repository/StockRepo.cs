using System;
using System.Linq;
using CarStore.DataAccess.Model;

namespace CarStore.Library.Repository
{
    public class StockRepo
    {
        public readonly IRepository<Stock> SReposity = new GenericRepo<Stock>();

        /// <summary>
        /// print all stock
        /// </summary>
        public void DisplayStock()
        {
            var stocks = SReposity.GetAll().ToList();

            foreach (var stock in stocks)
            {
                Console.WriteLine($"Stock ID [{stock.StockId}], at location ID [{stock.LocationId}], has product ID [{stock.ProductId}], with {stock.Inventory} stock.");
            }
        }
    }
}

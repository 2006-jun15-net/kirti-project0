using System;
using System.Linq;
using CarStore.DataAccess.Model;

namespace CarStore.Library.Repository
{
    /// <summary>
    /// product repository
    /// </summary>
    public class ProductRepo
    {
        public readonly IRepository<Product> PReposity = new GenericRepo<Product>();

        /// <summary>
        /// print all the products
        /// </summary>
        public void DisplayProducts()
        {
            var products = PReposity.GetAll().ToList();

            foreach (var product in products)
            {
                Console.WriteLine($"Product ID [{product.ProductId}], name of the product is {product.ProductName} and the price is {product.Price}");
            }
        }
    }
}

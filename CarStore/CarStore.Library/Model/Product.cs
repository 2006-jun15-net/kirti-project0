using System;
namespace CarStore.Library.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Product
    {
        // private fields for product name to ensure the name is valid
        private string _name;
        private decimal _price;

        public Product()
        {
        }

        /// <summary>
        /// product id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// product name, cannot be empty
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (value.Length == 0)
                    throw new ArgumentException("First-name cannot be empty", nameof(value));

                _name = value;
            }
        }

        /// <summary>
        /// price of the product
        /// </summary>
        public decimal Price
        {
            get => _price;
            set
            {
                if (_price <= 0)
                    throw new ArgumentException("Price cannot be less than 0", nameof(value));
                _price = value;
            }
        }
    }
}

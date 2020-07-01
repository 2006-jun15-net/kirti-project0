using System;
namespace CarStore.Library.Model
{
    /// <summary>
    /// stock object
    /// </summary>
    public class Stock
    {
        // private fields for product name to ensure the name is valid
        private int _locationId;
        private int _productId;
        private int _inventory;

        public int StockId { get; set; }

        public int ProductId
        {
            get => _productId;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Must select product.", nameof(value));

                _productId = value;
            }
        }

        public int LocationId
        {
            get => _locationId;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Location not selected, cannot place order without selecting location", nameof(value));

                _locationId = value;
            }
        }

        public int Inventory
        {
            get => _inventory;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Cannot be 0 or less, enter a higher number", nameof(value));

                _inventory = value;
            }
        }
        }
}

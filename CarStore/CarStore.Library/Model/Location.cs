using System;
using System.Collections.Generic;

namespace CarStore.Library.Model
{
    /// <summary>
    /// Location object: id, name, and stock of the products
    /// </summary>
    public class Location
    {
        // private fields for location name to ensure the name is valid
        private string _name;

        public Location()
        {
        }

        /// <summary>
        /// location id
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// location name, cannot be empty
        /// </summary>
        public string LocationName
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
        /// Dictionary collection ot keep track of the stock (like inventory)
        /// </summary>
        public Dictionary<Product, int> Stock { get; set; }
    }
}

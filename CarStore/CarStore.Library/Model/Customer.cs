using System;
using System.Collections.Generic;

namespace CarStore.Library.Model
{
    /// <summary>
    /// Customer object: Id, first and last name, username
    /// </summary>
    public class Customer
    {
        // private fields for first and last name and username to ensure the name is valid
        private string _fName;
        private string _lName;
        private string _userName;

        public Customer()
        {
        }

        /// <summary>
        /// Customer's id 
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Customer's first name, cannot be empty
        /// </summary>
        public string FirstName
        {
            get => _fName;

            set
            {
                if (value.Length == 0)
                    throw new ArgumentException("First-name cannot be empty", nameof(value));
                if (value.Length > 26)
                    throw new ArgumentException("First-name is too long", nameof(value));

                _fName = value;
            }
        }

        /// <summary>
        /// Customer's last name, cannot be empty
        /// </summary>
        public string LastName
        {
            get => _lName;
            set
            {
                if (value.Length == 0)
                    throw new ArgumentException("Last-name cannot be empty", nameof(value));
                if (value.Length > 26)
                    throw new ArgumentException("Last-name is too long", nameof(value));

                _lName = value;
            }
        }

        /// <summary>
        /// Customer's user name, cannot be empty
        /// </summary>
        public string UserName
        {
            get => _userName;
            set
            {
                if (value.Length == 0)
                    throw new ArgumentException("Last-name cannot be empty", nameof(value));

                _userName = value;
            }
         }

        /// <summary>
        /// Order history of the customer
        /// </summary>
        public List<Orders> Ohistory { get; set; }

    }
}

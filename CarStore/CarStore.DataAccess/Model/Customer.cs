using System;
using System.Collections.Generic;

namespace CarStore.DataAccess.Model
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Orders>();
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int? LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}

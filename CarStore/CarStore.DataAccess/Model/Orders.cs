using System;
using System.Collections.Generic;

namespace CarStore.DataAccess.Model
{
    public partial class Orders
    {
        public Orders()
        {
            Sold = new HashSet<Sold>();
        }

        public int OrderId { get; set; }
        public int LocationId { get; set; }
        public int CustomerId { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderTime { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<Sold> Sold { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace CarStore.DataAccess.Model
{
    public partial class OrderLine
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public virtual Orders Order { get; set; }
        public virtual Product Product { get; set; }
    }
}

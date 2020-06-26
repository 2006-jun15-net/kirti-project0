using System;
using System.Collections.Generic;

namespace CarStore.DataAccess.Model
{
    public partial class Sold
    {
        public int Sold1 { get; set; }
        public int OrderId { get; set; }

        public virtual Orders Order { get; set; }
    }
}

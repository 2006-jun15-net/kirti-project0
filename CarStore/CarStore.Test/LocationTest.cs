using System;
using CarStore.Library.Model;
using Xunit;

namespace CarStore.Test
{
    public class LocationTest
    {
        Location location = new Location();

        [Fact]
        public void LocationNameEmptyTest ()
        {
            Assert.ThrowsAny<ArgumentException>(() => location.LocationName = "");
        }

    }
}

using System;
using CarStore.Library.Model;
using Xunit;

namespace CarStore.Test
{
    public class OrderTest
    {
        Orders orders = new Orders();

        [Fact]
        public void CustomerIdTest()
        {
            Assert.Equal(0, orders.OrderId);
        }

        [Fact]
        public void LocationIdTest()
        {
            Assert.Equal(0, orders.OrderId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-545)]
        public void PriceNegativeOrZeroTest(decimal price)
        {
            Assert.ThrowsAny<ArgumentException>(() => orders.Price = price);
        }
    }
}

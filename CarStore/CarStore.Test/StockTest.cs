using System;
using CarStore.Library.Model;
using Xunit;

namespace CarStore.Test
{
    public class StockTest
    {
        Stock stock = new Stock();

        [Fact]
        public void LocationIdTest()
        {
            Assert.Equal(0, stock.LocationId);
        }

        [Fact]
        public void ProductIdTest()
        {
            Assert.Equal(0, stock.ProductId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-545)]
        public void InventoryNegativeOrZeroTest(int inventory)
        {
            Assert.ThrowsAny<ArgumentException>(() => stock.Inventory = inventory);
        }

    }
}

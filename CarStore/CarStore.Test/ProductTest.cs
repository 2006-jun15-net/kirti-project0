using System;
using CarStore.Library.Model;
using Xunit;

namespace CarStore.Test
{
    public class ProductTest
    {
        Product product = new Product();

        [Fact]
        public void ProductNameEmptyTest ()
        {
            Assert.ThrowsAny<ArgumentException>(() => product.ProductName = "");
        }

        [Fact]
        public void ProductPriceNegativeTest ()
        {
            Assert.ThrowsAny<ArgumentException>(() => product.Price = -23);
        }

        [Fact]
        public void ProductPriceZeroTest()
        {
            Assert.ThrowsAny<ArgumentException>(() => product.Price = 0);
        }
    }
}

using System;
using CarStore.Library.Model;
using Xunit;

namespace CarStore.Test
{
    public class CustomerTest
    {
        Customer customer = new Customer();

        [Theory]
        [InlineData("")]
        [InlineData("dfghjkjhsertyuiuytresdfhklkjhgfdfghjk")]

        public void CustomerFirstNameEmptyOrTooLongTest(string name)
        {
            Assert.ThrowsAny<ArgumentException>(() => customer.FirstName = name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("dfghjkjhsertyuiuytresfghjdfhklkjhgfdfghjk")]

        public void CustomerLastNameEmptyOrTooLongTest(string name)
        {
            Assert.ThrowsAny<ArgumentException>(() => customer.LastName = name);
        }
    }
}

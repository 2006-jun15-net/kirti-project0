using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CarStore.Library.Repository;
using CarStore.DataAccess.Model;

namespace CarStore.Test
{
    public class GenericRepoTest
    {
        public static readonly DbContextOptions<Project0Context> Options = new DbContextOptionsBuilder<Project0Context>()
            //.UseLoggerFactory(MyLoggerFactory)
            .UseSqlServer(CarStore.Library.Repository.SecretConfiguration.ConnectionString)
            .Options;

        [Fact]
        public void GetAllTest()
        {
            using var context = new Project0Context(Options);
            var repository = new GenericRepo<Location>(context);

            Console.WriteLine(repository.GetAll());
        }
    }
}

using System;
using System.Linq;
using CarStore.DataAccess.Model;

namespace CarStore.Library.Repository
{
    /// <summary>
    /// location repository
    /// </summary>
    public class LocationRepo
    {
        public readonly IRepository<Location> LReposity = new GenericRepo<Location>();

        /// <summary>
        /// print all locations
        /// </summary>
        public void DisplayLocation ()
        {
            var locations = LReposity.GetAll().ToList();

            foreach(var location in locations)
            {
                Console.WriteLine($"Location ID [{location.LocationId}], name of the location is {location.LocationName}");
            }
        }
    }
}

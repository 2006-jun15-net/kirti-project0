using System;
using System.Collections.Generic;
using CarStore.Library.Model;

namespace CarStore.Library
{
    /// <summary>
    /// repository interface class
    /// used by the repository classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// get all the information for the specified id
        /// </summary>
        /// <param name="id">(table)Id</param>
        /// <returns>the object at that specified id</returns>
        T GetWithId(object id);

        /// <summary>
        /// get everything from the table
        /// </summary>
        /// <returns>list of all the entries in the table</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// add to the table
        /// </summary>
        /// <param name="obj">object(new entry) to be added intot the table</param>
        void Add(T obj);

        /// <summary>
        /// delete from entry from the table at specified id
        /// </summary>
        /// <param name="id">(table)Id</param>
        void Delete(object id);

        /// <summary>
        /// save the changes
        /// </summary>
        void Save();
    }
}
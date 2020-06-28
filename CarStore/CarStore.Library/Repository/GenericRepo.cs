using System;
using System.Collections.Generic;
using CarStore.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarStore.Library.Repository
{
    /// <summary>
    /// generic repo class that uses repository interface
    /// this generic class will be called by other repository classes
    /// designed to communicate with the database and manage data access
    /// </summary>
    /// <typeparam name="T">class object</typeparam>
    public class GenericRepo<T> : IRepository<T> where T : class
    {
        // logger factory
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        // establish connection
        public static readonly DbContextOptions<Project0Context> Options = new DbContextOptionsBuilder<Project0Context>()
            .UseLoggerFactory(MyLoggerFactory)
            .UseSqlServer(SecretConfiguration.ConnectionString)
            .Options;

        // private fieds
        private readonly Project0Context context; // connection
        private DbSet<T> _tableLst; //  to modify/retun tables

        /// <summary>
        /// constructor to sent connection and table
        /// </summary>
        public GenericRepo()
        {
            context = new Project0Context(Options);
            _tableLst = context.Set<T>();
        }

        public GenericRepo(Project0Context dbContext)
        {
            context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// get the information at given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns information from the given id</returns>
        public T GetWithId(object id)
        {
            return _tableLst.Find(id);
        }

        /// <summary>
        /// gets all the information from the specified table
        /// </summary>
        /// <returns>return all the data from given table</returns>
        public IEnumerable<T> GetAll()
        {
            return _tableLst;
        }

        /// <summary>
        /// add and entry to the table
        /// </summary>
        /// <param name="obj">takes the object that needs to be added</param>
        public void Add(T obj)
        {
            _tableLst.Add(obj);
        }

        /// <summary>
        /// delete entry at givn id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(object id)
        {
            T deleteId = _tableLst.Find(id) ;
            _tableLst.Remove(deleteId);
        }

        /// <summary>
        /// save any changes made to the database
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
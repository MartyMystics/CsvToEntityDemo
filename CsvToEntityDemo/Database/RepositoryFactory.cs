using System;
using Autofac;
using CsvToEntityDemo.Interfaces;
using CsvToEntityDemo.Models;

namespace CsvToEntityDemo.Database
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IContainer container;

        public RepositoryFactory(IContainer container)
        {
            this.container = container;
        }

        public IRepository<T> GetRepository<T>(T entity) where T : class
        {
            var repository = typeof(IContainer)
                .GetMethod("Resolve")
                .MakeGenericMethod(typeof(IRepository<T>))
                .Invoke(container, new object[] {});

            return repository as IRepository<T>;
        }
    }
}

using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Mike.Spikes.Windsor101
{
    public interface IDataAccess<T> where T : IEntity,  new()
    {
        T GetById(int id);
        void Store(T item);
    }

    public class DataAccess<T> : IDataAccess<T> where T : IEntity, new()
    {
        public T GetById(int id)
        {
            return new T { Id = id };
        }

        public void Store(T item)
        {
            Console.Out.WriteLine("Stored {0} with id {1}", typeof(T).Name, item.Id);
        }
    }

    public class EmployeeDataAccess : IDataAccess<Employee>
    {
        public Employee GetById(int id)
        {
            return new Employee {Id = id};
        }

        public void Store(Employee item)
        {
            Console.Out.WriteLine("EmployeeDataAccess item.Id = {0}", item.Id);
        }
    }

    public interface IEntity
    {
        int Id { get; set; }
    }

    public class Customer : IEntity
    {
        public int Id { get; set; }
    }

    public class Employee : IEntity
    {
        public int Id { get; set; }
    }

    public class GenericsDemoInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For(typeof(IDataAccess<>)).ImplementedBy(typeof(DataAccess<>)),
                Component.For<IDataAccess<Employee>>().ImplementedBy<EmployeeDataAccess>()
                );
        }
    }
}
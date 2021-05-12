using CustomerDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerWebAPI.Repositories
{
  public  interface ICustomerRespository
    {
        Task<IEnumerable<Customer>> GetCustomers(int page, int size);

        Task<Customer> GetCustomer(int ID);

        Task<Customer> SaveCustomer(Customer customer);

        Task<Customer> UpdateCustomer(int iD, Customer customer);

        Task<Customer> DeleteCustomer(int iD);

    }
}

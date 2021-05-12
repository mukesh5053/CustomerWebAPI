using CustomerDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerWebAPI.DataContext;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebAPI.Repositories
{
    public class CustomerRepositories : ICustomerRespository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepositories(CustomerDbContext context)
        {
            _context = context;
        }
        public async Task<Customer> DeleteCustomer(int ID)
        {
          var result = await _context.Customer.FirstOrDefaultAsync(x => x.ID == ID);
            if (result != null )    
                {
                    _context.Customer.Remove(result);
                  await _context.SaveChangesAsync();
                }
             return result;
        }

        public async Task<Customer> GetCustomer(int ID)
        {
           return await _context.Customer.FirstOrDefaultAsync(x => x.ID == ID);           
        }

        //public async Task<IEnumerable<Customer>> GetCustomersByPaging(int page, int size)
        //{
        //    IQueryable<Customer> customer = _context.Customer;
        //     customer =  customer.Skip(page * (size - 1)).Take(size);
        //    return await customer.ToListAsync();
        //}

        public async Task<IEnumerable<Customer>> GetCustomers(int page, int size)
        {
            return await _context.Customer.ToListAsync();            
            //IQueryable<Customer> customer = _context.Customer;
            //customer = customer.Skip(size * (page - 1)).Take(size);
            //return await customer.ToListAsync();
        }

        public async Task<Customer> SaveCustomer(Customer customer)
        {
          var result = _context.Customer.Add(customer);
            await _context.SaveChangesAsync();
            return result.Entity;

        }

        public async Task<Customer> UpdateCustomer(int ID, Customer customer)
        {
            var result = await _context.Customer.FirstOrDefaultAsync(x => x.ID == ID);
            if (result != null)
            {
                result.CustomerCode = customer.CustomerCode;
                result.CustomerName = customer.CustomerName;
                await _context.SaveChangesAsync();
            }
            return result;
        }
    }
}

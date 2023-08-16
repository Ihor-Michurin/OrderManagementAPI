using Database;
using Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic
{
    public class CustomerStore
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerStore(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task UpdateCustomerOrderCountAsync(Guid customerId)
        {
            var customer = await _dbContext.Customers.FindAsync(customerId);

            if (customer != null)
            {
                customer.OrderCount += 1;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

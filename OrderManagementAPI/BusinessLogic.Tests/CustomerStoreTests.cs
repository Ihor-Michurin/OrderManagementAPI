using Xunit;
using Microsoft.EntityFrameworkCore;
using Database;
using Models;

namespace BusinessLogic.Tests
{
    public class CustomerStoreTests
    {
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddCustomer_Should_Add_Customer_To_DatabaseAsync()
        {
            // Arrange
            using var dbContext = CreateDbContext();
            var customerStore = new CustomerStore(dbContext);

            var customer = new Customer { Id = Guid.NewGuid(), Name = "Test Customer" };

            // Act
            await customerStore.AddCustomerAsync(customer);

            // Assert
            Assert.Single(dbContext.Customers);
            Assert.Equal(customer.Name, dbContext.Customers.First().Name);
        }

        [Fact]
        public async Task UpdateCustomerOrderCount_Should_Increment_Order_CountAsync()
        {
            // Arrange
            using var dbContext = CreateDbContext();
            var customerStore = new CustomerStore(dbContext);

            var customerId = Guid.NewGuid();
            var customer = new Customer { Id = customerId, Name = "Test Customer", OrderCount = 0 };
            dbContext.Customers.Add(customer);
            dbContext.SaveChanges();

            // Act
            await customerStore.UpdateCustomerOrderCountAsync(customerId);

            // Assert
            Assert.Equal(1, dbContext.Customers.First().OrderCount);
        }
    }
}

using Azure.Messaging.ServiceBus;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace BusinessLogic
{
    public class OrderStore
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _serviceBusConnectionString;
        private readonly string _serviceBusTopicName;

        public OrderStore(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _serviceBusConnectionString = configuration["ServiceBus:ConnectionString"];
            _serviceBusTopicName = configuration["ServiceBus:TopicName"];
        }

        public async Task AddOrderAsync(Order order)
        {
            try
            {
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();

                await using var client = new ServiceBusClient(_serviceBusConnectionString);

                var sender = client.CreateSender(_serviceBusTopicName);

                var message = new ServiceBusMessage(order.CustomerId.ToString());
                await sender.SendMessageAsync(message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<Order> UpdateOrderStatusAsync(Guid orderId, string newDescription)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return null;
            }

            order.Description = newDescription;
            await _dbContext.SaveChangesAsync();
            return order;
        }
    }
}

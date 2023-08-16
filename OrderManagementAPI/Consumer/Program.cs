using Azure.Messaging.ServiceBus;
using BusinessLogic;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Consumer
{
    internal class Program
    {
        private static CustomerStore _customerStore;

        static async Task Main(string[] args)
        {
            var connectionString = "Endpoint=sb://ordermanagementapitest.servicebus.windows.net/;SharedAccessKeyName=consumer;SharedAccessKey=H2b7/t4GlJgjF5ObKVS4LoQxVVrphwzxL+ASbJ4fBzU=";
            var topicName = "orders";
            var subscriptionName = "consumer";

            var serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql("Server=ordermanagementapi.postgres.database.azure.com;Port=5432;Database=OrderManagementAPI;User Id=postgres;Password=48abf328-9500-4fbc-8051-f5149ff31501");
                })
                .AddScoped<CustomerStore>()
                .BuildServiceProvider();

            _customerStore = serviceProvider.GetService<CustomerStore>();

            await using var client = new ServiceBusClient(connectionString);
            var processor = client.CreateProcessor(topicName, subscriptionName);

            var customerStore = serviceProvider.GetService<CustomerStore>();

            processor.ProcessMessageAsync += ProcessMessageHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;

            await processor.StartProcessingAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            await processor.StopProcessingAsync();

        }

        static async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            string customerId = args.Message.Body.ToString();

            await _customerStore.UpdateCustomerOrderCountAsync(Guid.Parse(customerId));

            Console.WriteLine($"Received message: CustomerId = {customerId}");

            await args.CompleteMessageAsync(args.Message);
        }

        static Task ProcessErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Error: {args.Exception}");
            return Task.CompletedTask;
        }
    }
}
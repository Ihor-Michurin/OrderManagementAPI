using BusinessLogic;
using Database;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.EntityFrameworkCore;

namespace OrderManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.Configure<ServiceBusOptions>(builder.Configuration.GetSection("ServiceBus"));
            builder.Services.AddScoped<OrderStore>();
            builder.Services.AddScoped<CustomerStore>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace OrderManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerStore _customerStore;

        public CustomersController(CustomerStore customerStore)
        {
            _customerStore = customerStore;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            await _customerStore.AddCustomerAsync(customer);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerStore.GetAllCustomersAsync();
            return Ok(customers);
        }
    }
}

namespace CallMeSdk.Client.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerProviderService _customerProviderService;

        public CustomersController(ICustomerProviderService customerProviderService)
        {
            _customerProviderService = customerProviderService;
        }

        [HttpGet("{clientName}")]
        public async Task<IActionResult> GetCustomers(string clientName)
        {
            var customers = await _customerProviderService.GetCustomersAsync(clientName);
            return customers is not null ? Ok(customers) : NotFound("Provider not found");
        }
    }
}

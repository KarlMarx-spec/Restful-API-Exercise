using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:long}")]   
        public async Task<IResult> GetCustomerAsync([FromRoute] long id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (customer is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(customer);
        }

        [HttpPost("")]
        public async Task<IResult> CreateCustomerAsync([FromBody] Customer customer)
        {
            if (await _context.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id) is null)
            {
                await _context.Customers.AddAsync(customer);
                _context.SaveChanges();
                return Results.Ok();
            }
            return Results.Conflict();
        }
    }
}

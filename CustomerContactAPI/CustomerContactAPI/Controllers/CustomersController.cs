using CustomerContactAPI.Data;
using CustomerContactAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerContactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CustomersController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            if(context.Customers == null)
            {
                return NotFound("Customer data is not available");
            }

            return await context.Customers.ToListAsync();
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            if(context.Customers == null)
            {
                return NotFound("Customer data is not available");
            }
            var customer = await context.Customers.FindAsync(id);
            if(customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }
            return customer;
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            if(context.Customers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customers' is null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        // PUT: api/customers/{id}
        [HttpPut]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if(id != customer.Id)
            {
                return BadRequest("Customer ID mismatch.");
            }
        
            context.Entry(customer).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound($"Customer with ID {id} not found for update.");
                }
                else
                {
                    throw; // Başka bir concurrency hatası varsa fırlat
                }
            }
            return NoContent();
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if(context.Customers == null)
            {
                return NotFound("Customer data is not available");
            }
            var customer = await context.Customers.FindAsync(id);
            if(customer == null)
            {
                return NotFound($"Customer with ID {id} not found for deletion.");
            }
            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return (context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

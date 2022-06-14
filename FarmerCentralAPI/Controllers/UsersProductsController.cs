using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmerCentralAPI.Models;

namespace FarmerCentralAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersProductsController : ControllerBase
    {
        private readonly FarmCentralContext _context;

        public UsersProductsController(FarmCentralContext context)
        {
            _context = context;
        }

        // GET: api/UsersProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersProduct>>> GetUsersProducts()
        {
            return await _context.UsersProducts.ToListAsync();
        }

        // GET: api/UsersProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsersProduct>> GetUsersProduct(int id)
        {
            var usersProduct = await _context.UsersProducts.FindAsync(id);

            if (usersProduct == null)
            {
                return NotFound();
            }

            return usersProduct;
        }

        // PUT: api/UsersProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsersProduct(int id, UsersProduct usersProduct)
        {
            if (id != usersProduct.UsersProductId)
            {
                return BadRequest();
            }

            _context.Entry(usersProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UsersProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsersProduct>> PostUsersProduct(UsersProduct usersProduct)
        {
            _context.UsersProducts.Add(usersProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsersProduct", new { id = usersProduct.UsersProductId }, usersProduct);
        }

        // DELETE: api/UsersProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsersProduct(int id)
        {
            var usersProduct = await _context.UsersProducts.FindAsync(id);
            if (usersProduct == null)
            {
                return NotFound();
            }

            _context.UsersProducts.Remove(usersProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersProductExists(int id)
        {
            return _context.UsersProducts.Any(e => e.UsersProductId == id);
        }
    }
}

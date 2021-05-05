using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSUNO.Api.Data;
using POSUNO.Api.Data.Entities;

namespace POSUNO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostumersController : ControllerBase
    {
        private readonly DataContext _context;

        public CostumersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Costumers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCostumers()
        {
            return await _context.Costumers.ToListAsync();
        }

        // GET: api/Costumers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCostumer(int id)
        {
            var costumer = await _context.Costumers.FindAsync(id);

            if (costumer == null)
            {
                return NotFound();
            }

            return costumer;
        }

        // PUT: api/Costumers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCostumer(int id, Customer costumer)
        {
            if (id != costumer.Id)
            {
                return BadRequest();
            }

            _context.Entry(costumer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostumerExists(id))
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

        // POST: api/Costumers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCostumer(Customer costumer)
        {
            _context.Costumers.Add(costumer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCostumer", new { id = costumer.Id }, costumer);
        }

        // DELETE: api/Costumers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCostumer(int id)
        {
            var costumer = await _context.Costumers.FindAsync(id);
            if (costumer == null)
            {
                return NotFound();
            }

            _context.Costumers.Remove(costumer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CostumerExists(int id)
        {
            return _context.Costumers.Any(e => e.Id == id);
        }
    }
}

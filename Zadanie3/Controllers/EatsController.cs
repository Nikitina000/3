using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zadanie3;
using Zadanie3.Model;

namespace Zadanie3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EatsController : ControllerBase
    {
        private readonly EatsContext _context;

        public EatsController(EatsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Eat>>> GetEats()
        {
          if (_context.Eats == null)
          {
              return NotFound();
          }
            return await _context.Eats.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Eat>> GetEat(int id)
        {
          if (_context.Eats == null)
          {
              return NotFound();
          }
            var eat = await _context.Eats.FindAsync(id);

            if (eat == null)
            {
                return NotFound();
            }

            return eat;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEat(int id, Eat eat)
        {
            if (id != eat.Id)
            {
                return BadRequest();
            }

            _context.Entry(eat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EatExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Eat>> PostEat(Eat eat)
        {
          if (_context.Eats == null)
          {
              return Problem("Entity set 'EatsContext.Eats'  is null.");
          }
            _context.Eats.Add(eat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEat", new { id = eat.Id }, eat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEat(int id)
        {
            if (_context.Eats == null)
            {
                return NotFound();
            }
            var eat = await _context.Eats.FindAsync(id);
            if (eat == null)
            {
                return NotFound();
            }

            _context.Eats.Remove(eat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EatExists(int id)
        {
            return (_context.Eats?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeriLogDemo_Net6.Model;

namespace SeriLogDemo_Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class t_agentController : ControllerBase
    {
        private readonly SeriLogContext _context;

        public t_agentController(SeriLogContext context)
        {
            _context = context;
        }

        // GET: api/t_agent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<t_agent>>> Gett_agent()
        {
            return await _context.t_agent.ToListAsync();
        }

        // GET: api/t_agent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<t_agent>> Gett_agent(Guid id)
        {
            var t_agent = await _context.t_agent.FindAsync(id);

            if (t_agent == null)
            {
                return NotFound();
            }

            return t_agent;
        }

        // PUT: api/t_agent/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putt_agent(Guid id, t_agent t_agent)
        {
            if (id != t_agent.id)
            {
                return BadRequest();
            }

            _context.Entry(t_agent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!t_agentExists(id))
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

        // POST: api/t_agent
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<t_agent>> Postt_agent(t_agent t_agent)
        {
            _context.t_agent.Add(t_agent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Gett_agent", new { id = t_agent.id }, t_agent);
        }

        // DELETE: api/t_agent/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletet_agent(Guid id)
        {
            var t_agent = await _context.t_agent.FindAsync(id);
            if (t_agent == null)
            {
                return NotFound();
            }

            _context.t_agent.Remove(t_agent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool t_agentExists(Guid id)
        {
            return _context.t_agent.Any(e => e.id == id);
        }
    }
}

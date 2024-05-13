using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameServer.Contexts;
using GameServer.Entities;
using System.Net.Mime;

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if ProducesConsumes
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
#endif
    public class UserLogonController : ControllerBase
    {
        private readonly InsigniaDBContext _context;

        public UserLogonController(InsigniaDBContext context)
        {
            _context = context;
        }

        // GET: api/UserLogon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLogon>>> GetUserLogons()
        {
          if (_context.UserLogons == null)
          {
              return NotFound();
          }
            return await _context.UserLogons.ToListAsync();
        }

        // GET: api/UserLogon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserLogon>> GetUserLogon(int id)
        {
          if (_context.UserLogons == null)
          {
              return NotFound();
          }
            var userLogon = await _context.UserLogons.FindAsync(id);

            if (userLogon == null)
            {
                return NotFound();
            }

            return userLogon;
        }

        // PUT: api/UserLogon/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserLogon(int id, UserLogon userLogon)
        {
            if (id != userLogon.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userLogon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserLogonExists(id))
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

        // POST: api/UserLogon
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserLogon>> PostUserLogon(UserLogon userLogon)
        {
          if (_context.UserLogons == null)
          {
              return Problem("Entity set 'InsigniaDBContext.UserLogons'  is null.");
          }
            _context.UserLogons.Add(userLogon);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserLogonExists(userLogon.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserLogon", new { id = userLogon.UserId }, userLogon);
        }

        // DELETE: api/UserLogon/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserLogon(int id)
        {
            if (_context.UserLogons == null)
            {
                return NotFound();
            }
            var userLogon = await _context.UserLogons.FindAsync(id);
            if (userLogon == null)
            {
                return NotFound();
            }

            _context.UserLogons.Remove(userLogon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserLogonExists(int id)
        {
            return (_context.UserLogons?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}

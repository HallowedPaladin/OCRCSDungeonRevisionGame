using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameServer.Contexts;
using GameServer.Entities;

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly InsigniaDBContext _context;

        public PasswordController(InsigniaDBContext context)
        {
            _context = context;
        }

        // GET: api/Password
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Password>>> GetPasswords()
        {
          if (_context.Passwords == null)
          {
              return NotFound();
          }
            return await _context.Passwords.ToListAsync();
        }

        // GET: api/Password/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Password>> GetPassword(int id)
        {
          if (_context.Passwords == null)
          {
              return NotFound();
          }
            var password = await _context.Passwords.FindAsync(id);

            if (password == null)
            {
                return NotFound();
            }

            return password;
        }

        // PUT: api/Password/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPassword(int id, Password password)
        {
            if (id != password.UserId)
            {
                return BadRequest();
            }

            _context.Entry(password).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PasswordExists(id))
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

        // POST: api/Password
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Password>> PostPassword(Password password)
        {
          if (_context.Passwords == null)
          {
              return Problem("Entity set 'InsigniaDBContext.Passwords'  is null.");
          }
            _context.Passwords.Add(password);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PasswordExists(password.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPassword", new { id = password.UserId }, password);
        }

        // DELETE: api/Password/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassword(int id)
        {
            if (_context.Passwords == null)
            {
                return NotFound();
            }
            var password = await _context.Passwords.FindAsync(id);
            if (password == null)
            {
                return NotFound();
            }

            _context.Passwords.Remove(password);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PasswordExists(int id)
        {
            return (_context.Passwords?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}

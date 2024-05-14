using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsigniaServer.Contexts;
using InsigniaServer.Entities;
using System.Net.Mime;

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if ProducesConsumes
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
#endif
    public class UserCredentialController : ControllerBase
    {
        private readonly InsigniaDBContext _context;

        public UserCredentialController(InsigniaDBContext context)
        {
            _context = context;
        }

        // GET: api/UserCredential
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCredential>>> GetUserCredentials()
        {
            if (_context.UserCredentials == null)
            {
                return NotFound();
            }
            return await _context.UserCredentials.ToListAsync();
        }

        // GET: api/UserCredential/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserCredential>> GetUserCredential(int id)
        {
            if (_context.UserCredentials == null)
            {
                return NotFound();
            }
            var userCredential = await _context.UserCredentials.FindAsync(id);

            if (userCredential == null)
            {
                return NotFound();
            }

            return userCredential;
        }

        // PUT: api/UserCredential/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserCredential(int id, UserCredential userCredential)
        {
            if (id != userCredential.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userCredential).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCredentialExists(id))
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

        // POST: api/UserCredential
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserCredential>> PostUserCredential(UserCredential userCredential)
        {
            if (_context.UserCredentials == null)
            {
                return Problem("Entity set 'InsigniaDBContext.UserCredentials'  is null.");
            }
            _context.UserCredentials.Add(userCredential);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserCredentialExists(userCredential.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserCredential", new { id = userCredential.UserId }, userCredential);
        }

        // DELETE: api/UserCredential/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCredential(int id)
        {
            if (_context.UserCredentials == null)
            {
                return NotFound();
            }
            var userCredential = await _context.UserCredentials.FindAsync(id);
            if (userCredential == null)
            {
                return NotFound();
            }

            _context.UserCredentials.Remove(userCredential);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserCredentialExists(int id)
        {
            return (_context.UserCredentials?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsigniaServer.Contexts;
using InsigniaServer.Entities;

namespace InsigniaServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if ProducesConsumes
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
#endif
    public class UserTypeController : ControllerBase
    {
        private readonly InsigniaDBContext _context;

        public UserTypeController(InsigniaDBContext context)
        {
            _context = context;
        }

        // GET: api/UserType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserType>>> GetUserTypes()
        {
            if (_context.UserTypes == null)
            {
                return NotFound();
            }
            return await _context.UserTypes.ToListAsync();
        }

        // GET: api/UserType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserType>> GetUserType(int id)
        {
            if (_context.UserTypes == null)
            {
                return NotFound();
            }
            var userType = await _context.UserTypes.FindAsync(id);

            if (userType == null)
            {
                return NotFound();
            }

            return userType;
        }

        // PUT: api/UserType/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserType(int id, UserType userType)
        {
            if (id != userType.UserTypeId)
            {
                return BadRequest();
            }

            _context.Entry(userType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTypeExists(id))
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

        // POST: api/UserType
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserType>> PostUserType(UserType userType)
        {
            if (_context.UserTypes == null)
            {
                return Problem("Entity set 'InsigniaDBContext.UserTypes'  is null.");
            }
            _context.UserTypes.Add(userType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserType", new { id = userType.UserTypeId }, userType);
        }

        // DELETE: api/UserType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserType(int id)
        {
            if (_context.UserTypes == null)
            {
                return NotFound();
            }
            var userType = await _context.UserTypes.FindAsync(id);
            if (userType == null)
            {
                return NotFound();
            }

            _context.UserTypes.Remove(userType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTypeExists(int id)
        {
            return (_context.UserTypes?.Any(e => e.UserTypeId == id)).GetValueOrDefault();
        }
    }
}

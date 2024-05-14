using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsigniaServer.Contexts;
using InsigniaServer.DTO;
using InsigniaServer.EntityHelpers;
using Microsoft.AspNetCore.Authorization;

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if ProducesConsumes
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
#endif
    public class UserController : ControllerBase
    {
        private readonly InsigniaDBContext _context;

        public UserController(InsigniaDBContext context)
        {
            _context = context;
        }

        // GET: api/User/GetUsers
        [HttpGet("GetUsers")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var List = await _context.Users.Select(
            s => new UserDTO
            {
                UserId = s.UserId,
                UserName = s.UserName,
                FirstName = s.FirstName,
                FamilyName = s.FamilyName,
                DateOfBirth = s.DateOfBirth,
                Email = s.Email,
                PhoneCountryCode = s.PhoneCountryCode,
                PhoneNumber = s.PhoneNumber,
                RegistrationDate = s.RegistrationDate,
                Timestamp = s.Timestamp,
                UserTypeId = s.UserTypeId
            }
           ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }

        }

        // GET: api/User/GetUser/5
        [HttpGet("GetUser/{userId}")]
        public async Task<ActionResult<UserDTO>> GetUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                UserDTO userDTO = new UserDTO();
                userDTO.UserId = user.UserId;
                userDTO.UserName = user.UserName;
                userDTO.FirstName = user.FirstName;
                userDTO.FamilyName = user.FamilyName;
                userDTO.DateOfBirth = user.DateOfBirth;
                userDTO.Email = user.Email;
                userDTO.PhoneCountryCode = user.PhoneCountryCode;
                userDTO.PhoneNumber = user.PhoneNumber;
                userDTO.RegistrationDate = user.RegistrationDate;
                userDTO.UserTypeId = user.UserTypeId;
                userDTO.Timestamp = user.Timestamp;
                return userDTO;
            }
        }

        // PUT: api/User/PutUserDTO/5
        [HttpPut("PutUser/{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO userDTO)
        {
            if (id != userDTO.UserId)
            {
                return BadRequest();
            }

            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.UserId = userDTO.UserId;
            user.UserName = userDTO.UserName;
            user.FirstName = userDTO.FirstName;
            user.FamilyName = userDTO.FamilyName;
            user.DateOfBirth = userDTO.DateOfBirth;
            user.Email = userDTO.Email;
            user.PhoneCountryCode = userDTO.PhoneCountryCode;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.RegistrationDate = userDTO.RegistrationDate;
            user.Timestamp = userDTO.Timestamp;
            user.UserTypeId = userDTO.UserTypeId;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/User/PostUser
        [HttpPost("PostUser")]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO)
        {
            var userHelper = new UserHelper(_context);

            try
            {
                UserDTO newUserDTO = userHelper.createUser(userDTO);
                return CreatedAtAction(nameof(GetUser), new { userId = newUserDTO.UserId }, newUserDTO);
            }
            catch (Exception e)
            {
                // TODO improve information in the error response
                return BadRequest();
            }
        }

        // DELETE: api/User/DeleteUser/5
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}

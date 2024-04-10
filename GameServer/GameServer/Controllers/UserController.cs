using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameServer.Contexts;
using GameServer.Entities;
using GameServer.DTO;

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly InsigniaDBContext _context;

        public UserController(InsigniaDBContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/GetUsersDTO
        [HttpGet("GetUsersDTO")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersDTO()
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
        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
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

            return user;
        }

        // GET: api/User/GetUserDTO/5
        [HttpGet("GetUserDTO/{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserDTO(int userId)
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
                return userDTO;
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

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

        // PUT: api/User/PutUserDTO/5
        [HttpPut("PutUserDTO/{id}")]
        public async Task<IActionResult> PutUserDTO(int id, UserDTO userDTO)
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

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'InsigniaDBContext.Users'  is null.");
          }
            _context.Users.Add(user);
            //_context.Entry(user.UserType).State = EntityState.Unchanged;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // POST: api/User/PostUserDTO
        [HttpPost("PostUserDTO")]
        public async Task<ActionResult<UserDTO>> PostUserDTO(UserDTO userDTO)
        {
            var user = new User()
            {
                UserName = userDTO.UserName,
                FirstName = userDTO.FirstName,
                FamilyName = userDTO.FamilyName,
                DateOfBirth = userDTO.DateOfBirth,
                Email = userDTO.Email,
                PhoneCountryCode = userDTO.PhoneCountryCode,
                PhoneNumber = userDTO.PhoneNumber,
                RegistrationDate = userDTO.RegistrationDate,
                UserTypeId = userDTO.UserTypeId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserDTO), new { userId = user.UserId }, userDTO);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
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

        // DELETE: api/User/DeleteUserDTO/5
        [HttpDelete("DeleteUserDTO/{id}")]
        public async Task<IActionResult> DeleteUserDTO(int id)
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

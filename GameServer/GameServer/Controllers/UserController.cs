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
using GameServer.EntityHelpers;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using GameServer.Auth;
using System.Security.Principal;
using System.Security.Claims;

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
        private readonly TokenUtility _tokenUtility;

        public UserController(InsigniaDBContext context, TokenUtility tokenUtility)
        {
            _context = context;
            _tokenUtility = tokenUtility;

        }

        // GET: api/User/GetUsersDTO
        [HttpGet("GetUsersDTO")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersDTO()
        {

            // Get the token from the request
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Call the token validation method
            ClaimsPrincipal principal = _tokenUtility.ValidateToken(token);

            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                return Unauthorized(); // Return 401 Unauthorized if token is invalid
            }

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

        // GET: api/User/GetUserDTO/5
        [HttpGet("GetUserDTO/{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserDTO(int userId)
        {
            // Get the token from the request
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Call the token validation method
            ClaimsPrincipal principal = _tokenUtility.ValidateToken(token);

            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                return Unauthorized(); // Return 401 Unauthorized if token is invalid
            }

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

        // POST: api/User/PostUserDTO
        [HttpPost("PostUserDTO")]
        public async Task<ActionResult<UserDTO>> PostUserDTO(UserDTO userDTO)
        {
            var userHelper = new UserHelper(_context);

            try
            {
                UserDTO newUserDTO = userHelper.createUser(userDTO);
                return CreatedAtAction(nameof(GetUserDTO), new { userId = newUserDTO.UserId }, newUserDTO);
            }
            catch (Exception e)
            {
                // TODO improve information in the error response
                return BadRequest();
            }
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

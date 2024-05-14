using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsigniaServer.Contexts;
using InsigniaServer.Entities;
using InsigniaServer.DTO;
using InsigniaServer.EntityHelpers;
using InsigniaServer.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if ProducesConsumes
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
#endif
    public class AuthenticationController : ControllerBase
    {
        private readonly InsigniaDBContext _context;
        private readonly TokenUtility _tokenUtility;

        public AuthenticationController(InsigniaDBContext context, TokenUtility tokenUtility)
        {
            _context = context;
            _tokenUtility = tokenUtility;
        }

        // Post: api/Authentication
        [HttpPost("Logon")]
        public async Task<ActionResult<UserDTO>> Logon(UserCredentialsDTO credentialsDTO)
        {
            UserHelper userHelper = new UserHelper(_context);
            UserDTO userDTO;

            try
            {
                userDTO = userHelper.LogonUser(credentialsDTO);
                if (userDTO.UserId != 0)
                {
                    // Generate a new JWT
                    var token = _tokenUtility.GenerateToken(userDTO.UserId);

                    Response.Headers.Add("Authorization", "Bearer " + token);
                    return Ok(userDTO);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // Post: api/Authentication
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegistrationDTO registrationDTO)
        {
            // Ensure we have adequate data in the userDTO object

            // Create the user
            UserHelper userHelper = new UserHelper(_context);
            UserDTO newUserDTO;

            try
            {
                newUserDTO = userHelper.createUser(registrationDTO.userDTO);
            }
            catch (Exception e)
            {
                // TODO improve information in the error response
                return BadRequest(e.Message);
            }

            try
            {
                // Store the password
                userHelper.savePassword(newUserDTO.UserId, registrationDTO.credentialsDTO.Password);
            }
            catch (Exception e)
            {
                // TODO improve information in the error response
                return BadRequest(e.Message);
            }

            // Verify email
            // Verify phone

            return Ok(newUserDTO);
        }

        // Post: api/Authentication
        [HttpPost("CreateLogon")]
        public async Task<ActionResult<UserLogon>> CreateLogon(UserLogonDTO userLogonDTO)
        {
            var userLogon = new UserLogon();

            userLogon.UserId = userLogonDTO.UserId;
            userLogon.LastLogonDate = DateTime.Now;
            userLogon.IsLocked = 0;
            userLogon.LogonAttempts = 0;
            _context.UserLogons.Add(userLogon);
            try
            {
                _context.SaveChanges();
                return Ok(userLogon);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }
        }
    }
}
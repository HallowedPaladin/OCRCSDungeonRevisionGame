using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameServer.Contexts;
using GameServer.Entities;
using GameServer.DTO;
using GameServer.EntityHelpers;
using System.Net.Mime;
using NuGet.Common;
using GameServer.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
//#if ProducesConsumes
//    [Produces(MediaTypeNames.Application.Json)]
//    [Consumes(MediaTypeNames.Application.Json)]
//#endif
    public class AuthenticationController : ControllerBase
    {
        private readonly InsigniaDBContext _context;
        private readonly TokenUtility _tokenUtility;
        private IConfiguration _config;

        //public AuthenticationController(InsigniaDBContext context, TokenUtility tokenUtility, IConfiguration config)
        public AuthenticationController(InsigniaDBContext context, IConfiguration config)
        {
            _context = context;
           //_tokenUtility = tokenUtility;
            _config = config;
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
                    // TODO need to make a singleton
                    //var token = _tokenUtility.GenerateToken(userDTO.UserId);

                    var jwtSecrretString = _config["Jwt:Secret"];
                    var jwtIssuer = _config["Jwt:Issuer"];
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecrretString));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    

                    var Sectoken = new JwtSecurityToken(jwtIssuer,
                      jwtIssuer,
                      null,
                      expires: DateTime.Now.AddMinutes(120),
                      signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

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
        [HttpPost("CreatLogon")]
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
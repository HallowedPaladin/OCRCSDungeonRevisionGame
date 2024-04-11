using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameServer.Contexts;
using GameServer.Entities;
using GameServer.DTO;
using GameServer.EntityHelpers;

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly InsigniaDBContext _context;

        public AuthenticationController(InsigniaDBContext context)
        {
            _context = context;
        }

        // Post: api/Authentication
        [HttpPost("Logon")]
        public async Task<ActionResult<UserDTO>> Logon(CredentialsDTO logonDTO)
        {
            UserDTO userDTO = new UserDTO();
            bool error = true;

            // Only validate if we have a UserName and a Password
            if (logonDTO.UserName != null && logonDTO.Password != null)
            {
                // Read the User record
                //var user = await _context.Users.FindAsync(logonDTO.UserName);
                //var user = from u in _context.Users where u.UserName.Equals(logonDTO.UserName) select u;
                User user = _context.Users.SingleOrDefault(user => user.UserName == logonDTO.UserName);

                if (user != null)
                {
                    // Read the Logon record and make sure the account is not locked
                    var userLogon = await _context.UserLogons.FindAsync(user.UserId);
                    if (userLogon == null)
                    {
                        // First time so create the logon record
                        userLogon = new UserLogon();
                        userLogon.UserId = user.UserId;
                        userLogon.IsLocked = 0;
                    }
                    if (userLogon.IsLocked == 0)
                    {
                        // The account is not locked
                        // Proceed with password validation
                        // Read the password record
                        var passwordRecord = await _context.Passwords.FindAsync(user.UserId);

                        if (passwordRecord != null)
                        {
                            // Compare passwords for a match
                            // TODO hash the passwords
                            if (logonDTO.Password.Equals(passwordRecord.PasswordHash))
                            {
                                // The passwords match!
                                // Reset the logon count and save the record
                                userLogon.LogonAttempts = 0;
                                _context.Entry(userLogon).State = EntityState.Modified;
                                try
                                {
                                    await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                    // Somebody was trying to logon in parallel
                                    // TODO set the correct retun code
                                }
                                // Success!

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
                                error = false;
                            }
                            else
                            {
                                // The password comparison failed
                                // Update the number of attempted logons
                                userLogon.LogonAttempts = userLogon.LogonAttempts + 1;
                                // Check maximum nuber of logon attmpts and lock the account
                                if (userLogon.LogonAttempts >= 3)
                                {
                                    userLogon.IsLocked = 1;
                                }
                                // Save the logon attmpts record
                                _context.Entry(userLogon).State = EntityState.Modified;
                                try
                                {
                                    await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                    // Somebody was trying to logon in parallel
                                    // TODO set the correct retun code
                                }
                            }
                        }

                    }
                }
            }

            if (error)
            {
                return NotFound();
            }
            else
            {
                return userDTO;
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

            return newUserDTO;
        }
    }
}
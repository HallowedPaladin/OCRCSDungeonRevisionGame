using System;
using GameServer.Contexts;
using GameServer.DTO;
using GameServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameServer.EntityHelpers
{
    using BCrypt.Net;

    public class UserHelper
    {
        private readonly InsigniaDBContext _context;

        public UserHelper(InsigniaDBContext context)
        {
            _context = context;
        }

        public UserDTO createUser(UserDTO userDTO)
        {
            if (userDTO.UserName != null && userDTO.UserName.Length > 0)
            {
                // First check the username is not already in use
                User existingUser = _context.Users.SingleOrDefault(user => user.UserName == userDTO.UserName);

                if (existingUser == null)
                {
                    var newUser = new User()
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

                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    userDTO.UserId = newUser.UserId;
                    userDTO.Timestamp = newUser.Timestamp;

                }
                else
                {
                    // TODO Use correct return code
                    throw new Exception("Create User failed");
                }
            }
            else
            {
                // TODO Use correct return code
                throw new Exception("Create User failed");
            }

            //return CreatedAtAction(nameof(GetUserDTO), new { userId = newUser.UserId }, userDTO);
            return userDTO;
        }

        public UserDTO LogonUser(UserCredentialsDTO credentialsDTO)
        {
            UserDTO userDTO = new UserDTO();
            UserHelper userHelper = new UserHelper(_context);
           

            // Only validate if we have a UserName and a Password
            if (credentialsDTO.UserName != null && credentialsDTO.Password != null)
            {
                // Read the User record
                //var user = await _context.Users.FindAsync(logonDTO.UserName);
                //var user = from u in _context.Users where u.UserName.Equals(logonDTO.UserName) select u;
                User user = _context.Users.SingleOrDefault(user => user.UserName == credentialsDTO.UserName);

                if (user != null)
                {
                    // Read the Logon record and make sure the account is not locked
                    var userLogon = _context.UserLogons.Find(user.UserId);
                    if (userLogon == null)
                    {
                        // First time so create the logon record
                        userLogon = new UserLogon();
                        userLogon.UserId = user.UserId;
                        userLogon.LastLogonDate = DateTime.Now;
                        userLogon.IsLocked = 0;
                        userLogon.LogonAttempts = 0;
                        _context.UserLogons.Add(userLogon);
                        //_context.Entry(userLogon).State = EntityState.Modified;
                        _context.SaveChanges();
                    }

                    // Only erform logon if the user record is not locked
                    if (userLogon.IsLocked == 0)
                    {
                        // The account is not locked
                        // Proceed with password validation
                        // Read the password record
                        var passwordRecord = _context.UserCredentials.Find(user.UserId);

                        if (passwordRecord != null)
                        {
                            // Compare passwords for a match
                            if (userHelper.ValidatePassword(credentialsDTO.Password, passwordRecord.PasswordHash))
                            {
                                // The passwords match!
                                // Reset the logon count and save the record
                                userLogon.LogonAttempts = 0;
                                userLogon.LastLogonDate = new DateTime();
                                _context.Entry(userLogon).State = EntityState.Modified;
                                try
                                {
                                    _context.SaveChanges();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                    // Somebody was trying to logon in parallel
                                    // TODO set the correct retun code
                                }
                                // Success!

                                userDTO = PopulateUserDTO(user, userDTO);
;                            }
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
                                // _context.Entry(userLogon).State = EntityState.Modified;
                                try
                                {
                                    _context.SaveChanges();
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
            return userDTO;
        }

        public void savePassword(int userID, string passwordString)
        {
            if (ValidatePassword(passwordString))
            {
                String passwordHash = DoHash(passwordString);

                var userCredential = new UserCredential();
                userCredential.UserId = userID;
                userCredential.PasswordHash = passwordHash;

                _context.UserCredentials.Add(userCredential);

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    //if (PasswordExists(password.UserId))
                    //{
                    //    throw new Exception("Conflict");
                    //}
                    //else
                    //{
                        throw;
                    //}
                }
            }
            else
            {
                throw new Exception("Bad password");
            }
        }

        public bool ValidatePassword(String plaintext, String hashedText)
        {
            bool valid = false;

            if (plaintext != null && hashedText != null)
            {
                if (BCrypt.Verify(plaintext, hashedText))
                {
                    valid = true;
                }
            }

            return valid;
        }

        private bool PasswordExists(int id)
        {
            return (_context.UserCredentials?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        private Boolean ValidatePassword(string password)
        {
            bool isValid = true;
            return isValid;
        }

        private String DoHash(string password)
        {
            string hashedPassword = BCrypt.HashPassword(password);
            return hashedPassword;
        }

        private UserDTO PopulateUserDTO(User user, UserDTO userDTO)
        {
            userDTO.DateOfBirth = user.DateOfBirth;
            userDTO.Email = user.Email;
            userDTO.FamilyName = user.FamilyName;
            userDTO.FirstName = user.FirstName;
            userDTO.IsEmailVerified = user.IsEmailVerified;
            userDTO.IsPhoneVerified = user.IsPhoneVerified;
            userDTO.PhoneCountryCode = user.PhoneCountryCode;
            userDTO.PhoneNumber = user.PhoneNumber;
            userDTO.RegistrationDate = user.RegistrationDate;
            userDTO.Timestamp = user.Timestamp;
            userDTO.UserId = user.UserId;
            userDTO.UserName = user.UserName;
            userDTO.UserTypeId = user.UserTypeId;

            return userDTO;
        }
    }
}
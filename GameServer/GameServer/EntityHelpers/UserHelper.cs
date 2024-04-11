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

        public void savePassword(int userID, string passwordString)
        {
            if (ValidatePassword(passwordString))
            {
                String passwordHash = HashPassword(passwordString);

                var password = new Password();
                password.UserId = userID;
                password.PasswordHash = passwordHash;

                _context.Passwords.Add(password);

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

        private bool PasswordExists(int id)
        {
            return (_context.Passwords?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        private Boolean ValidatePassword(string password)
        {
            bool isValid = true;
            return isValid;
        }

        private String HashPassword(string password)
        {
            string hashedPassword = BCrypt.HashPassword(password);
            return hashedPassword;
        }
    }
}
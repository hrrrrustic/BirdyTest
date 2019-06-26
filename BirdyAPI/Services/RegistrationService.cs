﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Answers;
using BirdyAPI.Models;

namespace BirdyAPI.Services
{
    public class RegistrationService
    {
        private readonly UserContext _context;

        public RegistrationService(UserContext context)
        {
            _context = context;
        }

        public RegistrationAnswer CreateNewAccount(User user)
        {
            if (_context.Users?.FirstOrDefault(k => k.Email == user.Email && k.PasswordHash == user.PasswordHash) != null)
                return new RegistrationAnswer {ErrorMessage = "Duplicate account"};
            user.Token = new Random().Next(int.MaxValue / 2, int.MaxValue);
            _context.Add(user);
            _context.SaveChanges();
            return new RegistrationAnswer { FirstName = user.FirstName};
        }
    }
}

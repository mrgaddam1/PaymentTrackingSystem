using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Web.Infrastructure.Implementation
{
    public class AuthManager(PTSContext _DbContext) : IAuthManager
    {
        private readonly ILogger<AuthManager> logger;
        private PTSContext DbContext { get; set; } = _DbContext;

        public async Task<UserViewModel?> AuthenticateUser(string emailId, string password)
        {
            var userViewModel = new UserViewModel();
            var user = await DbContext.Users.FirstOrDefaultAsync(u => u.EmailId == emailId);
            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword("Demo@523274");
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return userViewModel = null;
            }
            else
            {
                userViewModel.UserName = user.FirstName + " " + user.LastName;
                userViewModel.UserId = user.UserId;
                userViewModel.Email = user.EmailId;
                userViewModel.PasswordHash = user.Password;
                userViewModel.Role = null;
            }

            return userViewModel;
        }

        public async Task<bool> RegisterUser(string emailId, string password)
        {
            var existingUser = await DbContext.Users.FirstOrDefaultAsync(u => u.EmailId == emailId);
            if (existingUser != null) return false; // User already exists

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User { EmailId = emailId, Password = hashedPassword };
            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();
            return true;
        }
    }
}

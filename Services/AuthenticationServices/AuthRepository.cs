using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace dotnet_rpg.Services.AuthenticationServices
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;            
        }

        public Task<ServiceResponse<string>> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if(await UserExist(user.Username))
            {
                response.Succes = false;
                response.Message = "User already exist.";
                return response;
            }
            var encryptedPassword = CreatePasswordHash(password);
            user.PasswordHash = encryptedPassword.Hash;
            user.PasswordSalt = encryptedPassword.Salt;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();            
            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExist(string username)
        {
            if(await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        private (byte[] Hash, byte[] Salt) CreatePasswordHash(string password)
        {
            (byte[] Hash, byte[] Salt) encryptedPassword;
            using (var hmac = new HMACSHA512())
            {                
                encryptedPassword.Hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                encryptedPassword.Salt = hmac.Key;
            }
            return encryptedPassword;
        }

    }
}
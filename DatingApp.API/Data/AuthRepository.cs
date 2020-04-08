using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Data.Interfaces;

namespace DatingApp.API.Data {

    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            username = username.ToLower();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!_verifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            _createPasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }
        
        private void _createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
       
        private bool _verifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            var valid = false;

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                valid = computeHash.SequenceEqual(passwordHash);
            }

            return valid;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using DatingApp.API.DatingApp;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public static class Seed
    {
        public static void SeedUsers(DataContext context)
        {
            if (context.Users.Any())
                return;

            var data = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(data);

            _createPasswordHash("password", out byte[] passwordHash, out byte[] passwordSalt);

            foreach (var user in users)
            {
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();
                context.Users.Add(user);
            }

            context.SaveChanges();            
        }

        private static void _createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
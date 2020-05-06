using DatingApp.API.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly DataContext _contex;

        public Seed(DataContext contex){
            _contex = contex;
        }

        public void SeedUser()
        {
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach(var user in users)
            {
                byte[] passwordHash, passwordSalt;
                CreatPasswordHash("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                _contex.Users.Add(user);
            }

            _contex.SaveChanges();
        }

        private void CreatPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
    }
}

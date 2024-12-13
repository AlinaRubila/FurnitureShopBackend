using FurnitureShop.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace FurnitureShop
{
    public class UserDB
    {
        Model.ApplicationContext _database = new Model.ApplicationContext();
        static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }
        static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }
        Contracts.Token CreateToken(string name, int id, string role)
        {
            string s_id = id.ToString();
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, name), new Claim(ClaimTypes.Role, role), new Claim(ClaimTypes.NameIdentifier, s_id) };
            JwtSecurityToken token = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER, audience: AuthOptions.AUDIENCE, claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            Contracts.Token t = new Contracts.Token();
            t.token = new JwtSecurityTokenHandler().WriteToken(token);
            return t;
        }
        public Contracts.Token? Register(string name, string login, string password)
        {
            User? try_user = _database.Users.FirstOrDefault(u => u.Login == login);
            if (try_user != null) { return null; }
            string hashed_pass = HashPassword(password);
            User user = new User();
            Role? role = _database.Roles.Where(r => r.ID == 3).FirstOrDefault();
            if (role == null) { return null; }
            user.Name = name; user.Login = login; user.Password = hashed_pass; user.Role = role;
            _database.Users.Add(user);
            _database.SaveChanges();
            User? user1 = _database.Users.FirstOrDefault(u => u.Login == login);
            int id = 0;
            if (user1 != null) { id = user1.ID; }
            Contracts.Token t = CreateToken(user.Name, id, user.Role.Name);
            return t;
        }
        public Contracts.Token? Login(string login, string password)
        {
            User? try_user = _database.Users.Include(user => user.Role).Where(u => u.Login == login).FirstOrDefault();
            if (try_user == null) {  return null; }
            string hashed_pass = try_user.Password;
            bool r = VerifyHashedPassword(hashed_pass, password);
            if (r == false) { return null;  }
            Contracts.Token t = CreateToken(try_user.Name, try_user.ID, try_user.Role.Name);
            return t;
        }
        public Contracts.UserInfo GetUserInfo(int id)
        {
            User? user = _database.Users.Include(user => user.Role).FirstOrDefault(u => u.ID == id) ?? new User();
            Contracts.UserInfo info = new Contracts.UserInfo();
            Contracts.UserRole role = new Contracts.UserRole();
            role.ID = user.Role.ID; role.name = user.Role.Name;
            info.name = user.Name; info.role = role;
            return info;
        }
        public List<Contracts.UserInfo> GetAllUsers()
        {
            var users = _database.Users.Include(user => user.Role).ToList();
            List<Contracts.UserInfo> infos = new List<Contracts.UserInfo>();
            foreach (var user in users)
            {
                Contracts.UserInfo info = new Contracts.UserInfo();
                Contracts.UserRole role = new Contracts.UserRole();
                role.ID = user.Role.ID; role.name = user.Role.Name;
                info.name = user.Name; info.role = role;
                infos.Add(info);
            }
            return infos;
        }
    }
}

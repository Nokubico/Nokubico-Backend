using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nokubico.Domain.Account;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Enums;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Identity
{
    public class AuthenticateServices : IAuthenticate
    {
        private const string EmailProvider = "email";

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticateServices(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public bool UserExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public User? Register(string name, string email, string password)
        {
            if (UserExists(email))
            {
                return null;
            }

            var user = new User(email, name, UserRole.User);
            var account = new Account(email, EmailProvider, user);
            account.SetPassword(PasswordHasher.Hash(password));
            var wallet = new Wallet(user.Id, "KZ");

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Users.Add(user);
                _context.Accounts.Add(account);
                _context.Wallets.Add(wallet);
                _context.SaveChanges();
                transaction.Commit();
                return user;
            }
            catch
            {
                transaction.Rollback();
                return null;
            }
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool Authenticate(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }

            var account = _context.Accounts.FirstOrDefault(a => a.UserId == user.Id && a.ProviderId == EmailProvider);
            if (account == null || string.IsNullOrEmpty(account.Password))
            {
                return false;
            }

            return PasswordHasher.Verify(password, account.Password);
        }

        public string GenerateToken(Guid id, string email, UserRole role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? string.Empty));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(50);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
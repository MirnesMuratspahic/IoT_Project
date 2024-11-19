using AlarmSystem.Context;
using AlarmSystem.Models;
using AlarmSystem.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AlarmSystem.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AlarmSystem.Services
{
    public class UserService : IUserService
    {
        public ApplicationDbContext DbContext { get; set; }
        public IConfiguration Configuration { get; set; }
        public ErrorProvider error = new ErrorProvider() { Status = false };
        public ErrorProvider defaultError = new ErrorProvider() { Status = true, Name = "The property must not be null" };

        public UserService(ApplicationDbContext _context, IConfiguration _configuration) 
        {
            DbContext = _context;
            Configuration = _configuration;
        }

        public async Task<ErrorProvider> UserRegistration(dtoUserRegistration user)
        {
            if (user == null)
                return defaultError;

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == user.Email.ToLower());

            if (userFromDatabase != null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "There is already user with such email adress"
                };
                return error;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashString(user.Password);

            var newUser = new User()
            {
                Email = user.Email.ToLower(),
                PasswordHash = passwordHash,
                FirstName = user.FirstName.ToLower(),
                LastName = user.LastName.ToLower(),
                PhoneNumber = user.PhoneNumber,
                Status = "Active",
                Role = user.Role,
            };

            await DbContext.Users.AddAsync(newUser);
            await DbContext.SaveChangesAsync();
            var token = CreateToken(newUser);

            error = new ErrorProvider()
            {
                Status = false,
                Name = "User registered!"
            };

            return error;

        }

        public async Task<(ErrorProvider, string)> UserLogin(dtoUserLogin user)
        {
            if (user == null)
                return (defaultError, null);

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == user.Email.ToLower());

            if (userFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "The data that you have entered is incorrect."
                };
                return (error, null);
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, userFromDatabase.PasswordHash);
            if (!isPasswordValid)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "The data that you have entered is incorrect."
                };
                return (error, null);
            }

            var token = CreateToken(userFromDatabase);

            error = new ErrorProvider()
            {
                Status = false,
                Name = "Login successful!",
            };

            return (error, token);
        }


        private string CreateToken(User user)
        {
            List<Claim> _claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("Role", user.Role),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    claims: _claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }


        public async Task<(ErrorProvider, User)> GetUserInformations(string email)
        {
            if (string.IsNullOrEmpty(email))
                return (defaultError, null);

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if(userFromDatabase == null)
            {
                error = new ErrorProvider
                {
                    Status = true,
                    Name = "There is no user with same email adress!"
                };
                return (error, null);
            }

            return (error, userFromDatabase);


        }

        public async Task<ErrorProvider> UpdateUserInformations(dtoUserInformations dtoUserInformations)
        {
            if(dtoUserInformations == null)
            {
                return defaultError;
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == dtoUserInformations.Email);
            if(userFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "User not found."
                };
                return error;
            }

            userFromDatabase.FirstName = dtoUserInformations.FirstName != null ? dtoUserInformations.FirstName : userFromDatabase.FirstName;
            userFromDatabase.LastName = dtoUserInformations.LastName != null ? dtoUserInformations.LastName : userFromDatabase.LastName;    
            userFromDatabase.PhoneNumber = dtoUserInformations.PhoneNumber != null ? dtoUserInformations.PhoneNumber : userFromDatabase.PhoneNumber;
            userFromDatabase.ImageUrl = dtoUserInformations.ImageUrl != null ? dtoUserInformations.ImageUrl : userFromDatabase.ImageUrl;

            DbContext.Users.Update(userFromDatabase);
            await DbContext.SaveChangesAsync();
            error = new ErrorProvider()
            {
                Status = false,
                Name = "User's informations successfully changed!"
            };
            return(error);

        }
    }
}

using Microsoft.IdentityModel.Tokens;
using MinimalJwt.Models;
using MinimalJwt.Repositories;
using MinimalJwt.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalJwt.Controllers
{
    public class UserController : IUserService
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public User Get(UserLogin userLogin)
        {
            
                User user = UserRepository.Users.FirstOrDefault(o => o.Username.Equals
                (userLogin.Username, StringComparison.OrdinalIgnoreCase) && o.Password.Equals(userLogin.Password));
                return user;

            

        }
        IResult Login(UserLogin user, WebApplicationBuilder builder)
        {
            if (!string.IsNullOrEmpty(user.Username) &&
                !string.IsNullOrEmpty(user.Password))
            {
                var loggedInUser = userService.Get(user);

                if (loggedInUser is null) return Results.NotFound("User not found");

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
                    new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
                    new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
                    new Claim(ClaimTypes.Surname, loggedInUser.Surname),
                    new Claim(ClaimTypes.Role, loggedInUser.Role)
                };

                var token = new JwtSecurityToken
                (
                    issuer: builder.Configuration["Jwt:Issuer"],
                    audience: builder.Configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(60),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256)
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(tokenString);
            }
            return Results.BadRequest("Invalid user credentials");
        }

    }
}

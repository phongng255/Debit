using AutoMapper;
using Debit.DTOs;
using Debit.Entities;
using Debit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Debit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public AuthController(AppDbContext dbContext, IConfiguration configuration, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<JWT>> Token(Login login)
        {
            // Verify account
            if (dbContext.Users != null)
            {
                var user = dbContext.Users.FirstOrDefault(x => x.PhoneNumber == login.PhoneNumber);
                if (user == null)
                {
                    return BadRequest(new { message = "Số Điện Thoại không tồn tại!" });
                }
                else
                {
                    if (user.PassworhHash != null)
                    {
                        bool verified = VerifyPassword(login.Password, user.PassworhHash);
                        if (verified == false)
                        {
                            return BadRequest(new { message = "Mật khẩu không khớp!" });
                        }
                        var accessToken = await GetAccessToken(user.Id);
                        var refreshToken = await GetRefreshToken(user.Id);
                        JWT tokenResult = new JWT(
                             new JwtSecurityTokenHandler().WriteToken(accessToken),
                             new JwtSecurityTokenHandler().WriteToken(refreshToken)
                         );
                        return Ok(tokenResult);
                    }
                }
            }
            return BadRequest(new { message = "Lỗi kết nối Sever!" });
        }

        // Check verify Password
        private bool VerifyPassword(string password, string passwordHash)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(password, passwordHash);
            if (verified == true)
            {
                return true;
            }
            return false;
        }

        // Get AccessToken
        private Task<JwtSecurityToken> GetAccessToken(Guid id)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("Id", id.ToString()));

            var accessTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTConfig:AccessTokenSecret"]));
            var accessTokenCredentials = new SigningCredentials(accessTokenKey, SecurityAlgorithms.HmacSha256Signature);

            var accessToken = new JwtSecurityToken(
                issuer: configuration["JWTConfig:Issuer"],
                audience: configuration["JWTConfig:Audience"],
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: accessTokenCredentials,
                claims: claims
            );
            return Task.FromResult(accessToken);
        }

        // Get RefeshToken
        private Task<JwtSecurityToken> GetRefreshToken(Guid id)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("Id", id.ToString()));

            var refreshTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTConfig:RefreshTokenSecret"]));
            var refreshTokenCredentials = new SigningCredentials(refreshTokenKey, SecurityAlgorithms.HmacSha256Signature);

            var refreshToken = new JwtSecurityToken(
                issuer: configuration["JWTConfig:Issuer"],
                audience: configuration["JWTConfig:Audience"],
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: refreshTokenCredentials,
                claims: claims
            );
            return Task.FromResult(refreshToken);
        }
        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<UserDTO>> Me()
        {
            Guid userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            //Get user
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest();
            }
            var userDTO = mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }
        protected Guid GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            Guid id = new Guid(userId);
            return id;
        }

        [HttpPost("[Action]")]
        public async Task<ActionResult<JWT>> Refresh(JWT jwt)
        {
            Guid? id = ValidateRefreshToken(jwt.RefreshToken);
            if (id != null)
            {
                var accessToken = await GetAccessToken((Guid)id);

                JWT tokenResult = new JWT(
                    new JwtSecurityTokenHandler().WriteToken(accessToken),
                    jwt.RefreshToken
                );
                return Ok(tokenResult);
            }
            return Unauthorized();
        }
        private Guid? ValidateRefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["JWTConfig:RefreshTokenSecret"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWTConfig:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWTConfig:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                string id = jwtToken.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                return new Guid(id);
            }
            catch
            {
                return null;
            }
        }
    }

}


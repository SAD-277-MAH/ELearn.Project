using ELearn.Common.Helpers.Interface;
using ELearn.Common.Utilities;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Tokens;
using ELearn.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Common.Helpers.Service
{
    public class Utilities : IUtilities
    {
        private readonly DatabaseContext _db;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly TokenSettings _tokenSettings;
        private readonly IHttpContextAccessor _http;

        public Utilities(DatabaseContext db, IConfiguration config, UserManager<User> userManager, IHttpContextAccessor http)
        {
            _db = db;
            _config = config;
            _userManager = userManager;
            var tokenSettingsSection = _config.GetSection("TokenSettings");
            _tokenSettings = tokenSettingsSection.Get<TokenSettings>();
            _http = http;
        }
        #region Token
        public async Task<TokenForResponseDto> GenerateNewTokenAsync(TokenForRequestDto tokenForRequestDto)
        {
            var user = await _userManager.FindByNameAsync(tokenForRequestDto.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, tokenForRequestDto.Password))
            {
                user = _db.Users.Single(u => u.Id == user.Id);
                var newRefreshToken = CreateRefreshToken(_tokenSettings.ClientId, user.Id, tokenForRequestDto.IsRemember);

                var oldRefreshToken = await _db.Tokens.Where(t => t.UserId == user.Id).ToListAsync();
                if (oldRefreshToken.Any())
                {
                    _db.Tokens.RemoveRange(oldRefreshToken);
                }

                _db.Tokens.Add(newRefreshToken);

                await _db.SaveChangesAsync();

                var accessToken = await CreateAccessTokenAsync(user, newRefreshToken.Value);

                return new TokenForResponseDto()
                {
                    Status = true,
                    Token = accessToken.Token,
                    RefreshToken = accessToken.RefreshToken,
                    User = user
                };
            }
            else
            {
                return new TokenForResponseDto()
                {
                    Status = false,
                    Message = "اطلاعات کاربری اشتباه است"
                };
            }
        }

        public async Task<TokenForResponseDto> CreateAccessTokenAsync(User user, string refreshToken)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenSettings.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            double tokenExpireTime = Convert.ToDouble(_tokenSettings.ExpireTime);
            var tokenDes = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _tokenSettings.Site,
                Audience = _tokenSettings.Audience,
                Expires = DateTime.Now.AddMinutes(tokenExpireTime),
                SigningCredentials = creds
            };

            var newAccessToken = tokenHandler.CreateToken(tokenDes);
            var encodedAccessToken = tokenHandler.WriteToken(newAccessToken);

            return new TokenForResponseDto()
            {
                Token = encodedAccessToken,
                RefreshToken = refreshToken
            };
        }

        public Token CreateRefreshToken(string clientId, string userId, bool isRemember)
        {
            return new Token()
            {
                ClientId = clientId,
                UserId = userId,
                IP = _http.HttpContext.Connection != null ? _http.HttpContext.Connection.RemoteIpAddress != null ? _http.HttpContext.Connection.RemoteIpAddress.ToString() : "no IP" : "no IP",
                Value = Guid.NewGuid().ToString("N"),
                ExpireTime = isRemember ? DateTime.Now.AddDays(7) : DateTime.Now.AddHours(6)
            };
        }

        public async Task<TokenForResponseDto> RefreshAccessTokenAsync(TokenForRequestDto tokenForRequestDto)
        {
            try
            {
                string ip = _http.HttpContext.Connection != null ? _http.HttpContext.Connection.RemoteIpAddress != null ? _http.HttpContext.Connection.RemoteIpAddress.ToString() : "no IP" : "no IP";
                var refreshToken = await _db.Tokens.FirstOrDefaultAsync(t => t.ClientId == _tokenSettings.ClientId && t.Value == tokenForRequestDto.RefreshToken && t.IP == ip);

                if (refreshToken == null)
                {
                    return new TokenForResponseDto()
                    {
                        Status = false,
                        Message = "خطا در اعتبارسنجی مجدد"
                    };
                }

                if (refreshToken.ExpireTime < DateTime.Now)
                {
                    return new TokenForResponseDto()
                    {
                        Status = false,
                        Message = "خطا در اعتبارسنجی مجدد"
                    };
                }

                var user = await _userManager.FindByIdAsync(refreshToken.UserId);

                if (user == null)
                {
                    return new TokenForResponseDto()
                    {
                        Status = false,
                        Message = "خطا در اعتبارسنجی مجدد"
                    };
                }

                var response = await CreateAccessTokenAsync(user, refreshToken.Value);

                return new TokenForResponseDto()
                {
                    Status = true,
                    Token = response.Token,
                };
            }
            catch (Exception ex)
            {
                return new TokenForResponseDto()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }
        #endregion

        public string FindLocalPathFromUrl(string url)
        {
            var temp = url.Replace("https://", "").Replace("http://", "").Split('/').Skip(2);

            return (temp.Aggregate("", (current, item) => current + (item + "\\"))).TrimEnd('\\');
        }
    }
}

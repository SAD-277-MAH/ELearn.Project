using ELearn.Data.Dtos.Site.Tokens;
using ELearn.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Common.Helpers.Interface
{
    public interface IUtilities
    {
        Task<TokenForResponseDto> GenerateNewTokenAsync(TokenForRequestDto tokenForRequestDto);

        Task<TokenForResponseDto> CreateAccessTokenAsync(User user, string refreshToken);

        Token CreateRefreshToken(string clientId, string userId, bool isRemember);

        Task<TokenForResponseDto> RefreshAccessTokenAsync(TokenForRequestDto tokenForRequestDto);

        string FindLocalPathFromUrl(string url);
    }
}

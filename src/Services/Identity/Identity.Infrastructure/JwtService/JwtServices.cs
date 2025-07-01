using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Domain.Abstractions;
using Identity.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.ConfigurationSettings;

namespace Identity.Infrastructure.JwtService;

public class JwtServices : IJwtServices
{
    private readonly JwtSettings _jwtSettings;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public JwtServices(IOptions<JwtSettings> jwtSettings)
    {
        _tokenHandler = new JwtSecurityTokenHandler();
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        var claims = GetClaims(user);
        var tokenDescriptor = GetTokenDescriptor(claims);
        var token = _tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return await Task.FromResult(_tokenHandler.WriteToken(token));
    }

    public async Task<string> GenerateRefreshTokenAsync()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return await Task.FromResult(Convert.ToBase64String(randomNumber));
    }

    #region Helpers

    private SecurityTokenDescriptor GetTokenDescriptor(List<Claim> claims)
    {
        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                SecurityAlgorithms.HmacSha256Signature)
        };
    }

    private List<Claim> GetClaims(User user)
    {
        return new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("phone", user.PhoneNumber),
            new("full_name", user.FirstName + " " + user.LastName),
            new("role", user.Role.ToString())
        };
    }

    #endregion
}
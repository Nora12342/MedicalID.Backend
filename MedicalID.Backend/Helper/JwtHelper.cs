using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public static class JwtHelper
{
    public static string GenerateToken(string userId, string username, string role, IConfiguration config)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId), 
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var keyBytes = Encoding.UTF8.GetBytes(config["Jwt:Key"]);
        if (keyBytes.Length < 32)
            throw new InvalidOperationException("JWT key must be at least 256 bits (32 bytes).");

        var key = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

using CourseManagementSystem.Core.DTOs.Auth;
using CourseManagementSystem.Core.Entities;
using CourseManagementSystem.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generate a JWT token with user claims
    /// </summary>
    /// <param name="user">User entity containing id, name, email, and role</param>
    /// <returns>JWT token string</returns>
    public string GenerateToken(User user)
    {
        // Read JWT settings from configuration
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        // Create security key from secret
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Define claims to include in the token
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleName),
            new Claim("UserId", user.Id.ToString()),
            new Claim("RoleId", user.RoleId.ToString())
        };

        // Create the JWT token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        // Return the serialized token string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Create a token response DTO with user information and token
    /// </summary>
    /// <param name="user">User entity</param>
    /// <param name="token">Generated JWT token</param>
    /// <returns>Complete token response DTO</returns>
    public TokenResponseDto CreateTokenResponse(User user, string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        return new TokenResponseDto
        {
            Token = token,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.RoleName
        };
    }
}
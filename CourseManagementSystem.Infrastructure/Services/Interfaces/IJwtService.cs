using CourseManagementSystem.Core.DTOs.Auth;
using CourseManagementSystem.Core.Entities;

namespace CourseManagementSystem.Infrastructure.Services.Interfaces
{
    public interface IJwtService
    {
        /// <summary>
        /// Generate a JWT token for a user
        /// </summary>
        /// <param name="user">User entity with role information</param>
        /// <returns>JWT token string</returns>
        string GenerateToken(User user);

        /// <summary>
        /// Create a complete token response DTO
        /// </summary>
        /// <param name="user">User entity</param>
        /// <param name="token">Generated JWT token</param>
        /// <returns>Token response DTO with user information</returns>
        TokenResponseDto CreateTokenResponse(User user, string token);
    }

}

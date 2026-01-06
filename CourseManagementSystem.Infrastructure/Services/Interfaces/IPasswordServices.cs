
namespace CourseManagementSystem.Infrastructure.Services.Interfaces
{
    public interface IPasswordService
    {
        /// <summary>
        /// Hash a plain text password using BCrypt
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <returns>Hashed password string</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verify a plain text password against a hashed password
        /// </summary>
        /// <param name="password">Plain text password to verify</param>
        /// <param name="hashedPassword">Hashed password to compare against</param>
        /// <returns>True if password matches, false otherwise</returns>
        bool VerifyPassword(string password, string hashedPassword);
    }

}
